// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Native
{
    using System;
    using System.Runtime.InteropServices;
    using Enum;
    using Struct;

    internal static class ProgrammerApi
    {
        private const string ProgrammerDll32 = @".\dll\x86\Programmer.dll";
        private const string ProgrammerDll64 = @".\dll\x64\Programmer.dll";

        #region [STLINK]

        #region [TryConnectStLink]

        [DllImport(ProgrammerDll32, EntryPoint = "TryConnectStLink", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int TryConnectStLink32(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode);

        [DllImport(ProgrammerDll64, EntryPoint = "TryConnectStLink", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int TryConnectStLink64(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode);

        private static int TryConnectStLinkNative(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode)
        {
            return !Environment.Is64BitProcess
                ? TryConnectStLink32(stLinkProbeIndex, shared, debugConnectMode)
                : TryConnectStLink64(stLinkProbeIndex, shared, debugConnectMode);
        }

        internal static int TryConnectStLink(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode)
        {
            try
            {
                return TryConnectStLinkNative(stLinkProbeIndex, shared, debugConnectMode);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [GetStLinkList]

        [DllImport(ProgrammerDll32, EntryPoint = "GetStLinkList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetStLinkList32(ref IntPtr stLinkList, int shared);

        [DllImport(ProgrammerDll64, EntryPoint = "GetStLinkList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetStLinkList64(ref IntPtr stLinkList, int shared);

        private static int GetStLinkListNative(ref IntPtr stLinkList, int shared)
        {
            return !Environment.Is64BitProcess
                ? GetStLinkList32(ref stLinkList, shared)
                : GetStLinkList64(ref stLinkList, shared);
        }

        internal static int GetStLinkList(ref IntPtr stLinkList, int shared)
        {
            try
            {
                return GetStLinkListNative(ref stLinkList, shared);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [GetStLinkEnumerationList]

        [DllImport(ProgrammerDll32, EntryPoint = "GetStLinkEnumerationList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetStLinkEnumerationList32(ref IntPtr stLinkList, int shared);

        [DllImport(ProgrammerDll64, EntryPoint = "GetStLinkEnumerationList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetStLinkEnumerationList64(ref IntPtr stLinkList, int shared);

        private static int GetStLinkEnumerationListNative(ref IntPtr stLinkList, int shared)
        {
            return !Environment.Is64BitProcess
                ? GetStLinkEnumerationList32(ref stLinkList, shared)
                : GetStLinkEnumerationList64(ref stLinkList, shared);
        }

        internal static int GetStLinkEnumerationList(ref IntPtr stLinkList, int shared)
        {
            try
            {
                return GetStLinkEnumerationListNative(ref stLinkList, shared);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [ConnectStLink]

        [DllImport(ProgrammerDll32, EntryPoint = "ConnectStLink", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ConnectStLink32(DebugConnectParameters debugParameters);

        [DllImport(ProgrammerDll64, EntryPoint = "ConnectStLink", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ConnectStLink64(DebugConnectParameters debugParameters);

        private static int ConnectStLinkNative(DebugConnectParameters debugParameters)
        {
            return !Environment.Is64BitProcess
                ? ConnectStLink32(debugParameters)
                : ConnectStLink64(debugParameters);
        }

        internal static int ConnectStLink(DebugConnectParameters debugParameters)
        {
            try
            {
                return ConnectStLinkNative(debugParameters);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [Reset]

        [DllImport(ProgrammerDll32, EntryPoint = "Reset", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int Reset32([MarshalAs(UnmanagedType.U4)] DebugResetMode rstMode);

        [DllImport(ProgrammerDll64, EntryPoint = "Reset", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int Reset64([MarshalAs(UnmanagedType.U4)] DebugResetMode rstMode);

        private static int ResetNative(DebugResetMode rstMode)
        {
            return !Environment.Is64BitProcess
                ? Reset32(rstMode)
                : Reset64(rstMode);
        }

        internal static int Reset(DebugResetMode rstMode)
        {
            try
            {
                return ResetNative(rstMode);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #endregion

        #region [Bootloader]

        #region [GetUsartList]

        [DllImport(ProgrammerDll32, EntryPoint = "GetUsartList", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetUsartList32(ref IntPtr usartList);

        [DllImport(ProgrammerDll64, EntryPoint = "GetUsartList", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetUsartList64(ref IntPtr usartList);

        private static int GetUsartListNative(ref IntPtr usartList)
        {
            return !Environment.Is64BitProcess
                ? GetUsartList32(ref usartList)
                : GetUsartList64(ref usartList);
        }

        internal static int GetUsartList(ref IntPtr usartList)
        {
            try
            {
                return GetUsartListNative(ref usartList);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [ConnectUsartBootloader]

        [DllImport(ProgrammerDll32, EntryPoint = "ConnectUsartBootloader", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ConnectUsartBootloader32(UsartConnectParameters usartParameters);

        [DllImport(ProgrammerDll64, EntryPoint = "ConnectUsartBootloader", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ConnectUsartBootloader64(UsartConnectParameters usartParameters);

        private static int ConnectUsartBootloaderNative(UsartConnectParameters usartParameters)
        {
            return !Environment.Is64BitProcess
                ? ConnectUsartBootloader32(usartParameters)
                : ConnectUsartBootloader64(usartParameters);
        }

        internal static int ConnectUsartBootloader(UsartConnectParameters usartParameters)
        {
            try
            {
                return ConnectUsartBootloaderNative(usartParameters);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [SendByteUart]

        [DllImport(ProgrammerDll32, EntryPoint = "SendByteUart", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int SendByteUart32(int bytes);

        [DllImport(ProgrammerDll64, EntryPoint = "SendByteUart", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int SendByteUart64(int bytes);

        private static int SendByteUartNative(int bytes)
        {
            return !Environment.Is64BitProcess
                ? SendByteUart32(bytes)
                : SendByteUart64(bytes);
        }

        internal static int SendByteUart(int bytes)
        {
            try
            {
                return SendByteUartNative(bytes);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [GetDfuDeviceList]

        [DllImport(ProgrammerDll32, EntryPoint = "GetDfuDeviceList", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetDfuDeviceList32(ref IntPtr dfuList, int iPID, int iVID);

        [DllImport(ProgrammerDll64, EntryPoint = "GetDfuDeviceList", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetDfuDeviceList64(ref IntPtr dfuList, int iPID, int iVID);

        private static int GetDfuDeviceListNative(ref IntPtr dfuList, int iPID, int iVID)
        {
            return !Environment.Is64BitProcess
                ? GetDfuDeviceList32(ref dfuList, iPID, iVID)
                : GetDfuDeviceList64(ref dfuList, iPID, iVID);
        }

        internal static int GetDfuDeviceList(ref IntPtr dfuList, int iPID, int iVID)
        {
            try
            {
                return GetDfuDeviceListNative(ref dfuList, iPID, iVID);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [ConnectDfuBootloader]

        [DllImport(ProgrammerDll32, EntryPoint = "ConnectDfuBootloader", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ConnectDfuBootloader32(string usbIndex);

        [DllImport(ProgrammerDll64, EntryPoint = "ConnectDfuBootloader", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ConnectDfuBootloader64(string usbIndex);

        private static int ConnectDfuBootloaderNative(string usbIndex)
        {
            return !Environment.Is64BitProcess
                ? ConnectDfuBootloader32(usbIndex)
                : ConnectDfuBootloader64(usbIndex);
        }

        internal static int ConnectDfuBootloader(string usbIndex)
        {
            try
            {
                return ConnectDfuBootloaderNative(usbIndex);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [ConnectDfuBootloader2]

        [DllImport(ProgrammerDll32, EntryPoint = "ConnectDfuBootloader2", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ConnectDfuBootloader232(DfuConnectParameters dfuParameters);

        [DllImport(ProgrammerDll64, EntryPoint = "ConnectDfuBootloader2", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ConnectDfuBootloader264(DfuConnectParameters dfuParameters);

        private static int ConnectDfuBootloader2Native(DfuConnectParameters dfuParameters)
        {
            return !Environment.Is64BitProcess
                ? ConnectDfuBootloader232(dfuParameters)
                : ConnectDfuBootloader264(dfuParameters);
        }

        internal static int ConnectDfuBootloader2(DfuConnectParameters dfuParameters)
        {
            try
            {
                return ConnectDfuBootloader2Native(dfuParameters);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [ConnectSpiBootloader]

        [DllImport(ProgrammerDll32, EntryPoint = "ConnectSpiBootloader", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ConnectSpiBootloader32(SpiConnectParameters spiParameters);

        [DllImport(ProgrammerDll64, EntryPoint = "ConnectSpiBootloader", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ConnectSpiBootloader64(SpiConnectParameters spiParameters);

        private static int ConnectSpiBootloaderNative(SpiConnectParameters spiParameters)
        {
            return !Environment.Is64BitProcess
                ? ConnectSpiBootloader32(spiParameters)
                : ConnectSpiBootloader64(spiParameters);
        }

        internal static int ConnectSpiBootloader(SpiConnectParameters spiParameters)
        {
            try
            {
                return ConnectSpiBootloaderNative(spiParameters);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [ConnectCanBootloader]

        [DllImport(ProgrammerDll32, EntryPoint = "ConnectCanBootloader", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ConnectCanBootloader32(CanConnectParameters canParameters);

        [DllImport(ProgrammerDll64, EntryPoint = "ConnectCanBootloader", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ConnectCanBootloader64(CanConnectParameters canParameters);

        private static int ConnectCanBootloaderNative(CanConnectParameters canParameters)
        {
            return !Environment.Is64BitProcess
                ? ConnectCanBootloader32(canParameters)
                : ConnectCanBootloader64(canParameters);
        }

        internal static int ConnectCanBootloader(CanConnectParameters canParameters)
        {
            try
            {
                return ConnectCanBootloaderNative(canParameters);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [ConnectI2cBootloader]

        [DllImport(ProgrammerDll32, EntryPoint = "ConnectI2cBootloader", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ConnectI2cBootloader32(I2CConnectParameters i2cParameters);

        [DllImport(ProgrammerDll64, EntryPoint = "ConnectI2cBootloader", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ConnectI2cBootloader64(I2CConnectParameters i2cParameters);

        private static int ConnectI2cBootloaderNative(I2CConnectParameters i2cParameters)
        {
            return !Environment.Is64BitProcess
                ? ConnectI2cBootloader32(i2cParameters)
                : ConnectI2cBootloader64(i2cParameters);
        }

        internal static int ConnectI2cBootloader(I2CConnectParameters i2cParameters)
        {
            try
            {
                return ConnectI2cBootloaderNative(i2cParameters);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #endregion

        #region [General purposes]

        //[DllImport(ProgrammerDll64, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //internal static extern void SetDisplayCallbacks(DisplayCallBacks c);

        //[DllImport(ProgrammerDll64, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //internal static extern void SetVerbosityLevel(int level);

        #region [CheckDeviceConnection]

        [DllImport(ProgrammerDll32, EntryPoint = "CheckDeviceConnection", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern bool CheckDeviceConnection32();

        [DllImport(ProgrammerDll64, EntryPoint = "CheckDeviceConnection", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern bool CheckDeviceConnection64();

        private static bool CheckDeviceConnectionNative()
        {
            return !Environment.Is64BitProcess
                ? CheckDeviceConnection32()
                : CheckDeviceConnection64();
        }

        internal static bool CheckDeviceConnection()
        {
            try
            {
                return CheckDeviceConnectionNative();
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [GetDeviceGeneralInf]

        [DllImport(ProgrammerDll32, EntryPoint = "GetDeviceGeneralInf", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern IntPtr GetDeviceGeneralInf32();

        [DllImport(ProgrammerDll64, EntryPoint = "GetDeviceGeneralInf", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern IntPtr GetDeviceGeneralInf64();

        private static IntPtr GetDeviceGeneralInfNative()
        {
            return !Environment.Is64BitProcess
                ? GetDeviceGeneralInf32()
                : GetDeviceGeneralInf64();
        }

        internal static IntPtr GetDeviceGeneralInf()
        {
            try
            {
                return GetDeviceGeneralInfNative();
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [ReadMemory]

        [DllImport(ProgrammerDll32, EntryPoint = "ReadMemory", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ReadMemory32(uint address, ref IntPtr data, uint size);

        [DllImport(ProgrammerDll64, EntryPoint = "ReadMemory", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ReadMemory64(uint address, ref IntPtr data, uint size);

        private static int ReadMemoryNative(uint address, ref IntPtr data, uint size)
        {
            return !Environment.Is64BitProcess
                ? ReadMemory32(address, ref data, size)
                : ReadMemory64(address, ref data, size);
        }

        internal static int ReadMemory(uint address, ref IntPtr data, uint size)
        {
            try
            {
                return ReadMemoryNative(address, ref data, size);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [WriteMemory]

        [DllImport(ProgrammerDll32, EntryPoint = "WriteMemory", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int WriteMemory32(uint address, IntPtr data, uint size);

        [DllImport(ProgrammerDll64, EntryPoint = "WriteMemory", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int WriteMemory64(uint address, IntPtr data, uint size);

        private static int WriteMemoryNative(uint address, IntPtr data, uint size)
        {
            return !Environment.Is64BitProcess
                ? WriteMemory32(address, data, size)
                : WriteMemory64(address, data, size);
        }

        internal static int WriteMemory(uint address, IntPtr data, uint size)
        {
            try
            {
                return WriteMemoryNative(address, data, size);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [DownloadFile]

        [DllImport(ProgrammerDll32, EntryPoint = "DownloadFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int DownloadFile32([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, [MarshalAs(UnmanagedType.LPWStr)] string binPath);

        [DllImport(ProgrammerDll64, EntryPoint = "DownloadFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int DownloadFile64([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, [MarshalAs(UnmanagedType.LPWStr)] string binPath);

        private static int DownloadFileNative(string filePath, uint address, uint skipErase, uint verify, string binPath)
        {
            return !Environment.Is64BitProcess
                ? DownloadFile32(filePath, address, skipErase, verify, binPath)
                : DownloadFile64(filePath, address, skipErase, verify, binPath);
        }

        internal static int DownloadFile(string filePath, uint address, uint skipErase, uint verify, string binPath)
        {
            try
            {
                return DownloadFileNative(filePath, address, skipErase, verify, binPath);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [Execute]

        [DllImport(ProgrammerDll32, EntryPoint = "Execute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int Execute32(uint address);

        [DllImport(ProgrammerDll64, EntryPoint = "Execute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int Execute64(uint address);

        private static int ExecuteNative(uint address)
        {
            return !Environment.Is64BitProcess
                ? Execute32(address)
                : Execute64(address);
        }

        internal static int Execute(uint address)
        {
            try
            {
                return ExecuteNative(address);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [MassErase]

        [DllImport(ProgrammerDll32, EntryPoint = "MassErase", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int MassErase32(string sFlashMemName);

        [DllImport(ProgrammerDll64, EntryPoint = "MassErase", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int MassErase64(string sFlashMemName);

        private static int MassEraseNative(string sFlashMemName)
        {
            return !Environment.Is64BitProcess
                ? MassErase32(sFlashMemName)
                : MassErase64(sFlashMemName);
        }

        internal static int MassErase(string sFlashMemName)
        {
            try
            {
                return MassEraseNative(sFlashMemName);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [SectorErase]

        [DllImport(ProgrammerDll32, EntryPoint = "SectorErase", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SectorErase32(uint[] sectors, uint sectorNbr, string sFlashMemName);

        [DllImport(ProgrammerDll64, EntryPoint = "SectorErase", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SectorErase64(uint[] sectors, uint sectorNbr, string sFlashMemName);

        private static int SectorEraseNative(uint[] sectors, uint sectorNbr, string sFlashMemName)
        {
            return !Environment.Is64BitProcess
                ? SectorErase32(sectors, sectorNbr, sFlashMemName)
                : SectorErase64(sectors, sectorNbr, sFlashMemName);
        }

        internal static int SectorErase(uint[] sectors, uint sectorNbr, string sFlashMemName)
        {
            try
            {
                return SectorEraseNative(sectors, sectorNbr, sFlashMemName);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [ReadUnprotect]

        [DllImport(ProgrammerDll32, EntryPoint = "ReadUnprotect", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ReadUnprotect32();

        [DllImport(ProgrammerDll64, EntryPoint = "ReadUnprotect", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ReadUnprotect64();

        private static int ReadUnprotectNative()
        {
            return !Environment.Is64BitProcess
                ? ReadUnprotect32()
                : ReadUnprotect64();
        }

        internal static int ReadUnprotect()
        {
            try
            {
                return ReadUnprotectNative();
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [GetTargetInterfaceType]

        [DllImport(ProgrammerDll32, EntryPoint = "GetTargetInterfaceType", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetTargetInterfaceType32();

        [DllImport(ProgrammerDll64, EntryPoint = "GetTargetInterfaceType", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetTargetInterfaceType64();

        private static int GetTargetInterfaceTypeNative()
        {
            return !Environment.Is64BitProcess
                ? GetTargetInterfaceType32()
                : GetTargetInterfaceType64();
        }

        internal static int GetTargetInterfaceType()
        {
            try
            {
                return GetTargetInterfaceTypeNative();
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [GetCancelPointer]

        [DllImport(ProgrammerDll32, EntryPoint = "GetCancelPointer", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern IntPtr GetCancelPointer32();

        [DllImport(ProgrammerDll64, EntryPoint = "GetCancelPointer", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern IntPtr GetCancelPointer64();

        private static IntPtr GetCancelPointerNative()
        {
            return !Environment.Is64BitProcess
                ? GetCancelPointer32()
                : GetCancelPointer64();
        }

        internal static IntPtr GetCancelPointer()
        {
            try
            {
                return GetCancelPointerNative();
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [FileOpen]

        [DllImport(ProgrammerDll32, EntryPoint = "FileOpen", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr FileOpen32([MarshalAs(UnmanagedType.LPWStr)] string filePath);

        [DllImport(ProgrammerDll64, EntryPoint = "FileOpen", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr FileOpen64([MarshalAs(UnmanagedType.LPWStr)] string filePath);

        private static IntPtr FileOpenNative(string filePath)
        {
            return !Environment.Is64BitProcess
                ? FileOpen32(filePath)
                : FileOpen64(filePath);
        }

        internal static IntPtr FileOpen(string filePath)
        {
            try
            {
                return FileOpenNative(filePath);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [FreeFileData]

        [DllImport(ProgrammerDll32, EntryPoint = "FreeFileData", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void FreeFileData32(FileDataC data);

        [DllImport(ProgrammerDll64, EntryPoint = "FreeFileData", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void FreeFileData64(FileDataC data);

        private static void FreeFileDataNative(FileDataC data)
        {
            if (!Environment.Is64BitProcess)
            {
                FreeFileData32(data);
            }
            else
            {
                FreeFileData64(data);
            }
        }

        internal static void FreeFileData(FileDataC data)
        {
            try
            {
                FreeFileDataNative(data);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [Verify]

        [DllImport(ProgrammerDll32, EntryPoint = "Verify", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int Verify32(FileDataC fileData, uint address);

        [DllImport(ProgrammerDll64, EntryPoint = "Verify", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int Verify64(FileDataC fileData, uint address);

        private static int VerifyNative(FileDataC fileData, uint address)
        {
            return !Environment.Is64BitProcess
                ? Verify32(fileData, address)
                : Verify64(fileData, address);
        }

        internal static int Verify(FileDataC fileData, uint address)
        {
            try
            {
                return VerifyNative(fileData, address);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [VerifyMemory]

        [DllImport(ProgrammerDll32, EntryPoint = "VerifyMemory", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int VerifyMemory32(uint address, IntPtr data, uint size);

        [DllImport(ProgrammerDll64, EntryPoint = "VerifyMemory", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int VerifyMemory64(uint address, IntPtr data, uint size);

        private static int VerifyMemoryNative(uint address, IntPtr data, uint size)
        {
            return !Environment.Is64BitProcess
                ? VerifyMemory32(address, data, size)
                : VerifyMemory64(address, data, size);
        }

        internal static int VerifyMemory(uint address, IntPtr data, uint size)
        {
            try
            {
                return VerifyMemoryNative(address, data, size);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [SaveFileToFile]

        [DllImport(ProgrammerDll32, EntryPoint = "SaveFileToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SaveFileToFile32(FileDataC fileData, [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        [DllImport(ProgrammerDll64, EntryPoint = "SaveFileToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SaveFileToFile64(FileDataC fileData, [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        private static int SaveFileToFileNative(FileDataC fileData, string sFileName)
        {
            return !Environment.Is64BitProcess
                ? SaveFileToFile32(fileData, sFileName)
                : SaveFileToFile64(fileData, sFileName);
        }

        internal static int SaveFileToFile(FileDataC fileData, string sFileName)
        {
            try
            {
                return SaveFileToFileNative(fileData, sFileName);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [SaveMemoryToFile]

        [DllImport(ProgrammerDll32, EntryPoint = "SaveMemoryToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SaveMemoryToFile32(int address, int size, [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        [DllImport(ProgrammerDll64, EntryPoint = "SaveMemoryToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SaveMemoryToFile64(int address, int size, [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        private static int SaveMemoryToFileNative(int address, int size, string sFileName)
        {
            return !Environment.Is64BitProcess
                ? SaveMemoryToFile32(address, size, sFileName)
                : SaveMemoryToFile64(address, size, sFileName);
        }

        internal static int SaveMemoryToFile(int address, int size, string sFileName)
        {
            try
            {
                return SaveMemoryToFileNative(address, size, sFileName);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [Disconnect]

        [DllImport(ProgrammerDll32, EntryPoint = "Disconnect", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int Disconnect32();

        [DllImport(ProgrammerDll64, EntryPoint = "Disconnect", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int Disconnect64();

        private static int DisconnectNative()
        {
            return !Environment.Is64BitProcess
                ? Disconnect32()
                : Disconnect64();
        }

        internal static int Disconnect()
        {
            try
            {
                return DisconnectNative();
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [DeleteInterfaceList]

        [DllImport(ProgrammerDll32, EntryPoint = "DeleteInterfaceList", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void DeleteInterfaceList32();

        [DllImport(ProgrammerDll64, EntryPoint = "DeleteInterfaceList", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void DeleteInterfaceList64();

        private static void DeleteInterfaceListNative()
        {
            if (!Environment.Is64BitProcess)
            {
                DeleteInterfaceList32();
            }
            else
            {
                DeleteInterfaceList64();
            }
        }

        internal static void DeleteInterfaceList()
        {
            try
            {
                DeleteInterfaceListNative();
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [AutomaticMode]

        [DllImport(ProgrammerDll32, EntryPoint = "AutomaticMode", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void AutomaticMode32([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, int isMassErase, string obCommand, int run);

        [DllImport(ProgrammerDll64, EntryPoint = "AutomaticMode", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void AutomaticMode64([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, int isMassErase, string obCommand, int run);

        private static void AutomaticModeNative(string filePath, uint address, uint skipErase, uint verify, int isMassErase, string obCommand, int run)
        {
            if (!Environment.Is64BitProcess)
            {
                AutomaticMode32(filePath, address, skipErase, verify, isMassErase, obCommand, run);
            }
            else
            {
                AutomaticMode64(filePath, address, skipErase, verify, isMassErase, obCommand, run);
            }
        }

        internal static void AutomaticMode(string filePath, uint address, uint skipErase, uint verify, int isMassErase, string obCommand, int run)
        {
            try
            {
                AutomaticModeNative(filePath, address, skipErase, verify, isMassErase, obCommand, run);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [GetStorageStructure]

        [DllImport(ProgrammerDll32, EntryPoint = "GetStorageStructure", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetStorageStructure32(ref IntPtr deviceStorageStruct);

        [DllImport(ProgrammerDll64, EntryPoint = "GetStorageStructure", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetStorageStructure64(ref IntPtr deviceStorageStruct);

        private static int GetStorageStructureNative(ref IntPtr deviceStorageStruct)
        {
            return !Environment.Is64BitProcess
                ? GetStorageStructure32(ref deviceStorageStruct)
                : GetStorageStructure64(ref deviceStorageStruct);
        }

        internal static int GetStorageStructure(ref IntPtr deviceStorageStruct)
        {
            try
            {
                return GetStorageStructureNative(ref deviceStorageStruct);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #endregion

        #region [Option Bytes]

        #region [SendOptionBytesCmd]

        [DllImport(ProgrammerDll32, EntryPoint = "SendOptionBytesCmd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SendOptionBytesCmd32(string command);

        [DllImport(ProgrammerDll64, EntryPoint = "SendOptionBytesCmd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SendOptionBytesCmd64(string command);

        private static int SendOptionBytesCmdNative(string command)
        {
            return !Environment.Is64BitProcess
                ? SendOptionBytesCmd32(command)
                : SendOptionBytesCmd64(command);
        }

        internal static int SendOptionBytesCmd(string command)
        {
            try
            {
                return SendOptionBytesCmdNative(command);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [InitOptionBytesInterface]

        [DllImport(ProgrammerDll32, EntryPoint = "InitOptionBytesInterface", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern IntPtr InitOptionBytesInterface32();

        [DllImport(ProgrammerDll64, EntryPoint = "InitOptionBytesInterface", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern IntPtr InitOptionBytesInterface64();

        private static IntPtr InitOptionBytesInterfaceNative()
        {
            return !Environment.Is64BitProcess
                ? InitOptionBytesInterface32()
                : InitOptionBytesInterface64();
        }

        internal static IntPtr InitOptionBytesInterface()
        {
            try
            {
                return InitOptionBytesInterfaceNative();
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [FastRomInitOptionBytesInterface]

        [DllImport(ProgrammerDll32, EntryPoint = "FastRomInitOptionBytesInterface", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern IntPtr FastRomInitOptionBytesInterface32(ushort deviceId);

        [DllImport(ProgrammerDll64, EntryPoint = "FastRomInitOptionBytesInterface", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern IntPtr FastRomInitOptionBytesInterface64(ushort deviceId);

        private static IntPtr FastRomInitOptionBytesInterfaceNative(ushort deviceId)
        {
            return !Environment.Is64BitProcess
                ? FastRomInitOptionBytesInterface32(deviceId)
                : FastRomInitOptionBytesInterface64(deviceId);
        }

        internal static IntPtr FastRomInitOptionBytesInterface(ushort deviceId)
        {
            try
            {
                return FastRomInitOptionBytesInterfaceNative(deviceId);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [ObDisplay]

        [DllImport(ProgrammerDll32, EntryPoint = "ObDisplay", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ObDisplay32();

        [DllImport(ProgrammerDll64, EntryPoint = "ObDisplay", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ObDisplay64();

        private static int ObDisplayNative()
        {
            return !Environment.Is64BitProcess
                ? ObDisplay32()
                : ObDisplay64();
        }

        internal static int ObDisplay()
        {
            try
            {
                return ObDisplayNative();
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #endregion

        #region [Loaders]

        #region [SetLoadersPath]

        [DllImport(ProgrammerDll32, EntryPoint = "SetLoadersPath", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void SetLoadersPath32(string path);

        [DllImport(ProgrammerDll64, EntryPoint = "SetLoadersPath", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void SetLoadersPath64(string path);

        private static void SetLoadersPathNative(string path)
        {
            if (!Environment.Is64BitProcess)
            {
                SetLoadersPath32(path);
            }
            else
            {
                SetLoadersPath64(path);
            }
        }

        internal static void SetLoadersPath(string path)
        {
            try
            {
                SetLoadersPathNative(path);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [SetExternalLoaderPath]

        [DllImport(ProgrammerDll32, EntryPoint = "SetExternalLoaderPath", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void SetExternalLoaderPath32(string path, ref IntPtr externalLoaderInfo);

        [DllImport(ProgrammerDll64, EntryPoint = "SetExternalLoaderPath", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void SetExternalLoaderPath64(string path, ref IntPtr externalLoaderInfo);

        private static void SetExternalLoaderPathNative(string path, ref IntPtr externalLoaderInfo)
        {
            if (!Environment.Is64BitProcess)
            {
                SetExternalLoaderPath32(path, ref externalLoaderInfo);
            }
            else
            {
                SetExternalLoaderPath64(path, ref externalLoaderInfo);
            }
        }

        internal static void SetExternalLoaderPath(string path, ref IntPtr externalLoaderInfo)
        {
            try
            {
                SetExternalLoaderPathNative(path, ref externalLoaderInfo);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [GetExternalLoaders]

        [DllImport(ProgrammerDll32, EntryPoint = "GetExternalLoaders", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetExternalLoaders32(string path, ref IntPtr externalStorageNfo);

        [DllImport(ProgrammerDll64, EntryPoint = "GetExternalLoaders", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetExternalLoaders64(string path, ref IntPtr externalStorageNfo);

        private static int GetExternalLoadersNative(string path, ref IntPtr externalStorageNfo)
        {
            return !Environment.Is64BitProcess ? GetExternalLoaders32(path, ref externalStorageNfo) : GetExternalLoaders64(path, ref externalStorageNfo);
        }

        internal static int GetExternalLoaders(string path, ref IntPtr externalStorageNfo)
        {
            try
            {
                return GetExternalLoadersNative(path, ref externalStorageNfo);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [RemoveExternalLoader]

        [DllImport(ProgrammerDll32, EntryPoint = "RemoveExternalLoader", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void RemoveExternalLoader32(string path);

        [DllImport(ProgrammerDll64, EntryPoint = "RemoveExternalLoader", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void RemoveExternalLoader64(string path);

        private static void RemoveExternalLoaderNative(string path)
        {
            if (!Environment.Is64BitProcess)
            {
                RemoveExternalLoader32(path);
            }
            else
            {
                RemoveExternalLoader64(path);
            }
        }

        internal static void RemoveExternalLoader(string path)
        {
            try
            {
                RemoveExternalLoaderNative(path);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [DeleteLoaders]

        [DllImport(ProgrammerDll32, EntryPoint = "DeleteLoaders", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void DeleteLoaders32();

        [DllImport(ProgrammerDll64, EntryPoint = "DeleteLoaders", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void DeleteLoaders64();

        private static void DeleteLoadersNative()
        {
            if (!Environment.Is64BitProcess)
            {
                DeleteLoaders32();
            }
            else
            {
                DeleteLoaders64();
            }
        }

        internal static void DeleteLoaders()
        {
            try
            {
                DeleteLoadersNative();
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #endregion

        #region [STM32WB specific]

        #region [GetUID64]

        [DllImport(ProgrammerDll32, EntryPoint = "GetUID64", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetUID6432(out IntPtr data);

        [DllImport(ProgrammerDll64, EntryPoint = "GetUID64", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetUID6464(out IntPtr data);

        private static int GetUID64Native(out IntPtr data)
        {
            return !Environment.Is64BitProcess
                ? GetUID6432(out data)
                : GetUID6464(out data);
        }

        internal static int GetUID64(out IntPtr data)
        {
            try
            {
                return GetUID64Native(out data);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [FirmwareDelete]

        [DllImport(ProgrammerDll32, EntryPoint = "FirmwareDelete", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int FirmwareDelete32();

        [DllImport(ProgrammerDll64, EntryPoint = "FirmwareDelete", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int FirmwareDelete64();

        private static int FirmwareDeleteNative()
        {
            return !Environment.Is64BitProcess
                ? FirmwareDelete32()
                : FirmwareDelete64();
        }

        internal static int FirmwareDelete()
        {
            try
            {
                return FirmwareDeleteNative();
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [FirmwareUpgrade]

        [DllImport(ProgrammerDll32, EntryPoint = "FirmwareUpgrade", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int FirmwareUpgrade32(string filePath, uint address, uint firstInstall, uint startStack, uint verify);

        [DllImport(ProgrammerDll64, EntryPoint = "FirmwareUpgrade", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int FirmwareUpgrade64(string filePath, uint address, uint firstInstall, uint startStack, uint verify);

        private static int FirmwareUpgradeNative(string filePath, uint address, uint firstInstall, uint startStack, uint verify)
        {
            return !Environment.Is64BitProcess
                ? FirmwareUpgrade32(filePath, address, firstInstall, startStack, verify)
                : FirmwareUpgrade64(filePath, address, firstInstall, startStack, verify);
        }

        internal static int FirmwareUpgrade(string filePath, uint address, uint firstInstall, uint startStack, uint verify)
        {
            try
            {
                return FirmwareUpgradeNative(filePath, address, firstInstall, startStack, verify);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [StartWirelessStack]

        [DllImport(ProgrammerDll32, EntryPoint = "StartWirelessStack", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int StartWirelessStack32();

        [DllImport(ProgrammerDll64, EntryPoint = "StartWirelessStack", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int StartWirelessStack64();

        private static int StartWirelessStackNative()
        {
            return !Environment.Is64BitProcess
                ? StartWirelessStack32()
                : StartWirelessStack64();
        }

        internal static int StartWirelessStack()
        {
            try
            {
                return StartWirelessStackNative();
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [UpdateAuthKey]

        [DllImport(ProgrammerDll32, EntryPoint = "UpdateAuthKey", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int UpdateAuthKey32(string filePath);

        [DllImport(ProgrammerDll64, EntryPoint = "UpdateAuthKey", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int UpdateAuthKey64(string filePath);

        private static int UpdateAuthKeyNative(string filePath)
        {
            return !Environment.Is64BitProcess
                ? UpdateAuthKey32(filePath)
                : UpdateAuthKey64(filePath);
        }

        internal static int UpdateAuthKey(string filePath)
        {
            try
            {
                return UpdateAuthKeyNative(filePath);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [AuthKeyLock]

        [DllImport(ProgrammerDll32, EntryPoint = "AuthKeyLock", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int AuthKeyLock32();

        [DllImport(ProgrammerDll64, EntryPoint = "AuthKeyLock", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int AuthKeyLock64();

        private static int AuthKeyLockNative()
        {
            return !Environment.Is64BitProcess
                ? AuthKeyLock32()
                : AuthKeyLock64();
        }

        internal static int AuthKeyLock()
        {
            try
            {
                return AuthKeyLockNative();
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [WriteUserKey]

        [DllImport(ProgrammerDll32, EntryPoint = "WriteUserKey", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int WriteUserKey32(string filePath, byte keyType);

        [DllImport(ProgrammerDll64, EntryPoint = "WriteUserKey", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int WriteUserKey64(string filePath, byte keyType);

        private static int WriteUserKeyNative(string filePath, byte keyType)
        {
            return !Environment.Is64BitProcess
                ? WriteUserKey32(filePath, keyType)
                : WriteUserKey64(filePath, keyType);
        }

        internal static int WriteUserKey(string filePath, byte keyType)
        {
            try
            {
                return WriteUserKeyNative(filePath, keyType);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [AntiRollBack]

        [DllImport(ProgrammerDll32, EntryPoint = "AntiRollBack", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int AntiRollBack32();

        [DllImport(ProgrammerDll64, EntryPoint = "AntiRollBack", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int AntiRollBack64();

        private static int AntiRollBackNative()
        {
            return !Environment.Is64BitProcess
                ? AntiRollBack32()
                : AntiRollBack64();
        }

        internal static int AntiRollBack()
        {
            try
            {
                return AntiRollBackNative();
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #region [StartFus]

        [DllImport(ProgrammerDll32, EntryPoint = "StartFus", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int StartFus32();

        [DllImport(ProgrammerDll64, EntryPoint = "StartFus", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int StartFus64();

        private static int StartFusNative()
        {
            return !Environment.Is64BitProcess
                ? StartFus32()
                : StartFus64();
        }

        internal static int StartFus()
        {
            try
            {
                return StartFusNative();
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        #endregion

        #endregion

    }
}
