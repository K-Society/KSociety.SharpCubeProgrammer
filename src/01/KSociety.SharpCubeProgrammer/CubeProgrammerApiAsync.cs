// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using SharpCubeProgrammer.DeviceDataStructure;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Interface;
    using SharpCubeProgrammer.Struct;

    public partial class CubeProgrammerApi : ICubeProgrammerApiAsync
    {
        #region [ST-LINK]

        //ST-LINK module groups debug ports JTAG/SWD functions together.

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> TryConnectStLinkAsync(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.TryConnectStLink(stLinkProbeIndex, shared, debugConnectMode), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<IEnumerable<DebugConnectParameters>> GetStLinkListAsync(bool shared = false, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.GetStLinkList(shared), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<IEnumerable<DebugConnectParameters>> GetStLinkEnumerationListAsync(bool shared = false, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.GetStLinkEnumerationList(shared), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> ConnectStLinkAsync(DebugConnectParameters debugConnectParameters, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.ConnectStLink(debugConnectParameters), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> ResetAsync(DebugResetMode rstMode, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.Reset(rstMode), cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region [Bootloader]

        //Bootloader module is a way to group Serial interfaces USB/UART/SPI/I2C/CAN function together.

        /// <inheritdoc />
        public async ValueTask<IEnumerable<UsartConnectParameters>> GetUsartListAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.GetUsartList(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> ConnectUsartBootloaderAsync(UsartConnectParameters usartConnectParameters, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.ConnectUsartBootloader(usartConnectParameters), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> SendByteUartAsync(int bytes, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.SendByteUart(bytes), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<IEnumerable<DfuDeviceInfo>> GetDfuDeviceListAsync(int iPID = 0xdf11, int iVID = 0x0483, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.GetDfuDeviceList(iPID, iVID), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> ConnectDfuBootloaderAsync(string usbIndex, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.ConnectDfuBootloader(usbIndex), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> ConnectDfuBootloader2Async(DfuConnectParameters dfuParameters, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.ConnectDfuBootloader2(dfuParameters), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> ConnectDfuBootloader2Async(string usbIndex, byte rdu, byte tzenreg, int usbTimeout = 30000, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.ConnectDfuBootloader2(usbIndex, rdu, tzenreg, usbTimeout), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> ConnectSpiBootloaderAsync(SpiConnectParameters spiParameters, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.ConnectSpiBootloader(spiParameters), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> ConnectCanBootloaderAsync(CanConnectParameters canParameters, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.ConnectCanBootloader(canParameters), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> ConnectI2CBootloaderAsync(I2cConnectParameters i2CParameters, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.ConnectI2CBootloader(i2CParameters), cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region [General purposes]

        // General module groups general purposes functions used by any interface.

        /// <inheritdoc />
        public async ValueTask<DisplayCallBacks> SetDisplayCallbacksAsync(InitProgressBar initProgressBar, LogMessageReceived messageReceived, ProgressBarUpdateReceived progressBarUpdate, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.SetDisplayCallbacks(initProgressBar, messageReceived, progressBarUpdate), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<DisplayCallBacks> SetDisplayCallbacksAsync(DisplayCallBacks callbacksHandle, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.SetDisplayCallbacks(callbacksHandle), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask SetVerbosityLevelAsync(VerbosityLevel level, CancellationToken cancellationToken = default)
        {
            await Task.Run(() => this.SetVerbosityLevel(level), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<bool> CheckDeviceConnectionAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.CheckDeviceConnection(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<GeneralInf?> GetDeviceGeneralInfAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.GetDeviceGeneralInf(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<(CubeProgrammerError, byte[])> ReadMemoryAsync(string address, int byteSize, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.ReadMemory(address, byteSize), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> WriteMemoryAsync(string address, byte[] data, int size = 0, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.WriteMemory(address, data, size), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        [Obsolete("WriteMemoryAutoFillAsync is deprecated, please use WriteMemoryAsync instead.")]
        public async ValueTask<CubeProgrammerError> WriteMemoryAutoFillAsync(string address, byte[] data, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.WriteMemoryAutoFill(address, data), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        [Obsolete("WriteMemoryAndVerifyAsync is deprecated, please use WriteMemoryAsync instead.")]
        public async ValueTask<CubeProgrammerError> WriteMemoryAndVerifyAsync(string address, byte[] data, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.WriteMemoryAndVerify(address, data), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> EditSectorAsync(string address, byte[] data, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.EditSector(address, data), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> DownloadFileAsync(string inputFilePath, string address = "0x08000000", uint skipErase = 0U, uint verify = 1U, string binFilePath = "", CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.DownloadFile(inputFilePath, address, skipErase, verify, binFilePath), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> ExecuteAsync(string address = "0x08000000", CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.Execute(address), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> MassEraseAsync(string sFlashMemName = "", CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.MassErase(sFlashMemName), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> SectorEraseAsync(uint[] sectors, uint sectorNbr, string sFlashMemName = "", CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.SectorErase(sectors, sectorNbr, sFlashMemName), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> ReadUnprotectAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.ReadUnprotect(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> TzenRegressionAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.TzenRegression(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<TargetInterfaceType?> GetTargetInterfaceTypeAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.GetTargetInterfaceType(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<int> GetCancelPointerAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.GetCancelPointer(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<DeviceFileDataC?> FileOpenAsync(string filePath, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.FileOpen(filePath), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<IntPtr> FileOpenAsPointerAsync(string filePath, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.FileOpenAsPointer(filePath), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask FreeFileDataAsync(IntPtr data, CancellationToken cancellationToken = default)
        {
            await Task.Run(() => this.FreeFileData(data), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask FreeLibraryMemoryAsync(IntPtr ptr, CancellationToken cancellationToken = default)
        {
            await Task.Run(() => this.FreeLibraryMemory(ptr), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> VerifyAsync(IntPtr fileData, string address, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.Verify(fileData, address), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> VerifyMemoryAsync(string address, byte[] data, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.VerifyMemory(address, data), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> VerifyMemoryBySegmentAsync(string address, byte[] data, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.VerifyMemoryBySegment(address, data), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> SaveFileToFileAsync(IntPtr fileData, string sFileName, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.SaveFileToFile(fileData, sFileName), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> SaveMemoryToFileAsync(string address, string size, string fileName, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.SaveMemoryToFile(address, size, fileName), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> DisconnectAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.Disconnect(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask DeleteInterfaceListAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() => this.DeleteInterfaceList(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask AutomaticModeAsync(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1, CancellationToken cancellationToken = default)
        {
            await Task.Run(() => this.AutomaticMode(filePath, address, skipErase, verify, isMassErase, obCommand, run), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask SerialNumberingAutomaticModeAsync(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1, int enableSerialNumbering = 0, int serialAddress = 0, int serialSize = 0, string serialInitialData = "", CancellationToken cancellationToken = default)
        {
            await Task.Run(() => this.SerialNumberingAutomaticMode(filePath, address, skipErase, verify, isMassErase, obCommand, run, enableSerialNumbering, serialAddress, serialSize, serialInitialData), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<(CubeProgrammerError, DeviceStorageStructure)> GetStorageStructureAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.GetStorageStructure(), cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region [Option Bytes]

        //OB module groups option bytes functions used by any interface.

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> SendOptionBytesCmdAsync(string command, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.SendOptionBytesCmd(command), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<DevicePeripheralC?> InitOptionBytesInterfaceAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.InitOptionBytesInterface(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<DevicePeripheralC?> FastRomInitOptionBytesInterfaceAsync(ushort deviceId, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.FastRomInitOptionBytesInterface(deviceId), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> ObDisplayAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.ObDisplay(), cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region [Loaders]

        //Loaders module groups loaders functions.

        /// <inheritdoc />
        public async ValueTask SetLoadersPathAsync(string path, CancellationToken cancellationToken = default)
        {
            await Task.Run(() => this.SetLoadersPath(path), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<DeviceExternalLoader?> SetExternalLoaderPathAsync(string path, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.SetExternalLoaderPath(path), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<DeviceExternalLoader?> SetExternalLoaderOBLAsync(string path, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.SetExternalLoaderOBL(path), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<DeviceExternalStorageInfo?> GetExternalLoadersAsync(string path = @".\st\Programmer", CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.GetExternalLoaders(path), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask RemoveExternalLoaderAsync(string path, CancellationToken cancellationToken = default)
        {
            await Task.Run(() => this.RemoveExternalLoader(path), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask DeleteLoadersAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() => this.DeleteLoaders(), cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region [STM32WB specific]

        /// Specific APIs used exclusively for STM32WB series to manage BLE Stack, and they are available only through USB DFU and UART bootloader interfaces,
        /// except for the "firmwareDelete" and the "firmwareUpgrade", available through USB DFU, UART and SWD interfaces.
        /// Connection under Reset is mandatory.

        /// <inheritdoc />
        public async ValueTask<(CubeProgrammerError, byte[])> GetUID64Async(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.GetUID64(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<bool> FirmwareDeleteAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.FirmwareDelete(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<bool> FirmwareUpgradeAsync(string filePath, string address, WbFunctionArguments firstInstall, WbFunctionArguments startStack, WbFunctionArguments verify, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.FirmwareUpgrade(filePath, address, firstInstall, startStack, verify), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<bool> StartWirelessStackAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.StartWirelessStack(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<bool> UpdateAuthKeyAsync(string filePath, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.UpdateAuthKey(filePath), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> AuthKeyLockAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.AuthKeyLock(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> WriteUserKeyAsync(string filePath, byte keyType, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.WriteUserKey(filePath, keyType), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<bool> AntiRollBackAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.AntiRollBack(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<bool> StartFusAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.StartFus(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> UnlockChipAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.UnlockChip(), cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region [STM32MP specific functions]

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> ProgramSspAsync(string sspFile, string licenseFile, string tfaFile, int hsmSlotId, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.ProgramSsp(sspFile, licenseFile, tfaFile, hsmSlotId), cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region [STM32 HSM specific functions]

        /// <inheritdoc />
        public async ValueTask<string> GetHsmFirmwareIDAsync(int hsmSlotId, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.GetHsmFirmwareID(hsmSlotId), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<ulong> GetHsmCounterAsync(int hsmSlotId, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.GetHsmCounter(hsmSlotId), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<string> GetHsmStateAsync(int hsmSlotId, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.GetHsmState(hsmSlotId), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<string> GetHsmVersionAsync(int hsmSlotId, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.GetHsmVersion(hsmSlotId), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<string> GetHsmTypeAsync(int hsmSlotId, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.GetHsmType(hsmSlotId), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<CubeProgrammerError> GetHsmLicenseAsync(int hsmSlotId, string outLicensePath, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.GetHsmLicense(hsmSlotId, outLicensePath), cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region [EXTENDED]

        //public string VersionAPIAsync()
        //{
        //    if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
        //    {
        //        return Native.ProgrammerApi.VersionAPI();
        //    }

        //    return String.Empty;
        //}

        public async void HaltAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() => this.Halt(), cancellationToken).ConfigureAwait(false);
        }

        public async void RunAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() => this.Run(), cancellationToken).ConfigureAwait(false);
        }

        public async void StepAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() => this.Step(), cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region [Util]

        /// <inheritdoc />
        public async ValueTask<uint> HexConverterToUintAsync(string hex, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.HexConverterToUint(hex), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<int> HexConverterToIntAsync(string hex, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.HexConverterToInt(hex), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<string> HexConverterToStringAsync(uint hex, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.HexConverterToString(hex), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async ValueTask<string> HexConverterToStringAsync(int hex, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => this.HexConverterToString(hex), cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region [Dispose]

        /// <inheritdoc/>
        [SuppressMessage(
            "Usage",
            "CA1816:Dispose methods should call SuppressFinalize",
            Justification = "DisposeAsync should also call SuppressFinalize (see various .NET internal implementations).")]
        public ValueTask DisposeAsync()
        {
            // Still need to check if we've already disposed; can't do both.
            var wasDisposed = Interlocked.Exchange(ref this._isDisposed, DisposedFlag);
            if (wasDisposed != DisposedFlag)
            {
                GC.SuppressFinalize(this);

                // Always true, but means we get the similar syntax as Dispose,
                // and separates the two overloads.
                return this.DisposeAsync(true);
            }

            return default;
        }

        /// <summary>
        ///  Releases unmanaged and - optionally - managed resources, asynchronously.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected ValueTask DisposeAsync(bool disposing)
        {
            // Default implementation does a synchronous dispose.
            this.Dispose(disposing);

            return default;
        }

        #endregion
    }
}
