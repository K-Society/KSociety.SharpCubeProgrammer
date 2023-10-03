// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Native
{
    using System;
    using System.Runtime.InteropServices;
    using Enum;
    using Struct;

    internal static class ProgrammerApi
    {
        private const string ProgrammerDll = @".\Programmer.dll";

        #region [STLINK]

        [DllImport(ProgrammerDll, EntryPoint = "GetStLinkList", CallingConvention = CallingConvention.Cdecl,
            SetLastError = true)]
        internal static extern int GetStLinkList(IntPtr stLinkList, int shared);

        [DllImport(ProgrammerDll, EntryPoint = "ConnectStLink", CallingConvention = CallingConvention.Cdecl,
            SetLastError = true)]
        internal static extern int ConnectStLink(DebugConnectParameters debugParameters);

        [DllImport(ProgrammerDll, EntryPoint = "Reset", CallingConvention = CallingConvention.Cdecl,
            SetLastError = true)]
        internal static extern int Reset([MarshalAs(UnmanagedType.U4)] DebugResetMode rstMode);

        #endregion

        #region [Bootloader]

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int GetUsartList(IntPtr usartList);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern int ConnectUsartBootloader(
            [MarshalAs(UnmanagedType.U4)] UsartConnectParameters usartParameters);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int SendByteUart(int bytes);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int GetDfuDeviceList(IntPtr dfuList, int iPID, int iVID);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern int ConnectDfuBootloader(string usbIndex);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int ConnectDfuBootloader2(
            [MarshalAs(UnmanagedType.U4)] DfuConnectParameters dfuParameters);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int ConnectSpiBootloader(
            [MarshalAs(UnmanagedType.U4)] SpiConnectParameters spiParameters);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int ConnectCanBootloader(
            [MarshalAs(UnmanagedType.U4)] CanConnectParameters canParameters);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int ConnectI2cBootloader(
            [MarshalAs(UnmanagedType.U4)] I2cConnectParameters i2cParameters);

        #endregion

        #region [General purposes]

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern void SetDisplayCallbacks(DisplayCallBacks c);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern void SetVerbosityLevel(int level);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern bool CheckDeviceConnection();

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern IntPtr GetDeviceGeneralInf();

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int ReadMemory(uint address, out IntPtr data, uint size);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int WriteMemory(uint address, IntPtr data, uint size);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern int DownloadFile([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address,
            uint skipErase, uint verify, [MarshalAs(UnmanagedType.LPWStr)] string binPath);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern int Execute(uint address);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern int MassErase(string sFlashMemName);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern int SectorErase(uint[] sectors, uint sectorNbr, string sFlashMemName);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int ReadUnprotect();

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int GetTargetInterfaceType();

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern IntPtr GetCancelPointer();

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern IntPtr FileOpen([MarshalAs(UnmanagedType.LPWStr)] string filePath);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern void FreeFileData(FileDataC data);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int Verify(FileDataC fileData, uint address);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int VerifyMemory(uint address, IntPtr data, uint size);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern int SaveFileToFile(FileDataC fileData,
            [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern int SaveMemoryToFile(int address, int size,
            [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int Disconnect();

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern void DeleteInterfaceList();

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern void AutomaticMode([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address,
            uint skipErase, uint verify, int isMassErase, string obCommand, int run);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int GetStorageStructure(IntPtr deviceStorageStruct);
        //internal static extern int GetStorageStructure(ref StorageStructure deviceStorageStruct);

        #endregion

        #region [Option Bytes]

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern int SendOptionBytesCmd(string command);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern IntPtr InitOptionBytesInterface();

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int ObDisplay();

        #endregion

        #region [Loaders]

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern void SetLoadersPath(string path);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern void SetExternalLoaderPath(string path, IntPtr externalLoaderInfo);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern void GetExternalLoaders(string path, IntPtr externalStorageNfo);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern void RemoveExternalLoader(string path);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern void DeleteLoaders();

        #endregion

        #region [STM32WB specific]

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int GetUID64(out IntPtr data);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int FirmwareDelete();

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern int FirmwareUpgrade(string filePath, uint address, uint firstInstall, uint startStack,
            uint verify);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int StartWirelessStack();

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern int UpdateAuthKey(string filePath);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int AuthKeyLock();

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            SetLastError = true)]
        internal static extern int WriteUserKey(string filePath, byte keyType);

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int AntiRollBack();

        [DllImport(ProgrammerDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int StartFus();

        #endregion

    }
}
