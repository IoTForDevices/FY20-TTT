# Generic Prerequisites

This folder contains a number of generic prerequisites, mostly as azcli scripts, that can be used to get started with a particular lab without running all previous labs. This allows all labs to be ran regardless of a specific order in which you run them.

If you are only running one lab at a time, it is  good idea to delete the resource group you created at the beginning of the lab to reduce the amount of Azure subscription costs for running the lab.

| Name | Description
| ---- | -----------
| 01-create-iothub.azcli | Create the TMP-AZ-220-RG resource group and an Azure IoT Hub (S1)
| 01-remove-iothub.azcli | Remove the TMP-AZ-220-RG Azure Resource Group + all resources it holds

