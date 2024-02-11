[![Logo](https://raw.githubusercontent.com/k-society/KSociety.SharpCubeProgrammer/master/docs/K-Society__Logo_vs-negative.png)](https://github.com/K-Society)

[![build status](https://img.shields.io/github/actions/workflow/status/K-Society/KSociety.SharpCubeProgrammer/build.yml?branch=develop)](https://github.com/K-Society/KSociety.SharpCubeProgrammer/actions/workflows/build.yml?query=branch%3Adevelop) 
[![latest version](https://img.shields.io/nuget/v/KSociety.SharpCubeProgrammer)](https://www.nuget.org/packages/KSociety.SharpCubeProgrammer)
[![download count](https://img.shields.io/nuget/dt/KSociety.SharpCubeProgrammer)](https://www.nuget.org/stats/packages/KSociety.SharpCubeProgrammer?groupby=Version)
[KSociety.SharpCubeProgrammer Home](https://github.com/K-Society/KSociety.SharpCubeProgrammer)

# KSociety.SharpCubeProgrammer

KSociety.SharpCubeProgrammer is a wrapper for CubeProgrammer_API v2.15.0.

It makes use of several 3rd party tools:

- STM32 Cube Programmer
   You can find the source, licensing information and documentation [here](https://www.st.com/en/development-tools/stm32cubeprog.html).

## Introduction

This is a C# wrapper for STM32 CubeProgrammer_API v2.15.0 (not fully tested).
This package does not contain any C/C++ runtimes (MSVC), and is meant to run on Windows operating systems only (for now).
Please make sure you have updated the firmware of your ST-LINK V2 / V3, you can do this using STM32CubeProgrammer.
The STM32 CubeProgrammer_API is a C-library, created by ST for ST-Link access to micro-controllers 
for the purpose of flash downloads or general memory access. 
The ST-Link drivers is required, and can be downloaded from st.com and installed [(STSW-LINK009)](https://www.st.com/en/development-tools/stsw-link009.html).
This has been tested on Windows 10, you don't need to install STM32CubeProgrammer.

### KSociety.SharpCubeProgrammer
STM32CubeProgrammer_API C# wrapper, the first wrapper for C#. Any suggestions are welcome.

## Get Packages

You can get KSociety.SharpCubeProgrammer by [grabbing the latest NuGet package](https://www.nuget.org/packages/KSociety.SharpCubeProgrammer/).

## Currently supported features

## STLINK functions
- GetStLinkList
- GetStLinkEnumerationList
- ConnectStLink
- Reset

## Bootloader functions 
- GetDfuDeviceList
- ConnectDfuBootloader

## General purposes functions
- SetDisplayCallbacks
- SetVerbosityLevel
- CheckDeviceConnection
- GetDeviceGeneralInf
- ReadMemory
- WriteMemory
- EditSector
- DownloadFile
- Execute
- MassErase
- SectorErase
- ReadUnprotect
- TzenRegression (does not exist)
- GetTargetInterfaceType
- GetCancelPointer
- FileOpen
- Verify
- VerifyMemory
- SaveFileToFile
- SaveMemoryToFile
- Disconnect
- DeleteInterfaceList
- AutomaticMode
- GetStorageStructure

## Option Bytes functions
- SendOptionBytesCmd
- InitOptionBytesInterface
- FastRomInitOptionBytesInterface
- ObDisplay

## Loaders functions
- SetLoadersPath
- SetExternalLoaderPath
- GetExternalLoaders
- RemoveExternalLoader
- DeleteLoaders

## STM32WB specific functions
- GetUID64
- FirmwareDelete
- FirmwareUpgrade
- StartWirelessStack
- UpdateAuthKey
- AuthKeyLock
- WriteUserKey
- AntiRollBack
- StartFus
- UnlockChip

## STM32MP specific functions
- ProgramSsp

## STM32 HSM specific functions
- GetHsmFirmwareID
- GetHsmCounter
- GetHsmState
- GetHsmVersion
- GetHsmType
- GetHsmLicense

## Prerequisites

- Visual Studio 2022 (17.8.3 or higher) with the following installed:

| Workloads |
| :-------- |
| .NET desktop development |
| Desktop development with C++ |

| Individual components |
| :-------------------- |
| MSVC v143 - VS 2022 C++ x64/x86 |

## Get Started

Examples include the QuickStart project, is a very basic example.

- Creates a new instance of the CharpCubeProgrammer class:

```csharp
var cubeProgrammerApi = new SharpCubeProgrammer.CubeProgrammerApi();
```

- Set up the logging system:

```csharp
var displayCallBacks = new DisplayCallBacks
{
    InitProgressBar = InitProgressBar,
    LogMessage = ReceiveMessage,
    LoadBar = ProgressBarUpdate
};

cubeProgrammerApi.SetDisplayCallbacks(displayCallBacks);

cubeProgrammerApi.SetVerbosityLevel(CubeProgrammerVerbosityLevel.CubeprogrammerVerLevelDebug);
```

- Retrieves all ST-LINK connected probes:

```csharp
var stLinkList = cubeProgrammerApi.GetStLinkEnumerationList();
```

- Connect:

```csharp
if (stLinkList.Any())
{
    var stLink = stLinkList.First();
    stLink.ConnectionMode = DebugConnectionMode.UnderResetMode;
    stLink.Shared = 0;

    var connectionResult = cubeProgrammerApi.ConnectStLink(stLink);

    //...

}
else
{
    Console.WriteLine("No ST-Link found!");
}
```

- Retrieve general info:

```csharp
if (connectionResult.Equals(CubeProgrammerError.CubeprogrammerNoError))
{
    var generalInfo = cubeProgrammerApi.GetDeviceGeneralInf();
    if (generalInfo != null)
    {
        Console.WriteLine("INFO: \n" +
                            "Board: {0} \n" +
                            "Bootloader Version: {1} \n" +
                            "Cpu: {2} \n" +
                            "Description: {3} \n" +
                            "DeviceId: {4} \n" +
                            "FlashSize: {5} \n" +
                            "RevisionId: {6} \n" +
                            "Name: {7} \n" +
                            "Series: {8} \n" +
                            "Type: {9}",
            generalInfo.Value.Board,
            generalInfo.Value.BootloaderVersion,
            generalInfo.Value.Cpu,
            generalInfo.Value.Description,
            generalInfo.Value.DeviceId,
            generalInfo.Value.FlashSize,
            generalInfo.Value.RevisionId,
            generalInfo.Value.Name,
            generalInfo.Value.Series,
            generalInfo.Value.Type);
    }
}
```

- Send option bytes:

```csharp
var sendOptionBytesCmd = cubeProgrammerApi.SendOptionBytesCmd("-ob RDP=170");
```

- Erase:

```csharp
var massErase = cubeProgrammerApi.MassErase("");
```

- Flash:

```csharp
var downloadFile = cubeProgrammerApi.DownloadFile(firmwarePath, "0x08000000", 1U, 1U);
```

- Run:

```csharp
var execute = cubeProgrammerApi.Execute("0x08000000");
```

- Disconnect:

```csharp
cubeProgrammerApi.Disconnect();
```

- Delete interface list:

```csharp
cubeProgrammerApi.DeleteInterfaceList();
```

## License
The project is under Microsoft Reciprocal License [(MS-RL)](http://www.opensource.org/licenses/MS-RL)

## Dependencies

List of technologies, frameworks and libraries used for implementation:

- [Microsoft.Bcl.AsyncInterfaces](https://www.nuget.org/packages/Microsoft.Bcl.AsyncInterfaces)
- [Microsoft.Extensions.Logging.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions)