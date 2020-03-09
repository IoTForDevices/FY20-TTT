# Device Message Routing
Message routing enables you to send messages from your devices to cloud services in an automated, scalable, and reliable manner. Message routing can be used for:
- Sending device telemetry messages as well as events namely, device lifecycle events, and device twin change events to built-in-endpoint and custom endpoints.
- Filtering data before routing it to various endpoints by applying rich queries. Message routing allows you to query on the message properties and message body as well as device twin tags and device twin properties.
## In this lab
In this lab, you will learn to create a message route to blob storage and create another message route to an Azure Analytics job.
## Prerequisites
This lab assumes that you have the following resources available:
Resource Type | Resource Name
--------------|--------------
Resource Group | AZ-220-RG
IoT Hub | AZ-220-HUB-*{YOUR-ID}*
### Exercise 1: Create a new Device Identity and a Simulated Device
- Inside the Azure Cloud Shell, now create a new Device Identity in your IoT Hub:
  ```sh
  az iot hub device-identity create --hub-name {IoTHubName} --device-id VibrationSensorId
  az iot hub device-identity show-connection-string --hub-name {IoTHubName} --device-id VibrationSensorId --output table
  ``` 
You will now configure a simuated device written in C# to connect to your Azure IoT Hub using the Device ID and Shared Access Key created in exercise 1.
  - Using Visual Studio Code, open the /LabFiles folder and the Program.cs file.
  - Add your connection string to the Program.cs file
  - Run the simulated device from within the Visual Studio Code terminal
### Exercise 2: Create a Message Route to Azure Blob Storage
The architecture of our vibration monitoring system requires data be sent to two destinations: storage and analysis. Azure IoT Hub provides a great method of directing data to the right service, through *message routing*.
- Add a new message route to your IoT Hub to store logging messages, name it **vibrationLoggingRoute** and call the endpoint **vibrationLogEndpoint** and select **Pick a container** as storage location.
  - Create a new *StorageV2 (general purpose V2)* Storage Account in your AZ-220-RG Resource Group with the name **vibrationstore-_"{YOUR-ID}"_**
  - Make sure to set the **Performance** field to *Standard* and the **Replication** filed to *Locally-redundant storage (LRS)*.
  - Create a new container in the storage account you just created with the name **vibrationcontainer**. Make sure that the **Public access level** is set to *Private (no anonymous access)*.
  - In the message routing blade, enable the newly created route and set the following query:
    ```sql
    sensorID = "VSLog"
    ```
### Exercise 3: Logging Route verifcation with Azure Stream Analytics
To verify that the logging route is working as expected, we will create a Stream Analytics job that routes logging messages to Blob storage, which can then be validated using Storage Explorer in the Azure Portal.
> NOTE: It may seem odd to be routing data to storage, then again sending it to storage through Azure Stream Analytics. In a production scenario, you wouldn't have both paths long-term. Instead, the second path that we're creating here would not exist. We're using it here simply as a way to demonstrate Azure Stream Analytics in an easy-to-validate way in a lab environment.
- Create a Stream Analytics Job with the name **vibrationJob**, and with the *Streaming Units* set to **1**.
- Create aliases for **stream input** (*vibrationInput*), **stream output** (*vibrationOutput*). Input should come from your IoT Hub, output should go to your **vibrationcontainer**. Store the data as **Line separated JSON**.
- In the query edit pane, add the following query:
  ```sql
  SELECT
    *
  INTO
    vibrationOutput
  FROM
    vibrationInput
  ```
- Test the logging route by looking at json data that was send by the Stream Analytics job. This data contains only telemetry messages, no logging messages.

> NOTE: Once you are done with this lab, make sure to stop the application and the Stream Analytics Job to limit the Azure Consumption on your subscription.

This concludes LAB07. If you want to have more detailed instructions for the lab, complete step-by-step instructions are [available here](https://github.com/MicrosoftLearning/AZ-220-Microsoft-Azure-IoT-Developer/blob/master/Instructions/Labs/LAB_AK_07-analyze-message-data-in-real-time.md).
