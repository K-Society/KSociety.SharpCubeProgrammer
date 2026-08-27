// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Interface
{
    using System;
    using System.Collections.Generic;
    using DeviceDataStructure;
    using Enum;
    using Struct;

    /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/CubeProgrammerApi/*'/>
    public interface ICubeProgrammerApi : IDisposable
    {

        #region [STLINK]

        //STLINK module groups debug ports JTAG/SWD functions together.

        CubeProgrammerError TryConnectStLink(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/GetStLinkList/*'/>
        IEnumerable<DebugConnectParameters> GetStLinkList(bool shared = false);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/GetStLinkEnumerationList/*'/>
        IEnumerable<DebugConnectParameters> GetStLinkEnumerationList(bool shared = false);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/ConnectStLink/*'/>
        CubeProgrammerError ConnectStLink(DebugConnectParameters debugConnectParameters);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/Reset/*'/>
        CubeProgrammerError Reset(DebugResetMode rstMode);

        #endregion

        #region [Bootloader]

        //Bootloader module is a way to group Serial interfaces USB/UART/SPI/I2C/CAN functions together.

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/GetUsartList/*'/>
        IEnumerable<UsartConnectParameters> GetUsartList();

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/ConnectUsartBootloader/*'/>
        CubeProgrammerError ConnectUsartBootloader(UsartConnectParameters usartConnectParameters);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/SendByteUart/*'/>
        CubeProgrammerError SendByteUart(int @byte);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/GetDfuDeviceList/*'/>
        IEnumerable<DfuDeviceInfo> GetDfuDeviceList(int iPID = 0xdf11, int iVID = 0x0483);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/ConnectDfuBootloader/*'/>
        CubeProgrammerError ConnectDfuBootloader(string usbIndex);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/ConnectDfuBootloader2/*'/>
        CubeProgrammerError ConnectDfuBootloader2(DfuConnectParameters dfuParameters);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/ConnectDfuBootloader2Overload/*'/>
        CubeProgrammerError ConnectDfuBootloader2(string usbIndex, byte rdu, byte tzenreg, int usbTimeout = 30000);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/ConnectSpiBootloader/*'/>
        CubeProgrammerError ConnectSpiBootloader(SpiConnectParameters spiParameters);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/ConnectCanBootloader/*'/>
        CubeProgrammerError ConnectCanBootloader(CanConnectParameters canParameters);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/ConnectI2CBootloader/*'/>
        CubeProgrammerError ConnectI2CBootloader(I2cConnectParameters i2CParameters);

        #endregion

        #region [General purposes]

        // General module groups general purposes functions used by any interface.

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/SetDisplayCallbacksOverload/*'/>
        DisplayCallBacks SetDisplayCallbacks(InitProgressBar initProgressBar, LogMessageReceived messageReceived, ProgressBarUpdateReceived progressBarUpdate);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/SetDisplayCallbacks/*'/>
        DisplayCallBacks SetDisplayCallbacks(DisplayCallBacks c);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/SetVerbosityLevel/*'/>
        void SetVerbosityLevel(VerbosityLevel level);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/CheckDeviceConnection/*'/>
        bool CheckDeviceConnection();

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/GetDeviceGeneralInf/*'/>
        GeneralInf? GetDeviceGeneralInf();

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/ReadMemory/*'/>
        (CubeProgrammerError, byte[]) ReadMemory(string address, int byteSize);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/WriteMemory/*'/>
        CubeProgrammerError WriteMemory(string address, byte[] data, int size = 0);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/WriteMemoryAutoFill/*'/>
        CubeProgrammerError WriteMemoryAutoFill(string address, byte[] data, int size = 0);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/WriteMemoryAndVerify/*'/>
        CubeProgrammerError WriteMemoryAndVerify(string address, byte[] data, int size = 0);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/EditSector/*'/>
        CubeProgrammerError EditSector(string address, byte[] data, int size = 0);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/DownloadFile/*'/>
        CubeProgrammerError DownloadFile(string inputFilePath, string address = "0x08000000", uint skipErase = 0U, uint verify = 1U, string binFilePath = "");

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/Execute/*'/>
        CubeProgrammerError Execute(string address = "0x08000000");

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/MassErase/*'/>
        CubeProgrammerError MassErase(string sFlashMemName = "");

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/SectorErase/*'/>
        CubeProgrammerError SectorErase(uint[] sectors, uint sectorNbr, string sFlashMemName = "");

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/ReadUnprotect/*'/>
        CubeProgrammerError ReadUnprotect();

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/TzenRegression/*'/>
        CubeProgrammerError TzenRegression();

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/GetTargetInterfaceType/*'/>
        TargetInterfaceType? GetTargetInterfaceType();

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/GetCancelPointer/*'/>
        int GetCancelPointer();

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/FileOpen/*'/>
        DeviceFileDataC? FileOpen(string filePath);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/FileOpenAsPointer/*'/>
        IntPtr FileOpenAsPointer(string filePath);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/FreeFileData/*'/>
        void FreeFileData(IntPtr data);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/FreeLibraryMemory/*'/>
        void FreeLibraryMemory(IntPtr ptr);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/Verify/*'/>
        CubeProgrammerError Verify(IntPtr fileData, string address);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/VerifyMemory/*'/>
        CubeProgrammerError VerifyMemory(string address, byte[] data);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/VerifyMemoryBySegment/*'/>
        CubeProgrammerError VerifyMemoryBySegment(string address, byte[] data);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/SaveFileToFile/*'/>
        CubeProgrammerError SaveFileToFile(IntPtr fileData, string sFileName);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/SaveMemoryToFile/*'/>
        CubeProgrammerError SaveMemoryToFile(string address, string size, string fileName);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/Disconnect/*'/>
        void Disconnect();

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/DeleteInterfaceList/*'/>
        void DeleteInterfaceList();

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/AutomaticMode/*'/>
        void AutomaticMode(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/SerialNumberingAutomaticMode/*'/>
        void SerialNumberingAutomaticMode(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1, int enableSerialNumbering = 0, int serialAddress = 0, int serialSize = 0, string serialInitialData = "");

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/GetStorageStructure/*'/>
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
        bool FirmwareDelete();

        /// <summary>
        /// This routine allows to make upgrade of BLE stack firmware or FUS firmware.
        /// </summary>
        bool FirmwareUpgrade(string filePath, string address, WbFunctionArguments firstInstall, WbFunctionArguments startStack, WbFunctionArguments verify);

        /// <summary>
        /// This routine allows to start the programmed Stack.
        /// </summary>
        bool StartWirelessStack();

        /// <summary>
        /// This routine allows to start the programmed Stack.
        /// </summary>
        bool UpdateAuthKey(string filePath);

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
        bool AntiRollBack();

        /// <summary>
        /// This routine allows to start and establish a communication with the FUS operator.
        /// </summary>
        bool StartFus();

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

        void Step();

        //string WindowsVersion();

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
