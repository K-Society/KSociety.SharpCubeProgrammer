// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Interface
{
    using System;
    using System.Collections.Generic;
    using DeviceDataStructure;
    using Enum;
    using Events;
    using Struct;

    public interface ICubeProgrammerApi
    {
        public event EventHandler<StLinkFoundEventArgs> StLinksFoundStatus;
        public event EventHandler<StLinkAddedEventArgs> StLinkAdded;
        public event EventHandler<StLinkRemovedEventArgs> StLinkRemoved;

        public event EventHandler<Stm32BootLoaderFoundEventArgs> Stm32BootLoaderFoundStatus;
        public event EventHandler<Stm32BootLoaderAddedEventArgs> Stm32BootLoaderAdded;
        public event EventHandler<Stm32BootLoaderRemovedEventArgs> Stm32BootLoaderRemoved;

        public bool StLinkReady { get; }

        public bool Stm32BootLoaderReady { get; }

        public void GetStLinkPorts();

        //void Register();

        //public event EventHandler<NewMessageEventArgs> NewMessage;

        #region [STLINK]

        //STLINK module groups debug ports JTAG/SWD functions together.

        /// <summary>
        /// This routine allows to get ST-LINK conneted probe(s).
        /// </summary>
        /// <param name="shared"></param>
        /// <returns></returns>
        IEnumerable<DebugConnectParameters> GetStLinkList(bool shared = false);

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
        //void SetDisplayCallbacks(InitProgressBar initProgressBar, LogMessageReceived messageReceived, ProgressBarUpdateReceived progressBarUpdate);

        /// <summary>
        /// This routine allows to choose the verbosity level for display.
        /// </summary>
        /// <param name="level"></param>
        //void SetVerbosityLevel(CubeProgrammerVerbosityLevel level);

        /// <summary>
        /// This routine allows to check connection status [maintained or lost].
        /// </summary>
        /// <returns></returns>
        bool CheckDeviceConnection();

        /// <summary>
        /// This routine allows to get general device informations.
        /// </summary>
        GeneralInf? GetDeviceGeneralInf();

        /// <summary>
        /// This routine allows to receive memory data on the used interface with the configration already initialized.
        /// </summary>
        (CubeProgrammerError, byte[]) ReadMemory(string address, int byteSize);

        /// <summary>
        /// This routine allows to write memory data on the user interface with the configration already initialized.
        /// </summary>
        CubeProgrammerError WriteMemory(string address, byte[] data);

        /// <summary>
        /// This routine allows to download data from a file to the memory.
        /// File formats that are supported : hex, bin, srec, tsv, elf, axf, out, stm32, ext
        /// </summary>
        CubeProgrammerError DownloadFile(string inputFilePath, string address, uint skipErase, uint verify);

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
        void ReadUnprotect();

        /// <summary>
        /// This routine allows to know the interface what is in use.
        /// </summary>
        void GetTargetInterfaceType();

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
        void FreeFileData();

        /// <summary>
        /// This routine allows to verfiy if the indicated file data is identical to Flash memory content.
        /// </summary>
        CubeProgrammerError Verify(byte[] data, string address);

        /// <summary>
        /// This routine allows to verfiy if the indicated data[] is identical to Flash memory content.
        /// </summary>
        CubeProgrammerError VerifyMemory(string address, byte[] data);

        /// <summary>
        /// This routine allows to save the data file content to another file.
        /// </summary>
        void SaveFileToFile();

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
        void AutomaticMode();

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
        //CubeProgrammerError SendOptionBytesCmd2(string command);

        /// <summary>
        /// This routine allows to get option bytes values of the connected target.
        /// </summary>
        PeripheralC? InitOptionBytesInterface();

        /// <summary>
        /// This routine allows to diplay the Option bytes.
        /// </summary>
        CubeProgrammerError ObDisplay();

        #endregion

        #region [Loaders]

        //Loaders module groups loaders functions.

        /// <summary>
        /// This routine allows to specify the location of Flash Loader.
        /// </summary>
        /// <param name="path"></param>
        //void SetLoadersPath(string path);

        /// <summary>
        /// This routine allows to specify the path of the external Loaders to be loaded.
        /// </summary>
        /// <param name="path"></param>
        void SetExternalLoadersPath(string path);

        /// <summary>
        /// This routine allows to get available external Loaders in th mentioned path.
        /// </summary>
        void GetExternalLoaders();

        /// <summary>
        /// This routine allows to unload an external Loaders.
        /// </summary>
        void RemoveExternalLoader();

        /// <summary>
        /// This routine allows to delete all target Flash Loaders.
        /// </summary>
        void DeleteLoaders();

        #endregion

        #region [STM32WB specific]

        /// Specific APIs used exclusively for STM32WB series to manage BLE Stack and they are available only through USB DFU and UART bootloader interfaces,
        /// except for the “firmwareDelete" and the “firmwareUpgrade", available through USB DFU, UART and SWD interfaces.
        /// Connection under Reset is mandatory.

        /// <summary>
        /// This routine allows to read the device unique identifier.
        /// </summary>
        void GetUID64();

        /// <summary>
        /// This routine allows to erase the BLE stack firmware.
        /// </summary>
        void FirmwareDelete();

        /// <summary>
        /// This routine allows to make upgrade of BLE stack firmware or FUS firmware.
        /// </summary>
        void FirmwareUpgrade();

        /// <summary>
        /// This routine allows to start the programmed Stack.
        /// </summary>
        void StartWirelessStack();

        /// <summary>
        /// This routine allows to start the programmed Stack.
        /// </summary>
        void UpdateAuthKey();

        /// <summary>
        /// This routine allows to lock the authentication key and once locked, it is no longer possible to change it.
        /// </summary>
        void AuthKeyLock();

        /// <summary>
        /// This routine allows to write a customized user key.
        /// </summary>
        void WriteUserKey();

        /// <summary>
        /// This routine allows to activate the AntiRollBack.
        /// </summary>
        void AntiRollBack();

        /// <summary>
        /// This routine allows to start and establish a communication with the FUS operator.
        /// </summary>
        void StartFus();

        /// <summary>
        /// This routine allows to set default option Bytes.
        /// </summary>
        /// <returns></returns>
        void UnlockChip();

        #endregion

        #region [Util]

        uint HexConverterToUint(string hex);

        int HexConverterToInt(string hex);

        #endregion

    }
}
