# LAB03 - Setup the Development Environment
## Lab Scenario
As one of the developers at Contoso, setting up your development environment is an important step before starting to build in IoT solution. You need to prepare a work environment that you can use to develop your IoT solution, both on the Azure cloud side and for your local work environment. Your team has decided to use Visual Studio Code as the primary coding tool for device management.
> NOTE: If you checked the [pre-requisites](../README.md) for AZ220 and already have the tools installed, you can skip Lab03.

## In this lab
In this lab you will:
- Install the .NET Core 3 SDK, Azure CLI, and the Visual Studio Code (VSCode) editor.
- Install the VSCode extensions for developing Azure IoT solutions.
- Verify your Development Environment setup

### **Exercise 1: Install Developer Tools and Products**

#### On Windows
- Install the .NET Core SDK using the [Windows Installer](https://dotnet.microsoft.com/download).
- Install [Visual Studio Code](https://code.visualstudio.com/Download), together with the following extensions:
  - Azure IoT Tools (vsciot-vscode.azure-iot-tools) by Microsoft
  - C# for Visual Studio Code (ms-vscode.csharp) by Microsoft
- Install the [Azure CLI 2.0](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)
- Install the Azure CLI extension for IoT by entering the following command in a command prompt:
  ```batchfile
  az extension add --name azure-cli-iot-ext
  ```
- From inside a elevated Windows Powershell command prompt, enter the following command to install the Azure PowerShell:
  ```powershell
  Install-Module -Name AzureRM -AllowClobber
  ```
  You need to make sure that you have at least version 1.1.2.0 of PowerShell in order to use the Azure PowerShell
- After properly installing the AzureRM module, you need to load the module into your (non elevated) PowerShell session by entering the following command:
  ```powershell
  Import-Module -Name AzureRM
  ```
#### On Linux
- Install the .NET Core SDK using the [Ubunto 18.04 Package Manager](https://docs.microsoft.com/en-us/dotnet/core/install/linux-package-manager-ubuntu-1804).
- Use Visual Studio Code for [Remote Development using SSH](https://code.visualstudio.com/docs/remote/ssh)
- Install the following Visual Studio Extensions for remote usage:
  - Azure IoT Tools (vsciot-vscode.azure-iot-tools) by Microsoft
  - C$ for Visual Studio Code (ms-vscode.csharp) by Microsoft
- Install the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-apt?view=azure-cli-latest)
- Install the Azure CLI extension for IoT by entering the following command in a bash shell:
  ```sh
  az extension add --name azure-cli-iot-ext
  ```
  - Because you are running on a Linux VM, you cannot install the Azure PowerShell, because it depends on Windows Powershell. However, you can still use Azure PowerShell scripts, but you have to make sure to execute them in an [Azure Cloud Shell](https://azure.microsoft.com/en-us/features/cloud-shell/).

### **Exercise 2: Verify Development Environment Setup**
Depending on the operating system you are using, run the following commands from either a bash shell or a command prompt:
```batchfile
az --version
dotnet --version
```
The azure-cli should at least have version 2.0.64, the dotnet core SDK should have version 3.0 or higher.

This concludes LAB03. If you want to have more detailed instructions for the lab (when you are installing on Windows), complete step-by-step instructions are [available here](https://github.com/MicrosoftLearning/AZ-220-Microsoft-Azure-IoT-Developer/blob/master/Instructions/Labs/LAB_AK_03-set-up-the-development-environment.md).
