// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Threading;
    using DeviceDataStructure;
    using Enum;
    using Struct;

    /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/CubeProgrammerApiAsync/*'/>
    public interface ICubeProgrammerApiAsync : IAsyncDisposable
    {

        #region [STLINK]

        //STLINK module groups debug ports JTAG/SWD functions together.

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/TryConnectStLinkAsync/*'/>
        ValueTask<CubeProgrammerError> TryConnectStLinkAsync(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/GetStLinkListAsync/*'/>
        ValueTask<IEnumerable<DebugConnectParameters>> GetStLinkListAsync(bool shared = false, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/GetStLinkEnumerationListAsync/*'/>
        ValueTask<IEnumerable<DebugConnectParameters>> GetStLinkEnumerationListAsync(bool shared = false, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/ConnectStLinkAsync/*'/>
        ValueTask<CubeProgrammerError> ConnectStLinkAsync(DebugConnectParameters debugConnectParameters, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/ResetAsync/*'/>
        ValueTask<CubeProgrammerError> ResetAsync(DebugResetMode rstMode, CancellationToken cancellationToken = default);

        #endregion

        #region [Bootloader]

        //Bootloader module is a way to group Serial interfaces USB/UART/SPI/I2C/CAN functions together.

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/GetUsartListAsync/*'/>
        ValueTask<IEnumerable<UsartConnectParameters>> GetUsartListAsync(CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/ConnectUsartBootloaderAsync/*'/>
        ValueTask<CubeProgrammerError> ConnectUsartBootloaderAsync(UsartConnectParameters usartConnectParameters, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/SendByteUartAsync/*'/>
        ValueTask<CubeProgrammerError> SendByteUartAsync(int @byte, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/GetDfuDeviceListAsync/*'/>
        ValueTask<IEnumerable<DfuDeviceInfo>> GetDfuDeviceListAsync(int iPID = 0xdf11, int iVID = 0x0483, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/ConnectDfuBootloaderAsync/*'/>
        ValueTask<CubeProgrammerError> ConnectDfuBootloaderAsync(string usbIndex, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/ConnectDfuBootloader2Async/*'/>
        ValueTask<CubeProgrammerError> ConnectDfuBootloader2Async(DfuConnectParameters dfuParameters, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/ConnectDfuBootloader2OverloadAsync/*'/>
        ValueTask<CubeProgrammerError> ConnectDfuBootloader2Async(string usbIndex, byte rdu, byte tzenreg, int usbTimeout = 30000, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/ConnectSpiBootloaderAsync/*'/>
        ValueTask<CubeProgrammerError> ConnectSpiBootloaderAsync(SpiConnectParameters spiParameters, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/ConnectCanBootloaderAsync/*'/>
        ValueTask<CubeProgrammerError> ConnectCanBootloaderAsync(CanConnectParameters canParameters, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/ConnectI2CBootloaderAsync/*'/>
        ValueTask<CubeProgrammerError> ConnectI2CBootloaderAsync(I2cConnectParameters i2CParameters, CancellationToken cancellationToken = default);

        #endregion

        #region [General purposes]

        // General module groups general purposes functions used by any interface.

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/SetDisplayCallbacksAsync/*'/>
        ValueTask<DisplayCallBacks> SetDisplayCallbacksAsync(InitProgressBar initProgressBar, LogMessageReceived messageReceived, ProgressBarUpdateReceived progressBarUpdate, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/SetDisplayCallbacksAsync/*'/>
        ValueTask<DisplayCallBacks> SetDisplayCallbacksAsync(DisplayCallBacks c, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/SetVerbosityLevelAsync/*'/>
        ValueTask SetVerbosityLevelAsync(VerbosityLevel level, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/CheckDeviceConnectionAsync/*'/>
        ValueTask<bool> CheckDeviceConnectionAsync(CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/GetDeviceGeneralInfAsync/*'/>
        ValueTask<GeneralInf?> GetDeviceGeneralInfAsync(CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/ReadMemoryAsync/*'/>
        ValueTask<(CubeProgrammerError, byte[])> ReadMemoryAsync(string address, int byteSize, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/WriteMemoryAsync/*'/>
        ValueTask<CubeProgrammerError> WriteMemoryAsync(string address, byte[] data, int size = 0, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/WriteMemoryAutoFillAsync/*'/>
        ValueTask<CubeProgrammerError> WriteMemoryAutoFillAsync(string address, byte[] data, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/WriteMemoryAndVerifyAsync/*'/>
        ValueTask<CubeProgrammerError> WriteMemoryAndVerifyAsync(string address, byte[] data, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/EditSectorAsync/*'/>
        ValueTask<CubeProgrammerError> EditSectorAsync(string address, byte[] data, int size = 0, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/DownloadFileAsync/*'/>
        ValueTask<CubeProgrammerError> DownloadFileAsync(string inputFilePath, string address = "0x08000000", uint skipErase = 0U, uint verify = 1U, string binFilePath = "", CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/ExecuteAsync/*'/>
        ValueTask<CubeProgrammerError> ExecuteAsync(string address = "0x08000000", CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/MassEraseAsync/*'/>
        ValueTask<CubeProgrammerError> MassEraseAsync(string sFlashMemName = "", CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/SectorEraseAsync/*'/>
        ValueTask<CubeProgrammerError> SectorEraseAsync(uint[] sectors, uint sectorNbr, string sFlashMemName = "", CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/ReadUnprotectAsync/*'/>
        ValueTask<CubeProgrammerError> ReadUnprotectAsync(CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/TzenRegressionAsync/*'/>
        ValueTask<CubeProgrammerError> TzenRegressionAsync(CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/GetTargetInterfaceTypeAsync/*'/>
        ValueTask<TargetInterfaceType?> GetTargetInterfaceTypeAsync(CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/GetCancelPointerAsync/*'/>
        ValueTask<int> GetCancelPointerAsync(CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/FileOpenAsync/*'/>
        ValueTask<DeviceFileDataC?> FileOpenAsync(string filePath, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/FileOpenAsPointerAsync/*'/>
        ValueTask<IntPtr> FileOpenAsPointerAsync(string filePath, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/FreeFileDataAsync/*'/>
        ValueTask FreeFileDataAsync(IntPtr data, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/FreeLibraryMemoryAsync/*'/>
        ValueTask FreeLibraryMemoryAsync(IntPtr ptr, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/VerifyAsync/*'/>
        ValueTask<CubeProgrammerError> VerifyAsync(IntPtr fileData, string address, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/VerifyMemoryAsync/*'/>
        ValueTask<CubeProgrammerError> VerifyMemoryAsync(string address, byte[] data, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/VerifyMemoryBySegmentAsync/*'/>
        ValueTask<CubeProgrammerError> VerifyMemoryBySegmentAsync(string address, byte[] data, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/SaveFileToFileAsync/*'/>
        ValueTask<CubeProgrammerError> SaveFileToFileAsync(IntPtr fileData, string sFileName, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/SaveMemoryToFileAsync/*'/>
        ValueTask<CubeProgrammerError> SaveMemoryToFileAsync(string address, string size, string fileName, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/DisconnectAsync/*'/>
        ValueTask DisconnectAsync(CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/DeleteInterfaceListAsync/*'/>
        ValueTask DeleteInterfaceListAsync(CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/AutomaticModeAsync/*'/>
        ValueTask AutomaticModeAsync(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/SerialNumberingAutomaticModeAsync/*'/>
        ValueTask SerialNumberingAutomaticModeAsync(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1, int enableSerialNumbering = 0, int serialAddress = 0, int serialSize = 0, string serialInitialData = "", CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/GetStorageStructureAsync/*'/>
        ValueTask<(CubeProgrammerError, DeviceStorageStructure)> GetStorageStructureAsync(CancellationToken cancellationToken = default);

        #endregion

        #region [Option Bytes]

        //OB module groups option bytes functions used by any interface.

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/SendOptionBytesCmdAsync/*'/>
        ValueTask<CubeProgrammerError> SendOptionBytesCmdAsync(string command, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/InitOptionBytesInterfaceAsync/*'/>
        ValueTask<DevicePeripheralC?> InitOptionBytesInterfaceAsync(CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/FastRomInitOptionBytesInterfaceAsync/*'/>
        ValueTask<DevicePeripheralC?> FastRomInitOptionBytesInterfaceAsync(ushort deviceId, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/ObDisplayAsync/*'/>
        ValueTask<CubeProgrammerError> ObDisplayAsync(CancellationToken cancellationToken = default);

        #endregion

        #region [Loaders]

        //Loaders module groups loaders functions.

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/SetLoadersPathAsync/*'/>
        ValueTask SetLoadersPathAsync(string path, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/SetExternalLoaderPathAsync/*'/>
        ValueTask<DeviceExternalLoader?> SetExternalLoaderPathAsync(string path, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/SetExternalLoaderOBLAsync/*'/>
        ValueTask<DeviceExternalLoader?> SetExternalLoaderOBLAsync(string path, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/GetExternalLoadersAsync/*'/>
        ValueTask<DeviceExternalStorageInfo?> GetExternalLoadersAsync(string path = @".\st\Programmer", CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/RemoveExternalLoaderAsync/*'/>
        ValueTask RemoveExternalLoaderAsync(string path, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/DeleteLoadersAsync/*'/>
        ValueTask DeleteLoadersAsync(CancellationToken cancellationToken = default);

        #endregion

        #region [STM32WB specific]

        /// Specific APIs used exclusively for STM32WB series to manage BLE Stack, and they are available only through USB DFU and UART bootloader interfaces,
        /// except for the "firmwareDelete" and the "firmwareUpgrade", available through USB DFU, UART and SWD interfaces.
        /// Connection under Reset is mandatory.

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/GetUID64Async/*'/>
        ValueTask<(CubeProgrammerError, byte[])> GetUID64Async(CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/FirmwareDeleteAsync/*'/>
        ValueTask<bool> FirmwareDeleteAsync(CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/FirmwareUpgradeAsync/*'/>
        ValueTask<bool> FirmwareUpgradeAsync(string filePath, string address, WbFunctionArguments firstInstall, WbFunctionArguments startStack, WbFunctionArguments verify, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/StartWirelessStackAsync/*'/>
        ValueTask<bool> StartWirelessStackAsync(CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/UpdateAuthKeyAsync/*'/>
        ValueTask<bool> UpdateAuthKeyAsync(string filePath, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/AuthKeyLockAsync/*'/>
        ValueTask<CubeProgrammerError> AuthKeyLockAsync(CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/WriteUserKeyAsync/*'/>
        ValueTask<CubeProgrammerError> WriteUserKeyAsync(string filePath, byte keyType, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/AntiRollBackAsync/*'/>
        ValueTask<bool> AntiRollBackAsync(CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/StartFusAsync/*'/>
        ValueTask<bool> StartFusAsync(CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/UnlockChipAsync/*'/>
        ValueTask<CubeProgrammerError> UnlockChipAsync(CancellationToken cancellationToken = default);

        #endregion

        #region [STM32MP specific functions]

        //Specific APIs used exclusively for STM32MP devices. The connection is available only through USB DFU and UART interfaces

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/ProgramSspAsync/*'/>
        ValueTask<CubeProgrammerError> ProgramSspAsync(string sspFile, string licenseFile, string tfaFile, int hsmSlotId, CancellationToken cancellationToken = default);

        #endregion

        #region [STM32 HSM specific functions]

        //Specific APIs used exclusively for STM32 devices to manage the Hardware Secure Module.

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/GetHsmFirmwareIDAsync/*'/>
        ValueTask<string> GetHsmFirmwareIDAsync(int hsmSlotId, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/GetHsmCounterAsync/*'/>
        ValueTask<ulong> GetHsmCounterAsync(int hsmSlotId, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/GetHsmStateAsync/*'/>
        ValueTask<string> GetHsmStateAsync(int hsmSlotId, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/GetHsmVersionAsync/*'/>
        ValueTask<string> GetHsmVersionAsync(int hsmSlotId, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/GetHsmTypeAsync/*'/>
        ValueTask<string> GetHsmTypeAsync(int hsmSlotId, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/GetHsmLicenseAsync/*'/>
        ValueTask<CubeProgrammerError> GetHsmLicenseAsync(int hsmSlotId, string outLicensePath, CancellationToken cancellationToken = default);

        #endregion

        #region [EXTENDED]

        void HaltAsync(CancellationToken cancellationToken = default);

        void RunAsync(CancellationToken cancellationToken = default);

        void StepAsync(CancellationToken cancellationToken = default);

        #endregion

        #region [Util]

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/HexConverterToUintAsync/*'/>
        ValueTask<uint> HexConverterToUintAsync(string hex, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/HexConverterToIntAsync/*'/>
        ValueTask<int> HexConverterToIntAsync(string hex, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/HexConverterToStringAsync/*'/>
        ValueTask<string> HexConverterToStringAsync(uint hex, CancellationToken cancellationToken = default);

        /// <include file='..\Doc\CubeProgrammerApiAsync.xml' path='docs/members[@name="cubeProgrammerApiAsync"]/HexConverterToStringAsync/*'/>
        ValueTask<string> HexConverterToStringAsync(int hex, CancellationToken cancellationToken = default);

        #endregion

    }
}
