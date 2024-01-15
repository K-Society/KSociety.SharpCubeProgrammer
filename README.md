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
This has been tested on Windows 10, you don't need to install cubeprogrammer.

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

- Register SharpCubeProgrammer as service with Autofac IoC:

Create the module for Autofac in a dedicated file (in this example under the Bindings folder) with the following contents:

```csharp
namespace MyNamespace.Bindings
{
    using Autofac;
    using KSociety.SharpCubeProgrammer;
    using KSociety.SharpCubeProgrammer.Interface;

    public class ProgrammerApi : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CubeProgrammerApi>().As<ICubeProgrammerApi>().SingleInstance();
        }
    }
}
```

Register the module:

```csharp
builder.RegisterModule(new Bindings.ProgrammerApi());
```

- Get ST-Link list and connect:

```csharp
var stLinkList = _cubeProgrammerApi.GetStLinkList();
var stLink = (KSociety.SharpCubeProgrammer.Struct.DebugConnectParameters)stLinkList.First().Clone();
var connectionResult = _cubeProgrammerApi.ConnectStLink(stLink);
```

- GeneralInfo:

```csharp
var generalInfo = _cubeProgrammerApi.GetDeviceGeneralInf();
```

- Erase:

```csharp
var massErase = _cubeProgrammerApi.MassErase("");
```

- Flash:

```csharp
var downloadFile = _cubeProgrammerApi.DownloadFile(firmwarePath, "0x08000000", 1U, 1U);
```

- Run:

```csharp
var execute = _cubeProgrammerApi.Execute("0x08000000");
```

## License
The project is under Microsoft Reciprocal License [(MS-RL)](http://www.opensource.org/licenses/MS-RL)

## Dependencies

List of technologies, frameworks and libraries used for implementation:

- [Microsoft.Bcl.AsyncInterfaces](https://www.nuget.org/packages/Microsoft.Bcl.AsyncInterfaces)
- [Microsoft.Extensions.Logging.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions)