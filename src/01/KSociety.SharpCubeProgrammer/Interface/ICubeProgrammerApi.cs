// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DeviceDataStructure;
    using Enum;
    using Events;
    using Struct;

    public interface ICubeProgrammerApi : IDisposable
    {
        event EventHandler<StLinkFoundEventArgs>? StLinksFoundStatus;
        event EventHandler<StLinkAddedEventArgs>? StLinkAdded;
        event EventHandler<StLinkRemovedEventArgs>? StLinkRemoved;

        event EventHandler<Stm32BootLoaderFoundEventArgs>? Stm32BootLoaderFoundStatus;
        event EventHandler<Stm32BootLoaderAddedEventArgs>? Stm32BootLoaderAdded;
        event EventHandler<Stm32BootLoaderRemovedEventArgs>? Stm32BootLoaderRemoved;

        bool StLinkReady { get; }

        bool Stm32BootLoaderReady { get; }

        ValueTask GetStLinkPorts(CancellationToken cancellationToken = default);

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
        void GetUsartList();

        /// <summary>
        /// This routine allows to start connection to device through USART interface.
        /// </summary>
        void ConnectUsartBootloader();

        /// <summary>
        /// This routine allows to send a single byte through the USART interface.
        /// </summary>
        void SendByteUart();

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
        void ConnectDfuBootloader2();

        /// <summary>
        /// This routine allows to start connection to device through SPI interface.
        /// </summary>
        void ConnectSpiBootloader();

        /// <summary>
        /// This routine allows to start connection to device through CAN interface.
        /// </summary>
        void ConnectCanBootloader();

        /// <summary>
        /// This routine allows to start connection to device through I2C interface.
        /// </summary>
        void ConnectI2cBootloader();

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
        void SetDisplayCallbacks(DisplayCallBacks callbacksHandle);

        /// <summary>
        /// This routine allows to choose the verbosity level for display.
        /// </summary>
        /// <param name="level">Indicates the verbosity number 0, 1 or 3.</param>
        void SetVerbosityLevel(CubeProgrammerVerbosityLevel level);

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
        CubeProgrammerError WriteMemory(string address, byte[] data);

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
        CubeProgrammerError DownloadFile(string inputFilePath, string address, uint skipErase = 0U, uint verify = 1U);

        /// <summary>
        /// This routine allows to run the application.
        /// </summary>
        CubeProgrammerError Execute(string address);

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
        void GetCancelPointer();

        /// <summary>
        /// This routine allows to open and get data from any supported file extension.
        /// </summary>
        FileDataC? FileOpen(string filePath);

        /// <summary>
        /// This routine allows to clean up the handled file data.
        /// </summary>
        void FreeFileData(FileDataC data);

        /// <summary>
        /// This routine allows to verify if the indicated file data is identical to Flash memory content.
        /// </summary>
        CubeProgrammerError Verify(byte[] data, string address);

        /// <summary>
        /// This routine allows to verify if the indicated data[] is identical to Flash memory content.
        /// </summary>
        CubeProgrammerError VerifyMemory(string address, byte[] data);

        /// <summary>
        /// This routine allows to save the data file content to another file.
        /// </summary>
        CubeProgrammerError SaveFileToFile(FileDataC fileData, string sFileName);

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
        ExternalLoader SetExternalLoaderPath(string path);

        /// <summary>
        /// This routine allows to get available external Loaders in th mentioned path.
        /// </summary>
        IEnumerable<ExternalLoader> GetExternalLoaders(string path = @".\st\Programmer");

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
        /// except for the “firmwareDelete" and the “firmwareUpgrade", available through USB DFU, UART and SWD interfaces.
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
