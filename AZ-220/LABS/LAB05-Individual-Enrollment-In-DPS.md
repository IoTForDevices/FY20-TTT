# Individual Enrollment of a Device in DPS
## Lab Scenario
Contoso's Asset Monitoring and Tracking Solution will require an IoT Device that has sensors for tracking location, temperature, pressure to be added in product transport boxes.

When a new box enters the system, it is equipped with the new IoT Device. The device needs to be auto-provisioned to IoT Hub using Device Provisioning Service. When an existing device needs to be removed (for instance when the device retires), it also needs to be "decommissioned" through DPS.
## In this lab
In this lab, you will, create an individual enrollment within Azure Device Provisioning Service (DPS) to automatically connect a pre-built simulated device to Azure IoT Hub. You will also fully retire the device by removing it from both DPS and IoT Hub.
## Prerequisites
This lab assumes that you have the following resources available:
Resource Type | Resource Name
--------------|--------------
Resource Group | AZ-220-RG
IoT Hub | AZ-220-HUB-*your unique identifier*
Device Provisioning Service | AZ-220-DPS-*your unique identifier*
### Exercise 1: Create a new individual enrollment in DPS
In this exercise, you will create a new individual enrollment for a device within the Device Provisioning Service (DPS) using *symmetric key attestation*. 
- Go to your Device Provisioning Service and find **Add individual enrollment**.
- Select **Symetric Key** and let DPS generate keys for the device enrollment.
- The **Registration ID** for your device must be `PDSSimulatedDevice1`.
- In the **Initial Device Twin State** add a desired property called `telemetryDelay` with value `2`
### Exercise 2: Configure the Simulated Device
You will now configure a Simulated Device written in C# to connect to Azure IoT using the individual enrollment created in the previous unit. You will also add code to the Simulated Device that will read and update the device configuration based on the device twin within Azure IoT Hub.
- Take the ID Scope for DPS and save it for later use.
- Using Visual Studio Code, open the /LabFiles folder and the Program.cs file.
  - Add the saved ID Scope to the Program.cs file
  - Assign the **registrationId** variable with your device **Registration ID**
  - Set the values for both the PrimaryKey and SecondaryKey in the Program.cs file.
- Replace `// TODO 1: Setup OnDesiredPropertyChanged Event Handling` comment to wire up an event handler.
- Implement the `OnDesiredPropertyChanged` method, reading the desired property, assigning it to the `_telemetryDelay` variable and setting the reportedProperty.
- Replace `// TODO 2: Load device twin properties` comment with code to read the device twin property desired state during device startup.
### Exercise 3: Test the Simulated Device
In this exercise, you will run the Simulated Device and verify it's sending sensor telemetry to Azure IoT Hub. You will also update the delay at which telemetry is sent to Azure IoT Hub by updating the device twin for the simulated device within Azure IoT Hub.
- Run the simulated device from within the Visual Studio Code terminal
- Inside an Azure Cloud Shell, run the following command to view D2C messages being sent to the Azure IoT Hub endpoint:
```sh
az iot hub monitor-events --hub-name {IoTHubName} --device-id DPSSimuatedDevice1
```
- Change the telemetryDelay Device Twin value from 2 to 5 and verify that only every five seconds a D2C message is send.
### Exercise 4: Retire the Device
Now you will perform the necessary tasks to retire the device from both the Device Provisioning Service (DPS) and Azure IoT Hub. To fully retire an IoT Device from an Azure IoT solution it must be removed from both of these services.
- Delete the `DPSSimulatedDevice1` from DPS
- Retire the device from your IoT Hub.
This concludes LAB05. If you want to have more detailed instructions for the lab (when you are installing on Windows), complete step-by-step instructions are [available here](https://github.com/MicrosoftLearning/AZ-220-Microsoft-Azure-IoT-Developer/blob/master/Instructions/Labs/LAB_AK_05-individual-enrollment-of-device-in-dps.md).