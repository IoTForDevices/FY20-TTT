// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Provisioning.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace X509CertificateSimulatedDevice
{
    class Program
    {
        // ////////////////////////////////////////////////////////

        // Azure Device Provisioning Service (DPS) Global Device Endpoint
        private const string GlobalDeviceEndpoint = "global.azure-devices-provisioning.net";

        // Azure Device Provisioning Service (DPS) ID Scope
        private static string dpsIdScope = "<DPS-ID-Scope>";

        // Certificate (PFX) File Name
        private static string s_certificateFileName = "new-device.cert.pfx";
        
        // Certificate (PFX) Password
        private static string s_certificatePassword = "1234";

        // NOTE: For the purposes of this example, the s_certificatePassword is
        // hard coded. In a production device, the password will need to be stored
        // in a more secure manner. Additionally, the certificate file (PFX) should
        // be stored securely on a production device using a Hardware Security Module.

        // ////////////////////////////////////////////////////////

        public static int Main(string[] args)
        {
            X509Certificate2 certificate = LoadProvisioningCertificate();

            using (var security = new SecurityProviderX509Certificate(certificate))
            {
                using (var transport = new ProvisioningTransportHandlerAmqp(TransportFallbackType.TcpOnly))
                {
                    ProvisioningDeviceClient provClient =
                        ProvisioningDeviceClient.Create(GlobalDeviceEndpoint, dpsIdScope, security, transport);


                    var provisioningDeviceLogic = new ProvisioningDeviceLogic(provClient, security);
                    provisioningDeviceLogic.RunAsync().GetAwaiter().GetResult();
                }
            }

            return 0;
        }

        private static X509Certificate2 LoadProvisioningCertificate()
        {
            var certificateCollection = new X509Certificate2Collection();
            certificateCollection.Import(s_certificateFileName, s_certificatePassword, X509KeyStorageFlags.UserKeySet);

            X509Certificate2 certificate = null;

            foreach (X509Certificate2 element in certificateCollection)
            {
                Console.WriteLine($"Found certificate: {element?.Thumbprint} {element?.Subject}; PrivateKey: {element?.HasPrivateKey}");
                if (certificate == null && element.HasPrivateKey)
                {
                    certificate = element;
                }
                else
                {
                    element.Dispose();
                }
            }

            if (certificate == null)
            {
                throw new FileNotFoundException($"{s_certificateFileName} did not contain any certificate with a private key.");
            }

            Console.WriteLine($"Using certificate {certificate.Thumbprint} {certificate.Subject}");
            return certificate;
        }
    }


    // The ProvisioningDeviceLogic class contains the device logic to read from the
    // simulated Device Sensors, and send Device-to-Cloud messages to the Azure IoT
    // Hub. It also contains the code that updates the device with changes to the
    // Device Twin "telemetryDelay" Desired Property.
    public class ProvisioningDeviceLogic
    {
        #region Constructor

        readonly ProvisioningDeviceClient _provClient;
        readonly SecurityProvider _security;
        DeviceClient iotClient;

        // Delay between Telemetry readings in Seconds (default to 1 second)
        private int _telemetryDelay = 1;

        public ProvisioningDeviceLogic(ProvisioningDeviceClient provisioningDeviceClient, SecurityProvider security)
        {
            _provClient = provisioningDeviceClient;
            _security = security;
        }

        #endregion

        public async Task RunAsync()
        {
            Console.WriteLine($"RegistrationID = {_security.GetRegistrationID()}");

            // Register the Device with DPS
            Console.Write("ProvisioningClient RegisterAsync . . . ");
            DeviceRegistrationResult result = await _provClient.RegisterAsync().ConfigureAwait(false);

            Console.WriteLine($"Device Registration Status: {result.Status}");
            Console.WriteLine($"ProvisioningClient AssignedHub: {result.AssignedHub}; DeviceID: {result.DeviceId}");

            // Verify Device Registration Status
            if (result.Status != ProvisioningRegistrationStatusType.Assigned)
            {
                throw new Exception($"DeviceRegistrationResult.Status is NOT 'Assigned'");
            }

            // Create x509 DeviceClient Authentication
            Console.WriteLine("Creating X509 DeviceClient authentication.");
            var auth = new DeviceAuthenticationWithX509Certificate(result.DeviceId, (_security as SecurityProviderX509).GetAuthenticationCertificate());


            Console.WriteLine("Simulated Device. Ctrl-C to exit.");
            using (iotClient = DeviceClient.Create(result.AssignedHub, auth, TransportType.Amqp))
            {
                // Explicitly open DeviceClient to communicate with Azure IoT Hub
                Console.WriteLine("DeviceClient OpenAsync.");
                await iotClient.OpenAsync().ConfigureAwait(false);


                // TODO 1: Setup OnDesiredPropertyChanged Event Handling to receive Desired Properties changes
                

                // TODO 2: Load Device Twin Properties since device is just starting up


                // Start reading and sending device telemetry
                Console.WriteLine("Start reading and sending device telemetry...");
                await SendDeviceToCloudMessagesAsync(iotClient);

                // Explicitly close DeviceClient
                Console.WriteLine("DeviceClient CloseAsync.");
                await iotClient.CloseAsync().ConfigureAwait(false);
            }
        }    

        private async Task SendDeviceToCloudMessagesAsync(DeviceClient deviceClient)
        {
            // Initial telemetry values
            double minTemperature = 20;
            double minHumidity = 60;
            double minPressure = 1013.25;
            double minLatitude = 39.810492;
            double minLongitude = -98.556061;
            Random rand = new Random();

            while (true)
            {
                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;
                double currentPressure = minPressure + rand.NextDouble() * 12;
                double currentLatitude = minLatitude + rand.NextDouble() * 0.5;
                double currentLongitude = minLongitude + rand.NextDouble() * 0.5;

                // Create JSON message
                var telemetryDataPoint = new
                {
                    temperature = currentTemperature,
                    humidity = currentHumidity,
                    pressure = currentPressure,
                    latitude = currentLatitude,
                    longitude = currentLongitude
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                // Add a custom application property to the message.
                // An IoT hub can filter on these properties without access to the message body.
                message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

                // Send the telemetry message
                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                // Delay before next Telemetry reading
                await Task.Delay(this._telemetryDelay * 1000);
            }
        }

    }
}
