// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Interface
{
    using System;
    using System.Collections.Generic;
    using DeviceDataStructure;
    using Enum;
    using Struct;

    public interface ICubeProgrammerApi : IDisposable
    {

        #region [STLINK]

        //STLINK module groups debug ports JTAG/SWD functions together.

        CubeProgrammerError TryConnectStLink(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode);

        /// <summary>
        /// This routine allows to get ST-LINK connected probe(s).
        /// </summary>
        /// <param name="shared"></param>
        /// <returns></returns>
        IEnumerable<DebugConnectParameters> GetStLinkList(bool shared = false);

        /// <summary>
        /// This routine allows to get ST-LINK connected probe(s) without connecting and intruse the target.
        /// </summary>
        /// <param name="shared"></param>
        /// <returns></returns>
        IEnumerable<DebugConnectParameters> GetStLinkEnumerationList(bool shared = false);

        /// <summary>
        /// This routine allows to start connection to device through SWD or JTAG interfaces.
        /// </summary>
        /// <param name="debugConnectParameters"></param>
        CubeProgrammerError ConnectStLink(DebugConnectParameters debugConnectParameters);

        /// <summary>
        /// This routine used to apply a target reset, use only with ST-LINK!.
        /// </summary>
        CubeProgrammerError Reset(DebugResetMode rstMode);

        #endregion

        #region [Bootloader]

        //Bootloader module is a way to group Serial interfaces USB/UART/SPI/I2C/CAN functions together.

        /// <summary>
        /// This routine allows to get connected serial ports.
        /// </summary>
        IEnumerable<UsartConnectParameters> GetUsartList();

        /// <summary>
        /// This routine allows to start connection to device through USART interface.
        /// </summary>
        CubeProgrammerError ConnectUsartBootloader(UsartConnectParameters usartConnectParameters);

        /// <summary>
        /// This routine allows to send a single byte through the USART interface.
        /// </summary>
        CubeProgrammerError SendByteUart(int bytes);

        /// <summary>
        /// This routine allows to get connected DFU devices.
        /// </summary>
        int GetDfuDeviceList(ref List<DfuDeviceInfo> dfuDeviceList);

        /// <summary>
        /// This routine allows to start a simple connection through USB DFU interface.
        /// </summary>
        CubeProgrammerError ConnectDfuBootloader(string usbIndex);

        /// <summary>
        /// This routine allows to start connection to device through USB DFU interface.
        /// </summary>
        CubeProgrammerError ConnectDfuBootloader2(DfuConnectParameters dfuParameters);

        /// <summary>
        /// This routine allows to start connection to device through SPI interface.
        /// </summary>
        CubeProgrammerError ConnectSpiBootloader(SpiConnectParameters spiParameters);

        /// <summary>
        /// This routine allows to start connection to device through CAN interface.
        /// </summary>
        CubeProgrammerError ConnectCanBootloader(CanConnectParameters canParameters);

        /// <summary>
        /// This routine allows to start connection to device through I2C interface.
        /// </summary>
        CubeProgrammerError ConnectI2CBootloader(I2CConnectParameters i2CParameters);

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
        DisplayCallBacks SetDisplayCallbacks(InitProgressBar initProgressBar, LogMessageReceived messageReceived, ProgressBarUpdateReceived progressBarUpdate);

        /// <summary>
        /// This routine allows to choose your custom display.
        /// </summary>
        /// <param name="callbacksHandle">Fill the struct to customize the display tool.</param>
        DisplayCallBacks SetDisplayCallbacks(DisplayCallBacks callbacksHandle);

        /// <summary>
        /// This routine allows to choose the verbosity level for display.
        /// </summary>
        /// <param name="level">Indicates the verbosity number 0, 1 or 3.</param>
        void SetVerbosityLevel(VerbosityLevel level);

        /// <summary>
        /// This routine allows to check connection status [maintained or lost].
        /// </summary>
        /// <returns></returns>
        bool CheckDeviceConnection();

        /// <summary>
        /// This routine allows to get general device information.
        /// </summary>
        GeneralInf? GetDeviceGeneralInf();

        /// <summary>
        /// This routine allows to receive memory data on the used interface with the configuration already initialized.
        /// </summary>
        (CubeProgrammerError, byte[]) ReadMemory(string address, int byteSize);

        /// <summary>
        /// This routine allows to write memory data on the user interface with the configuration already initialized.
        /// </summary>
        /// <param name="address">The address to start writing from.</param>
        /// <param name="data">Data buffer.</param>
        CubeProgrammerError WriteMemory(string address, byte[] data);

        /// <summary>
        /// This routine allows to write memory data on the user interface with the configuration already initialized.
        /// Aligns the buffer to a multiple of 8 bytes appending 0xFF if necessary.
        /// </summary>
        /// <param name="address">The address to start writing from.</param>
        /// <param name="data">Data buffer.</param>
        CubeProgrammerError WriteMemoryAutoFill(string address, byte[] data);

        /// <summary>
        /// This routine allows to write memory data and verify on the user interface with the configuration already initialized.
        /// Inside it uses the WriteMemoryAutoFill function.
        /// </summary>
        /// <param name="address">The address to start writing from.</param>
        /// <param name="data">Data buffer.</param>
        CubeProgrammerError WriteMemoryAndVerify(string address, byte[] data);

        /// <summary>
        /// This routine allows to write sector data on the user interface with the configuration already initialized.
        /// </summary>
        /// <param name="address">The address to start writing from.</param>
        /// <param name="data">Data buffer.</param>
        /// <returns>CubeprogrammerNoError if the writing operation correctly finished, otherwise an error occurred.</returns>
        /// <remarks>Unlike ST-LINK interface, the Bootloader interface can access only to some specific memory regions.</remarks>
        /// <remarks>Data size should not exceed sector size.</remarks>
        CubeProgrammerError EditSector(string address, byte[] data);

        /// <summary>
        /// This routine allows to download data from a file to the memory.
        /// File formats that are supported : hex, bin, srec, tsv, elf, axf, out, stm32, ext
        /// </summary>
        CubeProgrammerError DownloadFile(string inputFilePath, string address = "0x08000000", uint skipErase = 0U, uint verify = 1U);

        /// <summary>
        /// This routine allows to run the application.
        /// </summary>
        CubeProgrammerError Execute(string address = "0x08000000");

        /// <summary>
        /// This routine allows to erase the whole Flash memory.
        /// </summary>
        CubeProgrammerError MassErase(string sFlashMemName = "");

        /// <summary>
        /// This routine allows to erase specific sectors of the Flash memory.
        /// </summary>
        CubeProgrammerError SectorErase(uint[] sectors, uint sectorNbr, string sFlashMemName = "");

        /// <summary>
        /// This routine allows to disable the readout protection.
        /// If the memory is not protected, a message appears to indicate that the device is not
        /// under Readout protection and the command has no effects.
        /// </summary>
        CubeProgrammerError ReadUnprotect();

        /// <summary>
        /// This routine allows the TZEN Option Byte regression.
        /// </summary>
        /// <returns>CubeprogrammerNoError if the disabling correctly accomplished, otherwise an error occurred.</returns>
        /// <remarks>Depending on the device used, this routine take a specific time.</remarks>
        CubeProgrammerError TzenRegression();

        /// <summary>
        /// This routine allows to know the interface what is in use.
        /// </summary>
        TargetInterfaceType? GetTargetInterfaceType();

        /// <summary>
        /// This routine allows to drop the current read/write operation.
        /// </summary>
        int GetCancelPointer();

        /// <summary>
        /// This routine allows to open and get data from any supported file extension.
        /// </summary>
        DeviceFileDataC? FileOpen(string filePath);

        /// <summary>
        /// This routine allows to open and get pointer from any supported file extension.
        /// </summary>
        IntPtr FileOpenAsPointer(string filePath);

        /// <summary>
        /// This routine allows to clean up the handled file data.
        /// </summary>
        void FreeFileData(IntPtr data);

        /// <summary>
        /// This routine allows to free a specific memory region, typically used after readMemory().
        /// </summary>
        /// <param name="ptr">The input pointer address.</param>
        void FreeLibraryMemory(IntPtr ptr);

        /// <summary>
        /// This routine allows to verify if the indicated file data is identical to Flash memory content.
        /// </summary>
        CubeProgrammerError Verify(IntPtr fileData, string address);

        /// <summary>
        /// This routine allows to verify if the indicated data[] is identical to Flash memory content.
        /// </summary>
        CubeProgrammerError VerifyMemory(string address, byte[] data);

        /// <summary>
        /// This routine allows to verify if the indicated data[] is identical to Flash memory content.
        /// </summary>
        CubeProgrammerError VerifyMemoryBySegment(string address, byte[] data);

        /// <summary>
        /// This routine allows to save the data file content to another file.
        /// </summary>
        CubeProgrammerError SaveFileToFile(IntPtr fileData, string sFileName);

        /// <summary>
        /// This routine allows to save Flash memory content to file.
        /// </summary>
        CubeProgrammerError SaveMemoryToFile(string address, string size, string fileName);

        /// <summary>
        /// This routine allows to clean up and disconnect the current connected target.
        /// </summary>
        CubeProgrammerError Disconnect();

        /// <summary>
        /// This routine allows to clear the list of each created interface.
        /// </summary>
        void DeleteInterfaceList();

        /// <summary>
        /// This routine allows to enter and make an automatic process for memory management through JTAG/SWD, UART, DFU, SPI, CAN and I²C interfaces.
        /// </summary>
        void AutomaticMode(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1);

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
        void SerialNumberingAutomaticMode(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1, int enableSerialNumbering = 0, int serialAddress = 0, int serialSize = 0, string serialInitialData = "");

        /// <summary>
        /// This routine allows to get Flash storage information.
        /// </summary>
        (CubeProgrammerError, DeviceStorageStructure) GetStorageStructure();

        #endregion

        #region [Option Bytes]

        //OB module groups option bytes functions used by any interface.

        /// <summary>
        /// This routine allows program the given Option Byte.
        /// The option bytes are configured by the end user depending on the application requirements.
        /// </summary>
        CubeProgrammerError SendOptionBytesCmd(string command);

        /// <summary>
        /// This routine allows to get option bytes values of the connected target.
        /// </summary>
        DevicePeripheralC? InitOptionBytesInterface();

        /// <summary>
        /// This routine allows to get option bytes values of the connected target.
        /// </summary>
        DevicePeripheralC? FastRomInitOptionBytesInterface(ushort deviceId);

        /// <summary>
        /// This routine allows to display the Option bytes.
        /// </summary>
        CubeProgrammerError ObDisplay();

        #endregion

        #region [Loaders]

        //Loaders module groups loaders functions.

        /// <summary>
        /// This routine allows to specify the location of Flash Loader.
        /// </summary>
        /// <param name="path">Indicates the full path of the considered folder.</param>
        void SetLoadersPath(string path);

        /// <summary>
        /// This routine allows to specify the path of the external Loaders to be loaded.
        /// </summary>
        /// <param name="path"></param>
        DeviceExternalLoader? SetExternalLoaderPath(string path);

        /// <summary>
        /// This routine allows to specify the path of the external Loaders to be loaded via OBL interfaces.
        /// </summary>
        /// <param name="path">Indicates the full path of the folder containing external Loaders.</param>
        DeviceExternalLoader? SetExternalLoaderOBL(string path);

        /// <summary>
        /// This routine allows to get available external Loaders in th mentioned path.
        /// </summary>
        DeviceExternalStorageInfo? GetExternalLoaders(string path = @".\st\Programmer");

        /// <summary>
        /// This routine allows to unload an external Loaders.
        /// </summary>
        void RemoveExternalLoader(string path);

        /// <summary>
        /// This routine allows to delete all target Flash Loaders.
        /// </summary>
        void DeleteLoaders();

        #endregion

        #region [STM32WB specific]

        /// Specific APIs used exclusively for STM32WB series to manage BLE Stack, and they are available only through USB DFU and UART bootloader interfaces,
        /// except for the "firmwareDelete" and the "firmwareUpgrade", available through USB DFU, UART and SWD interfaces.
        /// Connection under Reset is mandatory.

        /// <summary>
        /// This routine allows to read the device unique identifier.
        /// </summary>
        (CubeProgrammerError, byte[]) GetUID64();

        /// <summary>
        /// This routine allows to erase the BLE stack firmware.
        /// </summary>
        CubeProgrammerError FirmwareDelete();

        /// <summary>
        /// This routine allows to make upgrade of BLE stack firmware or FUS firmware.
        /// </summary>
        CubeProgrammerError FirmwareUpgrade(string filePath, string address, uint firstInstall, uint startStack, uint verify);

        /// <summary>
        /// This routine allows to start the programmed Stack.
        /// </summary>
        CubeProgrammerError StartWirelessStack();

        /// <summary>
        /// This routine allows to start the programmed Stack.
        /// </summary>
        CubeProgrammerError UpdateAuthKey(string filePath);

        /// <summary>
        /// This routine allows to lock the authentication key and once locked, it is no longer possible to change it.
        /// </summary>
        CubeProgrammerError AuthKeyLock();

        /// <summary>
        /// This routine allows to write a customized user key.
        /// </summary>
        CubeProgrammerError WriteUserKey(string filePath, byte keyType);

        /// <summary>
        /// This routine allows to activate the AntiRollBack.
        /// </summary>
        CubeProgrammerError AntiRollBack();

        /// <summary>
        /// This routine allows to start and establish a communication with the FUS operator.
        /// </summary>
        CubeProgrammerError StartFus();

        /// <summary>
        /// This routine allows to set default option Bytes.
        /// </summary>
        /// <returns></returns>
        CubeProgrammerError UnlockChip();

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
        CubeProgrammerError ProgramSsp(string sspFile, string licenseFile, string tfaFile, int hsmSlotId);

        #endregion

        #region [STM32 HSM specific functions]

        //Specific APIs used exclusively for STM32 devices to manage the Hardware Secure Module.

        /// <summary>
        /// This routine aims to get the HSM Firmware Identifier.
        /// </summary>
        /// <param name="hsmSlotId">The slot index of the plugged-in HSM</param>
        /// <returns>string that contains the HSM Firmware Identifier.</returns>
        string GetHsmFirmwareID(int hsmSlotId);

        /// <summary>
        /// This routine aims to get the current HSM counter.
        /// </summary>
        /// <param name="hsmSlotId">The slot index of the plugged-in HSM</param>
        /// <returns>Counter value</returns>
        ulong GetHsmCounter(int hsmSlotId);

        /// <summary>
        /// This routine aims to get the HSM State.
        /// </summary>
        /// <param name="hsmSlotId">The slot index of the plugged-in HSM</param>
        /// <returns>string with possible values: ST_STATE , OEM_STATE, OPERATIONAL_STATE , UNKNOWN_STATE</returns>
        string GetHsmState(int hsmSlotId);

        /// <summary>
        /// This routine aims to get the HSM version.
        /// </summary>
        /// <param name="hsmSlotId">The slot index of the plugged-in HSM</param>
        /// <returns>string with possible values: 1 , 2</returns>
        string GetHsmVersion(int hsmSlotId);

        /// <summary>
        /// This routine aims to get the HSM type.
        /// </summary>
        /// <param name="hsmSlotId">The slot index of the plugged-in HSM</param>
        /// <returns>string with possible values: SFI. SMU. SSP...</returns>
        string GetHsmType(int hsmSlotId);

        /// <summary>
        /// This routine aims to get and save the HSM license into a binary file.
        /// Connection to target must be established before performing this routine.
        /// </summary>
        /// <param name="hsmSlotId">The slot index of the plugged-in HSM</param>
        /// <param name="outLicensePath">Path of the output binary file.</param>
        /// <returns>0 if the operation was finished successfully, otherwise an error occurred.</returns>
        CubeProgrammerError GetHsmLicense(int hsmSlotId, string outLicensePath);

        #endregion

        #region [EXTENDED]

        //string VersionAPI();

        void Halt();

        void Run();

        #endregion

        #region [Util]

        /// <summary>
        /// HexConverterToUint
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        uint HexConverterToUint(string hex);

        /// <summary>
        /// HexConverterToInt
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        int HexConverterToInt(string hex);

        /// <summary>
        /// HexConverterToString
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        string HexConverterToString(uint hex);

        /// <summary>
        /// HexConverterToString
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        string HexConverterToString(int hex);

        #endregion

    }
}
