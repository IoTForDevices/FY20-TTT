# AZ-220 Practise Labs
The labs for AZ-220 are available to find out if you understand IoT related services enough to take the AZ-220 exam. A full description, containing multiple labs, can be found in the official [AZ-220 Labs Github repository](https://github.com/MicrosoftLearning/AZ-220-Microsoft-Azure-IoT-Developer). This repository contains a description for the same labs, but in a less step-by-step approach. The description is of a higher level. This can be useful for more experienced IoT Solution Architects / Developers to test their knowledge.

## Prerequisites for Windows 10
All of the labs can be executed from inside a virtual machine. All software that needs to be installed on the VM (or a physical machine) can be [found here](https://github.com/MicrosoftLearning/AZ-220-Microsoft-Azure-IoT-Developer/blob/master/lab.md). In addition to the required software, it makes sense to also install the Azure IoT CLI extension to see messages that devices are sending to IoT Central and to observe changes in the device twin.
> NOTE: If you are using an Azure Virtual Machine to host the labs, make sure to select a v3 VM (for instance D2v3). Only v3 supports nested virtualization, which is needed to run Docker for Windows.
## Prerequisites for Linux VM (Ubuntu 18.4)
- Install the .NET Core SDK through the [Ubunto 18.04 Package Manager](https://docs.microsoft.com/en-us/dotnet/core/install/linux-package-manager-ubuntu-1804)
- Use Visual Studio Code for [Remote Development using SSH](https://code.visualstudio.com/docs/remote/ssh)
- Install the following Visual Studio Extensions for remote usage:
  - Azure IoT Tools (vsciot-vscode.azure-iot-tools) by Microsoft
  - C$ for Visual Studio Code (ms-vscode.csharp) by Microsoft
- Install the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-apt?view=azure-cli-latest)
- Install the Azure CLI extension for IoT by entering the following command in a bash shell:
  ``` bash
  az extension add --name azure-cli-iot-ext
  ```
- Since you are running the labs on a Linux VM, you cannot install Azure PowerShell, because it depends on Windows Powershell. However, you can still use Azure PowerShell scripts, but you have to make sure to execute them in an [Azure Cloud Shell](https://azure.microsoft.com/en-us/features/cloud-shell/).