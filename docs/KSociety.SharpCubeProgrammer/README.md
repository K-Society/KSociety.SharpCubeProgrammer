[![Logo](https://raw.githubusercontent.com/k-society/KSociety.SharpCubeProgrammer/master/docs/K-Society__Logo_vs-negative.png)](https://github.com/K-Society)

[![build status](https://img.shields.io/github/actions/workflow/status/K-Society/KSociety.SharpCubeProgrammer/build.yml?branch=develop)](https://github.com/K-Society/KSociety.SharpCubeProgrammer/actions/workflows/build.yml?query=branch%3Adevelop) [![NuGet](https://img.shields.io/nuget/v/KSociety.SharpCubeProgrammer)](https://www.nuget.org/packages/KSociety.SharpCubeProgrammer)
[![download count](https://img.shields.io/nuget/dt/KSociety.SharpCubeProgrammer)](https://www.nuget.org/stats/packages/KSociety.SharpCubeProgrammer?groupby=Version)
[KSociety.SharpCubeProgrammer Home](https://github.com/K-Society/KSociety.SharpCubeProgrammer)

# KSociety.SharpCubeProgrammer

KSociety.SharpCubeProgrammer is a wrapper for CubeProgrammer_API v2.15.0.

It makes use of several 3rd party tools:

- STM32 Cube Programmer
   You can find the source, licensing information and documentation [here](https://www.st.com/en/development-tools/stm32cubeprog.html).

## Introduction

This is a C# wrapper for STM32 CubeProgrammer_API v2.14.0 (not fully tested).
This package does not contain any C/C++ runtimes (MSVC), and is meant to run on Windows operating systems only (for now).
Please make sure you have updated the firmware of your ST-LINK V2 / V3, you can do this using STM32CubeProgrammer.
The STM32 CubeProgrammer_API is a C-library, created by ST for ST-Link access to micro-controllers 
for the purpose of flash downloads or general memory access. 
The ST-Link drivers is required, and can be downloaded from st.com and installed [(STSW-LINK009)](https://www.st.com/en/development-tools/stsw-link009.html).
This has been tested on Windows 10, you don't need to install cubeprogrammer.

### KSociety.SharpCubeProgrammer
STM32CubeProgrammer_API C# wrapper.

## Get Packages

You can get KSociety.SharpCubeProgrammer by [grabbing the latest NuGet package](https://www.nuget.org/packages/KSociety.SharpCubeProgrammer/).

## Currently supported features
- GetStLinkList
- ConnectStLink
- Reset
- GetDfuDeviceList
- ConnectDfuBootloader
- CheckDeviceConnection
- GetDeviceGeneralInf
- ReadMemory
- WriteMemory
- DownloadFile
- Execute
- MassErase
- SectorErase
- FileOpen
- Verify
- VerifyMemory
- SaveMemoryToFile
- Disconnect
- DeleteInterfaceList
- GetStorageStructure
- SendOptionBytesCmd
- InitOptionBytesInterface
- ObDisplay

## Get Started

- Register SharpCubeProgrammer as service with Autofac IoC:

```csharp
builder.RegisterModule(new KSociety.SharpCubeProgrammer.Bindings.ProgrammerApi());
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

- [KSociety.Wmi](https://github.com/K-Society/KSociety.Wmi) (WMI)