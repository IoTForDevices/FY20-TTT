# Visualize a Data Stream in PowerBI
You have developed a device simulator that generates vibration data and other telemetry outputs for a conveyor belt system that takes packages and drops them off in mailing bins. You have built and tested a logging route that sends dat to Azure Blob storage.
The second route will be to an Event Hub, because an Event Hub is a convenient input to Stream Analytics. And Stream Analytics is a convenient way of handling anomaly detection, like the excessive vibration we're looking for in our scenario.
## In this lab
In this lab, you will learn to create a message route to an event hub, pass telemetry data to an Azure Analytics job to detect anomalies and present the results in a PowerBI dashboard. The lab is a continuation of Lab7. It uses the same simulated device (that you can find under LabFiles/07-Device-Message-Routing)
## Prerequisites
This lab assumes that you have the following resources available:
Resource Type | Resource Name
--------------|--------------
Resource Group | AZ-220-RG
IoT Hub | AZ-220-HUB-*{YOUR-ID}*
A valid Power BI Account, see the [original Lab description](https://github.com/MicrosoftLearning/AZ-220-Microsoft-Azure-IoT-Developer/blob/master/Instructions/Labs/LAB_AK_08-visualize-data-stream-in-power-bi.md) for instructions to sign up for PowerBI.
### Exercise 1: Add an Azure Event Hub Route and an Anomaly Query
In this exercise, we're going to add a query to the Stream Analytics job, and then use Microsoft Power BI to visualize the output from the query. The query searches for spikes and dips in the vibration data, reporting anomalies. We must create the second route, after first creating an instance of an Event Hubs namespace.
- Create an Event Hub Resource in a new *Namespace* called **vibrationNamespace-_"{YOUR-ID}"_** in the AZ-220-RG Resource Group. Limit the number of *Throughput units* to **1**.
- Create an Event Hub in the just created *Namespace* with the name **vibrationeventhubinstance**.
- Add a new message route to your IoT Hub to store telemetry messages, name it **vibrationTelemetryRoute** and call the endpoint **vibrationTelemetryEndpoint** and select **Event Hubs** as endpoint type.
- In the message routing blade, enable the newly created route and set the following query:
```sql
sensorID = "VSTel"
```
Make sure to start the simulated device from Lab07.
### Exercise 2: Add Telemetry Route
With this new IoT Hub route in place, we need to update our Stream Analytics job to handle the telemetry stream.
- In the Stream Analysis Job you created in Lab07, add an additional input stream, taking data from your just created Event Hub with the *input alias* set to **vibrationEventInput**.
- Also create an additional output connection to PowerBI with the *output alias* set to **vibrationBI**.
- Under *Dataset name* enter **vibrationDataset** and under *Table name* enter **vibrationTable** and select **User token** as *Authentication mode*
- Add the following SQL query before the already existing query:
  ```SQL
    WITH AnomalyDetectionStep AS
    (
        SELECT
            EVENTENQUEUEDUTCTIME AS time,
            CAST(vibration AS float) AS vibe,
            AnomalyDetection_SpikeAndDip(CAST(vibration AS float), 95, 120, 'spikesanddips')
                OVER(LIMIT DURATION(second, 120)) AS SpikeAndDipScores
        FROM vibrationEventInput
    )
    SELECT
        time,
        vibe,
        CAST(GetRecordPropertyValue(SpikeAndDipScores, 'Score') AS float) AS
        SpikeAndDipScore,
        CAST(GetRecordPropertyValue(SpikeAndDipScores, 'IsAnomaly') AS bigint) AS
        IsSpikeAndDipAnomaly
    INTO vibrationBI
    FROM AnomalyDetectionStep
  ```
Make sure to start the Stream Analysis Job.
### Create a PowerBI Dashboard
Now let's create a dashboard to visualize the query, using Microsoft Power BI.
- Navigate to https://app.powerbi.com/
- Check if the **vibrationDataset** is available and create a Dashboard with the name **Vibration Dash**.
- Add a custom streaming data tile pane (Gauge) for the vibrationDataset, and set the value of the Gauge control to **Vibe**, and change the Gauge's title to **Vibration**.
- Add a custom streaming data tile pane (Clustered bar chart) for the vibrationDataset, and set the value of the bar chart control to **SpikeAndDipScore**.
- Add a custom streaming data tile pane (Card) for the vibrationDataset, and set the value of the bar chart control to **IsSpikeAndDipAnomaly**, and change the Card's title to **Is anomaly?**.
- Add a custom streaming data tile pane (Line chart) for the vibrationDataset, and set the value of the line chart control to **IsSpikeAndDipAnomaly**, and change the Card's title to **Anomalies over the hour**. Make sure to set the Axis to **time** and set the Time window to display to **60 Minutes**.
> NOTE: Once you are done with this lab, make sure to stop the application and the Stream Analytics Job to limit the Azure Consumption on your subscription.

This concludes LAB08. If you want to have more detailed instructions for the lab, complete step-by-step instructions are [available here](https://github.com/MicrosoftLearning/AZ-220-Microsoft-Azure-IoT-Developer/blob/master/Instructions/Labs/LAB_AK_08-visualize-data-stream-in-power-bi.md).

