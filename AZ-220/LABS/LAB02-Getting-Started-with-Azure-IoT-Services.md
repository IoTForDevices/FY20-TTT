# Getting started with Azure IoT Services

## Lab Scenario
As an Azure IoT Developer for a leading gourmet cheese company named Contoso, you are tasked to start exploring Azure IoT services needed for connecting future devices.

## In this lab
In this lab, you will learn about a naming convention to create unique names, how to create an **IoT Hub** and get familiar with it and how to create a **Device Provision Service** and use it with your IoT Hub.
> NOTE: Make sure that you create all Azure services in a datacenter location closest to you.

### **Exercise 1: Naming Resources with a Unique Name**
Many Azure resources expose services that can be consumed across the web, which means they must have globally unique names. To achieve this without getting in conflicts with others, you should use a unique identifier that will be added to the end of the resource name. Chose a consistent unique identifier for all services you create, consisting of the following pattern: `YourInitialsYYMMDD`, where you follow your initials by the current date. Keep this postfix consistent for all services you create throughout your preparation for AZ220 instead of updating the date each day. In scripts that you might be use throughout these lab exercises, this postfix will be refered to as `{YOUR-ID}`.

### **Exercise 2: Create an IoT Hub using the Azure portal**
The Azure IoT Hub is a fully managed service that enables reliable and secure bidirectional communications between IoT devices and Azure. In this exercise we create one with a globally unique name that you can use throughout all labs that are part of this AZ220 preparation.
- Create an IoT Hub with a unique name, as explained previously and make sure to add it to the Resource Group you created in Lab01.
- Make sure that you create a **S1: Standard tier** IoT Hub, with the **Number of S1 IoT Hub units** set to **1** and the **Device-to-cloud partitions** set to **4**.

### **Exercise 3: Examine the IoT Hub Service**
Take a look at the different capabilities of the IoT Hub you created, by examining (and understanding) the different items you can find on the different IoT Hub blades, starting with the **IoT Hub Overview blade**.
### **Exercise 4: Create a Device Provisioning Service using the Azure Portal**
The Azure IoT Hub Device Provisioning Service is a helper service for IoT Hub that enables zero-touch, just-in-time provisioning to the right IoT hub without requiring human intervention. In this exercise you are going to create an instance of the IoT Hub Device Provisioning Service (DPS).

- Create an instance of DPS in the Azure portal with a unique name **(AZ-220-DPS-*{YOUR-ID}*)** and make sure you add it to the Resource Group you created in Lab01.
- Link your IoT Hub and Device Provisioning Service, using the **iothubowner** Access Policy.

### **Exercise 5: Examine the Device Provisioning Service**
Take a look at the different capabilities of the Device Provisioning Service you created, by examining (and understanding) the different items you can find, starting with the **Overview blade**.

This concludes LAB02. If you want to have more detailed instructions for the lab, complete step-by-step instructions are [available here](https://github.com/MicrosoftLearning/AZ-220-Microsoft-Azure-IoT-Developer/blob/master/Instructions/Labs/LAB_AK_02-getting-started-with-azure-iot-services.md).
