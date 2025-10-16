// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Native
{
    using System;
    using System.Runtime.InteropServices;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Struct;

    internal static class Functions
    {
        #region [STLINK]

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int GetStLinkList(ref IntPtr stLinkList, int shared);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int GetStLinkEnumerationList(ref IntPtr stLinkList, int shared);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int ConnectStLink(DebugConnectParameters debugConnectParameters);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int Reset([MarshalAs(UnmanagedType.U4)] DebugResetMode rstMode);

        #endregion

        #region [Bootloader]

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int GetUsartList(ref IntPtr usartList);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int ConnectUsartBootloader(UsartConnectParameters usartParameters);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int SendByteUart(int byteToSend);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int GetDfuDeviceList(ref IntPtr dfuList, int iPID, int iVID);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int ConnectDfuBootloader(IntPtr usbIndex);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int ConnectDfuBootloader2(DfuConnectParameters dfuParameters);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int ConnectSpiBootloader(SpiConnectParameters spiParameters);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int ConnectCanBootloader(CanConnectParameters canParameters);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int ConnectI2cBootloader(I2CConnectParameters i2cParameters);

        #endregion

        #region [General purposes]

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate void SetDisplayCallbacks(DisplayCallBacks c);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate void SetVerbosityLevel(int level);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int CheckDeviceConnection();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate IntPtr GetDeviceGeneralInf();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int ReadMemory(uint address, ref IntPtr data, uint size);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int WriteMemory(uint address, IntPtr data, uint size);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int EditSector(uint address, IntPtr data, uint size);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int DownloadFile([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, [MarshalAs(UnmanagedType.LPWStr)] string binPath);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int Execute(uint address);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int MassErase(IntPtr sFlashMemName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int SectorErase(uint[] sectors, uint sectorNbr, IntPtr sFlashMemName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int ReadUnprotect();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int TzenRegression();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int GetTargetInterfaceType();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int GetCancelPointer();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate IntPtr FileOpen([MarshalAs(UnmanagedType.LPWStr)] string filePath);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate void FreeFileData(IntPtr data);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate void FreeLibraryMemory(IntPtr ptr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int Verify(IntPtr fileData, uint address);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int SaveFileToFile(IntPtr fileData, [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int SaveMemoryToFile(int address, int size, [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int Disconnect();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate void DeleteInterfaceList();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate void AutomaticMode([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, int isMassErase, IntPtr obCommand, int run);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate void SerialNumberingAutomaticMode([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, int isMassErase, IntPtr obCommand, int run, int enableSerialNumbering, int serialAddress, int serialSize, string serialInitialData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int GetStorageStructure(ref IntPtr deviceStorageStruct);

        #endregion

        #region [Option Bytes functions]

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int SendOptionBytesCmd(IntPtr command);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate IntPtr InitOptionBytesInterface();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate IntPtr FastRomInitOptionBytesInterface(ushort deviceId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int ObDisplay();

        #endregion

        #region [Loaders functions]

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate void SetLoadersPath(string path);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate void SetExternalLoaderPath(string path, ref IntPtr externalLoaderInfo);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate void SetExternalLoaderOBL(string path, ref IntPtr externalLoaderInfo);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int GetExternalLoaders(string path, ref IntPtr externalStorageNfo);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate void RemoveExternalLoader(string path);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate void DeleteLoaders();

        #endregion

        #region [STM32WB specific functions]

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int GetUID64(ref IntPtr data);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int FirmwareDelete();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int FirmwareUpgrade([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint firstInstall, uint startStack, uint verify);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int StartWirelessStack();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int UpdateAuthKey([MarshalAs(UnmanagedType.LPWStr)] string filePath);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int AuthKeyLock();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int WriteUserKey([MarshalAs(UnmanagedType.LPWStr)] string filePath, byte keyType);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int AntiRollBack();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int StartFus();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int UnlockChip();

        #endregion

        #region [STM32MP specific functions]

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int ProgramSsp([MarshalAs(UnmanagedType.LPWStr)] string sspFile, [MarshalAs(UnmanagedType.LPWStr)] string licenseFile, [MarshalAs(UnmanagedType.LPWStr)] string tfaFile, int hsmSlotId);

        #endregion

        #region [STM32 HSM specific functions]

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate string GetHsmFirmwareID(int hsmSlotId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate ulong GetHsmCounter(int hsmSlotId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate string GetHsmState(int hsmSlotId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate string GetHsmVersion(int hsmSlotId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate string GetHsmType(int hsmSlotId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate int GetHsmLicense(int hsmSlotId, [MarshalAs(UnmanagedType.LPWStr)] string outLicensePath);

        #endregion

        #region [EXTENDED]

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate void CpuHalt();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate void CpuRun();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = false)]
        internal delegate void CpuStep();

        #endregion
    }
}
