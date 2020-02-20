# Automatically provision IoT devices securely and at scale with DPS
## Lab Scenario
Our asset tracking solution is getting bigger, and provisioning devices one by one does not scale. We want to use Device Provisioning Service (DPS) to enroll many devices automatically and securely using X.509 certificate authentication.
## In this lab
In this lab, you will setup a *group enrollment* within DPS using a CA X.509 certificate. You will also configure a simulated IoT device that will authenticate with DPS using a device certificate signed by the CA Certificate.
## Prerequisites
This lab assumes that you have the following resources available:
Resource Type | Resource Name
--------------|--------------
Resource Group | AZ-220-RG
IoT Hub | AZ-220-HUB-*your unique identifier*
Device Provisioning Service | AZ-220-DPS-*your unique identifier*
### Exercise 1: Generate and Configure X.509 CA Certificates using OpenSSL
In this exercise, you will generate an X.509 CA Certificate using OpenSSL within the Azure Cloud Shell. This certificate will be used to configure the Group Enrollment within DPS.
- Execute the script `prep-gen-x509-certs.h` that can be found in the [scripts folder](./LABS/06-Automatic-Enrollment-with-DPS/Scripts) in this repository, by uploading it to an Azure Cloud Shell and executing it there.
- Run the following command inside the ~/certificates directory of the Azure Cloud Shell to generate the CA and intermediate certificates and download the root certificate to your local machine:
  ```sh
  ./certGen.sh create_root_and_intermediate
  download ~/certificates/certs/azure-iot-test-only.root.ca.cert.pem
  ```
- Add the downloaded certificate to your Device Provisioning Service.
- Generate a verification code for the just installed root certificate and copy the generated verification code.
- Run the following command in the Azure Cloud Shell to generate a verification certificate and download it to your local machine:
  ```sh
  ./certGen.sh create_verification_certificate <verification-code>
  download ~/certificates/certs/verification-code.cert.pem
  ```
- Verify the root certificate with the verication certificate you just downloaded in the Device Provision Service.
### Exercise 2: Create a Group Enrollment (X.509 Certificate) in DPS
In this exercise, you will create a new individual enrollment for a device within DPS using *certificate attestation*.
- Add an enrollment group called "simulated-devices" to your Device Provision Service and make sure to assign the just created root certificate to it.
- In the **Initial Device Twin State** add a desired property called `telemetryDelay` with value `2`
### Exercise 3: Configure simulated device with X.509 certificate
In this exercise, you will configure a simulated device written in C# to connect to your Azure IoT Hub via DPS using an X.509 certificate. 
- Run the following command inside the ~/certificates directory of the Azure Cloud Shell to generate the CA and intermediate certificates and download the root certificate to your local machine:
  ```sh
  ./certGen.sh create_device_certificate simulated-device1
  download ~/certificates/certs/new-device.cert.pfx
  ```
- Take the ID Scope for DPS and save it for later use.
- Copy the downloaded new-device.cert.pfx X.509 device certificate file to the /LabFiles directory
- Using Visual Studio Code, open the /LabFiles folder and the SimulatedDevice.scproj file.
- Within the SimulatedDevice.csproj file, ensure the following block of XML exists within the <ItemGroup> tag of the file:
  ```xml
  <None Update="new-device.cert.pfx" CopyToOutputDirectory="PreserveNewest" />
  ```
- Add the saved ID Scope to the Program.cs file
- Verify that the X.509 related variables are allready correctly set. Specifically, look at the `s_certificateFileName` and the `s_certificatePassword` variables.
- Take a look at the `Main` method to understand how to load an X.509 certificate and to find out which transport protocol is used to communicate with your IoT Hub.
### Exercise 4: Handle Device Twin Desired Property changes
In this exercise, you will modify the simulated device source code to include an event handler to update device configurations based on device twin desired property changes sent to the device from Azure IoT Hub. The changes are simular to what you did in LAB05.
- Replace `// TODO 1: Setup OnDesiredPropertyChanged Event Handling` comment to wire up an event handler.
- Implement the `OnDesiredPropertyChanged` method, reading the desired property, assigning it to the `_telemetryDelay` variable and setting the reportedProperty.
- Replace `// TODO 2: Load device twin properties` comment with code to read the device twin property desired state during device startup.
### Exercise 4: Build and run the Device code
In this exercise, you will run the Simulated Device and verify it's sending sensor telemetry to Azure IoT Hub. You will also update the delay at which telemetry is sent to Azure IoT Hub by updating the device twin for the simulated device within Azure IoT Hub.
- Run the simulated device from within the Visual Studio Code terminal
- Inside an Azure Cloud Shell, run the following command to view D2C messages being sent to the Azure IoT Hub endpoint:
```sh
az iot hub monitor-events --hub-name {IoTHubName} --device-id DPSSimuatedDevice1
```
- Change the telemetryDelay Device Twin value from 1 to 2 and verify that only every five seconds a D2C message is send.
### Exercise 4: Retire the Device
Now you will perform the necessary tasks to retire the enrollment group and its devices from both the Device Provisioning Service and Azure IoT Hub.
- Set the **Enable entry** in the **Enrollment Group Details** blade of DPS to **Disable** and save the changes. This temporarily disables devices within the Enrollment Group.
- Permanently delete the Enrollment Group. This removes it completely from DPS.
> NOTE: If you delete an enrollment group for a certificate, devices that have the certificate in their certificate chain might still be able to enroll if a different, enabled enrollment group still exists for the root certificate or another intermediate certificate higher up in their certificate chain.
- Retire the device from your IoT Hub, by deleting it from the list of devices.
- Verify the retirement by again run the Simulated Device. This time it should thrown an exception.
This concludes LAB06. If you want to have more detailed instructions for the lab (when you are installing on Windows), complete step-by-step instructions are [available here](https://github.com/MicrosoftLearning/AZ-220-Microsoft-Azure-IoT-Developer/blob/master/Instructions/Labs/LAB_AK_06-automatic-enrollment-of-devices-in-dps.md).

