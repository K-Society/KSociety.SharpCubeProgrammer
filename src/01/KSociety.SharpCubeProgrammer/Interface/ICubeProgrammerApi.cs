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

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/TryConnectStLink/*'/>
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

        #region [Option Bytes functions]

        //OB module groups option bytes functions used by any interface.

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/SendOptionBytesCmd/*'/>
        CubeProgrammerError SendOptionBytesCmd(string command);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/InitOptionBytesInterface/*'/>
        DevicePeripheralC? InitOptionBytesInterface();

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/FastRomInitOptionBytesInterface/*'/>
        DevicePeripheralC? FastRomInitOptionBytesInterface(ushort deviceId);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/ObDisplay/*'/>
        CubeProgrammerError ObDisplay();

        #endregion

        #region [Loaders functions]

        //Loaders module groups loaders functions.

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/SetLoadersPath/*'/>
        void SetLoadersPath(string path);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/SetExternalLoaderPath/*'/>
        DeviceExternalLoader? SetExternalLoaderPath(string path);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/SetExternalLoaderOBL/*'/>
        DeviceExternalLoader? SetExternalLoaderOBL(string path);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/GetExternalLoaders/*'/>
        DeviceExternalStorageInfo? GetExternalLoaders(string path = @".\st\Programmer");

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/RemoveExternalLoader/*'/>
        void RemoveExternalLoader(string path);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/DeleteLoaders/*'/>
        void DeleteLoaders();

        #endregion

        #region [STM32WB specific functions]

        /// Specific APIs used exclusively for STM32WB series to manage BLE Stack, and they are available only through USB DFU and UART bootloader interfaces,
        /// except for the "firmwareDelete" and the "firmwareUpgrade", available through USB DFU, UART and SWD interfaces.
        /// Connection under Reset is mandatory.

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/GetUID64/*'/>
        (CubeProgrammerError, byte[]) GetUID64();

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/FirmwareDelete/*'/>
        bool FirmwareDelete();

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/FirmwareUpgrade/*'/>
        bool FirmwareUpgrade(string filePath, string address, WbFunctionArguments firstInstall, WbFunctionArguments startStack, WbFunctionArguments verify);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/StartWirelessStack/*'/>
        bool StartWirelessStack();

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/UpdateAuthKey/*'/>
        bool UpdateAuthKey(string filePath);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/AuthKeyLock/*'/>
        CubeProgrammerError AuthKeyLock();

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/WriteUserKey/*'/>
        CubeProgrammerError WriteUserKey(string filePath, byte keyType);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/AntiRollBack/*'/>
        bool AntiRollBack();

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/StartFus/*'/>
        bool StartFus();

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/UnlockChip/*'/>
        CubeProgrammerError UnlockChip();

        #endregion

        #region [STM32MP specific functions]

        //Specific APIs used exclusively for STM32MP devices. The connection is available only through USB DFU and UART interfaces

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/ProgramSsp/*'/>
        CubeProgrammerError ProgramSsp(string sspFile, string licenseFile, string tfaFile, int hsmSlotId);

        #endregion

        #region [STM32 HSM specific functions]

        //Specific APIs used exclusively for STM32 devices to manage the Hardware Secure Module.

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/GetHsmFirmwareID/*'/>
        string GetHsmFirmwareID(int hsmSlotId);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/GetHsmCounter/*'/>
        ulong GetHsmCounter(int hsmSlotId);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/GetHsmState/*'/>
        string GetHsmState(int hsmSlotId);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/GetHsmVersion/*'/>
        string GetHsmVersion(int hsmSlotId);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/GetHsmType/*'/>
        string GetHsmType(int hsmSlotId);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/GetHsmLicense/*'/>
        CubeProgrammerError GetHsmLicense(int hsmSlotId, string outLicensePath);

        #endregion

        #region [EXTENDED]

        void Halt();

        void Run();

        void Step();

        #endregion

        #region [Util]

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/HexConverterToUint/*'/>
        uint HexConverterToUint(string hex);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/HexConverterToInt/*'/>
        int HexConverterToInt(string hex);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/HexConverterToString/*'/>
        string HexConverterToString(uint hex);

        /// <include file='..\Doc\CubeProgrammerApi.xml' path='docs/members[@name="cubeProgrammerApi"]/HexConverterToString/*'/>
        string HexConverterToString(int hex);

        #endregion

    }
}
