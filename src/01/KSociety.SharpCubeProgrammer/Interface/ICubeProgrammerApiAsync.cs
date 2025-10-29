// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Threading;
    using DeviceDataStructure;
    using Enum;
    using Struct;

    public interface ICubeProgrammerApiAsync : IAsyncDisposable
    {

        #region [STLINK]

        //STLINK module groups debug ports JTAG/SWD functions together.

        ValueTask<CubeProgrammerError> TryConnectStLinkAsync(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to get ST-LINK connected probe(s).
        /// </summary>
        /// <param name="shared"></param>
        /// <returns></returns>
        ValueTask<IEnumerable<DebugConnectParameters>> GetStLinkListAsync(bool shared = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to get ST-LINK connected probe(s) without connecting and intruse the target.
        /// </summary>
        /// <param name="shared"></param>
        /// <returns></returns>
        ValueTask<IEnumerable<DebugConnectParameters>> GetStLinkEnumerationListAsync(bool shared = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to start connection to device through SWD or JTAG interfaces.
        /// </summary>
        /// <param name="debugConnectParameters"></param>
        ValueTask<CubeProgrammerError> ConnectStLinkAsync(DebugConnectParameters debugConnectParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine used to apply a target reset, use only with ST-LINK!.
        /// </summary>
        ValueTask<CubeProgrammerError> ResetAsync(DebugResetMode rstMode, CancellationToken cancellationToken = default);

        #endregion

        #region [Bootloader]

        //Bootloader module is a way to group Serial interfaces USB/UART/SPI/I2C/CAN functions together.

        /// <summary>
        /// This routine allows to get connected serial ports.
        /// </summary>
        ValueTask<IEnumerable<UsartConnectParameters>> GetUsartListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to start connection to device through USART interface.
        /// </summary>
        ValueTask<CubeProgrammerError> ConnectUsartBootloaderAsync(UsartConnectParameters usartConnectParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to send a single byte through the USART interface.
        /// </summary>
        ValueTask<CubeProgrammerError> SendByteUartAsync(int bytes, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to get connected DFU devices.
        /// </summary>
        ValueTask<int> GetDfuDeviceListAsync(List<DfuDeviceInfo> dfuDeviceList, int iPID = 0xdf11, int iVID = 0x0483, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to start a simple connection through USB DFU interface.
        /// </summary>
        ValueTask<CubeProgrammerError> ConnectDfuBootloaderAsync(string usbIndex, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to start connection to device through USB DFU interface.
        /// </summary>
        ValueTask<CubeProgrammerError> ConnectDfuBootloader2Async(DfuConnectParameters dfuParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to start connection to device through USB DFU interface.
        /// </summary>
        ValueTask<CubeProgrammerError> ConnectDfuBootloader2Async(string usbIndex, byte rdu, byte tzenreg, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to start connection to device through SPI interface.
        /// </summary>
        ValueTask<CubeProgrammerError> ConnectSpiBootloaderAsync(SpiConnectParameters spiParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to start connection to device through CAN interface.
        /// </summary>
        ValueTask<CubeProgrammerError> ConnectCanBootloaderAsync(CanConnectParameters canParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to start connection to device through I2C interface.
        /// </summary>
        ValueTask<CubeProgrammerError> ConnectI2CBootloaderAsync(I2CConnectParameters i2CParameters, CancellationToken cancellationToken = default);

        #endregion

        #region [General purposes]

        // General module groups general purposes functions used by any interface.

        /// <summary>
        /// This routine allows to choose your custom display.
        /// </summary>
        /// <param name="initProgressBar"></param>
        /// <param name="messageReceived"></param>
        /// <param name="progressBarUpdate"></param>
        /// <returns></returns>
        ValueTask<DisplayCallBacks> SetDisplayCallbacksAsync(InitProgressBar initProgressBar, LogMessageReceived messageReceived, ProgressBarUpdateReceived progressBarUpdate, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to choose your custom display.
        /// </summary>
        /// <param name="callbacksHandle">Fill the struct to customize the display tool.</param>
        ValueTask<DisplayCallBacks> SetDisplayCallbacksAsync(DisplayCallBacks callbacksHandle, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to choose the verbosity level for display.
        /// </summary>
        /// <param name="level">Indicates the verbosity number 0, 1 or 3.</param>
        ValueTask SetVerbosityLevelAsync(VerbosityLevel level, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to check connection status [maintained or lost].
        /// </summary>
        /// <returns></returns>
        ValueTask<bool> CheckDeviceConnectionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to get general device information.
        /// </summary>
        ValueTask<GeneralInf?> GetDeviceGeneralInfAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to receive memory data on the used interface with the configuration already initialized.
        /// </summary>
        ValueTask<(CubeProgrammerError, byte[])> ReadMemoryAsync(string address, int byteSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to write memory data on the user interface with the configuration already initialized.
        /// </summary>
        /// <param name="address">The address to start writing from.</param>
        /// <param name="data">Data buffer.</param>
        ValueTask<CubeProgrammerError> WriteMemoryAsync(string address, byte[] data, int size = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to write memory data on the user interface with the configuration already initialized.
        /// Aligns the buffer to a multiple of 8 bytes appending 0xFF if necessary.
        /// </summary>
        /// <param name="address">The address to start writing from.</param>
        /// <param name="data">Data buffer.</param>
        ValueTask<CubeProgrammerError> WriteMemoryAutoFillAsync(string address, byte[] data, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to write memory data and verify on the user interface with the configuration already initialized.
        /// Inside it uses the WriteMemoryAutoFill function.
        /// </summary>
        /// <param name="address">The address to start writing from.</param>
        /// <param name="data">Data buffer.</param>
        ValueTask<CubeProgrammerError> WriteMemoryAndVerifyAsync(string address, byte[] data, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to write sector data on the user interface with the configuration already initialized.
        /// </summary>
        /// <param name="address">The address to start writing from.</param>
        /// <param name="data">Data buffer.</param>
        /// <returns>CubeprogrammerNoError if the writing operation correctly finished, otherwise an error occurred.</returns>
        /// <remarks>Unlike ST-LINK interface, the Bootloader interface can access only to some specific memory regions.</remarks>
        /// <remarks>Data size should not exceed sector size.</remarks>
        ValueTask<CubeProgrammerError> EditSectorAsync(string address, byte[] data, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to download data from a file to the memory.
        /// File formats that are supported : hex, bin, srec, tsv, elf, axf, out, stm32, ext
        /// </summary>
        ValueTask<CubeProgrammerError> DownloadFileAsync(string inputFilePath, string address = "0x08000000", uint skipErase = 0U, uint verify = 1U, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to run the application.
        /// </summary>
        ValueTask<CubeProgrammerError> ExecuteAsync(string address = "0x08000000", CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to erase the whole Flash memory.
        /// </summary>
        ValueTask<CubeProgrammerError> MassEraseAsync(string sFlashMemName = "", CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to erase specific sectors of the Flash memory.
        /// </summary>
        ValueTask<CubeProgrammerError> SectorEraseAsync(uint[] sectors, uint sectorNbr, string sFlashMemName = "", CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to disable the readout protection.
        /// If the memory is not protected, a message appears to indicate that the device is not
        /// under Readout protection and the command has no effects.
        /// </summary>
        ValueTask<CubeProgrammerError> ReadUnprotectAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows the TZEN Option Byte regression.
        /// </summary>
        /// <returns>CubeprogrammerNoError if the disabling correctly accomplished, otherwise an error occurred.</returns>
        /// <remarks>Depending on the device used, this routine take a specific time.</remarks>
        ValueTask<CubeProgrammerError> TzenRegressionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to know the interface what is in use.
        /// </summary>
        ValueTask<TargetInterfaceType?> GetTargetInterfaceTypeAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to drop the current read/write operation.
        /// </summary>
        ValueTask<int> GetCancelPointerAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to open and get data from any supported file extension.
        /// </summary>
        ValueTask<DeviceFileDataC?> FileOpenAsync(string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to open and get pointer from any supported file extension.
        /// </summary>
        ValueTask<IntPtr> FileOpenAsPointerAsync(string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to clean up the handled file data.
        /// </summary>
        ValueTask FreeFileDataAsync(IntPtr data, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to free a specific memory region, typically used after readMemory().
        /// </summary>
        /// <param name="ptr">The input pointer address.</param>
        ValueTask FreeLibraryMemoryAsync(IntPtr ptr, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to verify if the indicated file data is identical to Flash memory content.
        /// </summary>
        ValueTask<CubeProgrammerError> VerifyAsync(IntPtr fileData, string address, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to verify if the indicated data[] is identical to Flash memory content.
        /// </summary>
        ValueTask<CubeProgrammerError> VerifyMemoryAsync(string address, byte[] data, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to verify if the indicated data[] is identical to Flash memory content.
        /// </summary>
        ValueTask<CubeProgrammerError> VerifyMemoryBySegmentAsync(string address, byte[] data, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to save the data file content to another file.
        /// </summary>
        ValueTask<CubeProgrammerError> SaveFileToFileAsync(IntPtr fileData, string sFileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to save Flash memory content to file.
        /// </summary>
        ValueTask<CubeProgrammerError> SaveMemoryToFileAsync(string address, string size, string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to clean up and disconnect the current connected target.
        /// </summary>
        ValueTask<CubeProgrammerError> DisconnectAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to clear the list of each created interface.
        /// </summary>
        ValueTask DeleteInterfaceListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to enter and make an automatic process for memory management through JTAG/SWD, UART, DFU, SPI, CAN and I²C interfaces.
        /// </summary>
        ValueTask AutomaticModeAsync(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to enter and make an automatic process for memory management with serial numbering through JTAG/SWD, UART, DFU, SPI, CAN and I²C interfaces.
        /// Connection to target must be established before performing automatic mode with serial numbering.
        /// </summary>
        /// <param name="filePath">Indicates the full file path.</param>
        /// <param name="address">The address to start downloading from.</param>
        /// <param name="skipErase">If we have a blank device, we can skip erasing memory before programming [skipErase=0].</param>
        /// <param name="verify">Add verification step after downloading.</param>
        /// <param name="isMassErase">Erase the whole Flash memory.</param>
        /// <param name="obCommand">Indicates the option bytes commands to be loaded "-ob [optionbyte=value] [optionbyte=value]..."</param>
        /// <param name="run">Start the application.</param>
        /// <param name="enableSerialNumbering">Enables the serial numbering.</param>
        /// <param name="serialAddress">The address where the inital data and the subsequent increments will be made.</param>
        /// <param name="serialSize">Size for the serial numbering.</param>
        /// <param name="serialInitialData">Intial data used for the serial numbering that will be incremented.</param>
        ValueTask SerialNumberingAutomaticModeAsync(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1, int enableSerialNumbering = 0, int serialAddress = 0, int serialSize = 0, string serialInitialData = "", CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to get Flash storage information.
        /// </summary>
        ValueTask<(CubeProgrammerError, DeviceStorageStructure)> GetStorageStructureAsync(CancellationToken cancellationToken = default);

        #endregion

        #region [Option Bytes]

        //OB module groups option bytes functions used by any interface.

        /// <summary>
        /// This routine allows program the given Option Byte.
        /// The option bytes are configured by the end user depending on the application requirements.
        /// </summary>
        ValueTask<CubeProgrammerError> SendOptionBytesCmdAsync(string command, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to get option bytes values of the connected target.
        /// </summary>
        ValueTask<DevicePeripheralC?> InitOptionBytesInterfaceAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to get option bytes values of the connected target.
        /// </summary>
        ValueTask<DevicePeripheralC?> FastRomInitOptionBytesInterfaceAsync(ushort deviceId, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to display the Option bytes.
        /// </summary>
        ValueTask<CubeProgrammerError> ObDisplayAsync(CancellationToken cancellationToken = default);

        #endregion

        #region [Loaders]

        //Loaders module groups loaders functions.

        /// <summary>
        /// This routine allows to specify the location of Flash Loader.
        /// </summary>
        /// <param name="path">Indicates the full path of the considered folder.</param>
        ValueTask SetLoadersPathAsync(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to specify the path of the external Loaders to be loaded.
        /// </summary>
        /// <param name="path"></param>
        ValueTask<DeviceExternalLoader?> SetExternalLoaderPathAsync(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to specify the path of the external Loaders to be loaded via OBL interfaces.
        /// </summary>
        /// <param name="path">Indicates the full path of the folder containing external Loaders.</param>
        ValueTask<DeviceExternalLoader?> SetExternalLoaderOBLAsync(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to get available external Loaders in th mentioned path.
        /// </summary>
        ValueTask<DeviceExternalStorageInfo?> GetExternalLoadersAsync(string path = @".\st\Programmer", CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to unload an external Loaders.
        /// </summary>
        ValueTask RemoveExternalLoaderAsync(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to delete all target Flash Loaders.
        /// </summary>
        ValueTask DeleteLoadersAsync(CancellationToken cancellationToken = default);

        #endregion

        #region [STM32WB specific]

        /// Specific APIs used exclusively for STM32WB series to manage BLE Stack, and they are available only through USB DFU and UART bootloader interfaces,
        /// except for the "firmwareDelete" and the "firmwareUpgrade", available through USB DFU, UART and SWD interfaces.
        /// Connection under Reset is mandatory.

        /// <summary>
        /// This routine allows to read the device unique identifier.
        /// </summary>
        ValueTask<(CubeProgrammerError, byte[])> GetUID64Async(CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to erase the BLE stack firmware.
        /// </summary>
        ValueTask<bool> FirmwareDeleteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to make upgrade of BLE stack firmware or FUS firmware.
        /// </summary>
        ValueTask<bool> FirmwareUpgradeAsync(string filePath, string address, WbFunctionArguments firstInstall, WbFunctionArguments startStack, WbFunctionArguments verify, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to start the programmed Stack.
        /// </summary>
        ValueTask<bool> StartWirelessStackAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to start the programmed Stack.
        /// </summary>
        ValueTask<bool> UpdateAuthKeyAsync(string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to lock the authentication key and once locked, it is no longer possible to change it.
        /// </summary>
        ValueTask<CubeProgrammerError> AuthKeyLockAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to write a customized user key.
        /// </summary>
        ValueTask<CubeProgrammerError> WriteUserKeyAsync(string filePath, byte keyType, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to activate the AntiRollBack.
        /// </summary>
        ValueTask<bool> AntiRollBackAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to start and establish a communication with the FUS operator.
        /// </summary>
        ValueTask<bool> StartFusAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine allows to set default option Bytes.
        /// </summary>
        /// <returns></returns>
        ValueTask<CubeProgrammerError> UnlockChipAsync(CancellationToken cancellationToken = default);

        #endregion

        #region [STM32MP specific functions]

        //Specific APIs used exclusively for STM32MP devices. The connection is available only through USB DFU and UART interfaces

        /// <summary>
        /// This routine aims to launch the Secure Secret Provisioning.
        /// If you are trying to start the SSP with HSM, the licenseFile parameter should be empty.
        /// </summary>
        /// <param name="sspFile">Indicates the full path of the ssp file [Use STM32TrustedPackageCreator to generate a ssp image].</param>
        /// <param name="licenseFile">Indicates the full path of the license file. If you are trying to start the SSP without HSM, the hsmSlotId should be 0.</param>
        /// <param name="tfaFile">Indicates the full path of the tfa-ssp file.</param>
        /// <param name="hsmSlotId">Indicates the HSM slot ID.</param>
        /// <returns>0 if the SSP was finished successfully, otherwise an error occurred.</returns>
        ValueTask<CubeProgrammerError> ProgramSspAsync(string sspFile, string licenseFile, string tfaFile, int hsmSlotId, CancellationToken cancellationToken = default);

        #endregion

        #region [STM32 HSM specific functions]

        //Specific APIs used exclusively for STM32 devices to manage the Hardware Secure Module.

        /// <summary>
        /// This routine aims to get the HSM Firmware Identifier.
        /// </summary>
        /// <param name="hsmSlotId">The slot index of the plugged-in HSM</param>
        /// <returns>string that contains the HSM Firmware Identifier.</returns>
        ValueTask<string> GetHsmFirmwareIDAsync(int hsmSlotId, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine aims to get the current HSM counter.
        /// </summary>
        /// <param name="hsmSlotId">The slot index of the plugged-in HSM</param>
        /// <returns>Counter value</returns>
        ValueTask<ulong> GetHsmCounterAsync(int hsmSlotId, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine aims to get the HSM State.
        /// </summary>
        /// <param name="hsmSlotId">The slot index of the plugged-in HSM</param>
        /// <returns>string with possible values: ST_STATE , OEM_STATE, OPERATIONAL_STATE , UNKNOWN_STATE</returns>
        ValueTask<string> GetHsmStateAsync(int hsmSlotId, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine aims to get the HSM version.
        /// </summary>
        /// <param name="hsmSlotId">The slot index of the plugged-in HSM</param>
        /// <returns>string with possible values: 1 , 2</returns>
        ValueTask<string> GetHsmVersionAsync(int hsmSlotId, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine aims to get the HSM type.
        /// </summary>
        /// <param name="hsmSlotId">The slot index of the plugged-in HSM</param>
        /// <returns>string with possible values: SFI. SMU. SSP...</returns>
        ValueTask<string> GetHsmTypeAsync(int hsmSlotId, CancellationToken cancellationToken = default);

        /// <summary>
        /// This routine aims to get and save the HSM license into a binary file.
        /// Connection to target must be established before performing this routine.
        /// </summary>
        /// <param name="hsmSlotId">The slot index of the plugged-in HSM</param>
        /// <param name="outLicensePath">Path of the output binary file.</param>
        /// <returns>0 if the operation was finished successfully, otherwise an error occurred.</returns>
        ValueTask<CubeProgrammerError> GetHsmLicenseAsync(int hsmSlotId, string outLicensePath, CancellationToken cancellationToken = default);

        #endregion

        #region [EXTENDED]

        //string VersionAPIAsync();

        void HaltAsync(CancellationToken cancellationToken = default);

        void RunAsync(CancellationToken cancellationToken = default);

        void StepAsync(CancellationToken cancellationToken = default);

        #endregion

        #region [Util]

        /// <summary>
        /// HexConverterToUint
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        ValueTask<uint> HexConverterToUintAsync(string hex, CancellationToken cancellationToken = default);

        /// <summary>
        /// HexConverterToInt
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        ValueTask<int> HexConverterToIntAsync(string hex, CancellationToken cancellationToken = default);

        /// <summary>
        /// HexConverterToString
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        ValueTask<string> HexConverterToStringAsync(uint hex, CancellationToken cancellationToken = default);

        /// <summary>
        /// HexConverterToString
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        ValueTask<string> HexConverterToStringAsync(int hex, CancellationToken cancellationToken = default);

        #endregion

    }
}
