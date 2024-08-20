// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Native
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

        [DllImport(ProgrammerDll32, EntryPoint = "Reset", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int Reset32([MarshalAs(UnmanagedType.U4)] DebugResetMode rstMode);

        [DllImport(ProgrammerDll64, EntryPoint = "Reset", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "GetUsartList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetUsartList32(ref IntPtr usartList);

        [DllImport(ProgrammerDll64, EntryPoint = "GetUsartList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "ConnectUsartBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ConnectUsartBootloader32(UsartConnectParameters usartParameters);

        [DllImport(ProgrammerDll64, EntryPoint = "ConnectUsartBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "SendByteUart", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int SendByteUart32(int bytes);

        [DllImport(ProgrammerDll64, EntryPoint = "SendByteUart", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "GetDfuDeviceList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetDfuDeviceList32(ref IntPtr dfuList, int iPID, int iVID);

        [DllImport(ProgrammerDll64, EntryPoint = "GetDfuDeviceList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "ConnectDfuBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ConnectDfuBootloader32(string usbIndex);

        [DllImport(ProgrammerDll64, EntryPoint = "ConnectDfuBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "ConnectDfuBootloader2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ConnectDfuBootloader232(DfuConnectParameters dfuParameters);

        [DllImport(ProgrammerDll64, EntryPoint = "ConnectDfuBootloader2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "ConnectSpiBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ConnectSpiBootloader32(SpiConnectParameters spiParameters);

        [DllImport(ProgrammerDll64, EntryPoint = "ConnectSpiBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "ConnectCanBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ConnectCanBootloader32(CanConnectParameters canParameters);

        [DllImport(ProgrammerDll64, EntryPoint = "ConnectCanBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "ConnectI2cBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ConnectI2cBootloader32(I2CConnectParameters i2cParameters);

        [DllImport(ProgrammerDll64, EntryPoint = "ConnectI2cBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        #region [SetDisplayCallbacks]

        [DllImport(ProgrammerDll32, EntryPoint = "SetDisplayCallbacks", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void SetDisplayCallbacks32(DisplayCallBacks c);

        [DllImport(ProgrammerDll64, EntryPoint = "SetDisplayCallbacks", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void SetDisplayCallbacks64(DisplayCallBacks c);

        private static void SetDisplayCallbacksNative(DisplayCallBacks c)
        {
            if (!Environment.Is64BitProcess)
            {
                SetDisplayCallbacks32(c);
            }
            else
            {
                SetDisplayCallbacks64(c);
            }
        }

        internal static void SetDisplayCallbacks(DisplayCallBacks c)
        {
            try
            {
                SetDisplayCallbacksNative(c);
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

        #region [SetVerbosityLevel]

        [DllImport(ProgrammerDll32, EntryPoint = "SetVerbosityLevel", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void SetVerbosityLevel32(int level);

        [DllImport(ProgrammerDll64, EntryPoint = "SetVerbosityLevel", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void SetVerbosityLevel64(int level);

        private static void SetVerbosityLevelNative(int level)
        {
            if (!Environment.Is64BitProcess)
            {
                SetVerbosityLevel32(level);
            }
            else
            {
                SetVerbosityLevel64(level);
            }
        }

        internal static void SetVerbosityLevel(int level)
        {
            try
            {
                SetVerbosityLevelNative(level);
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

        #region [CheckDeviceConnection]

        [DllImport(ProgrammerDll32, EntryPoint = "CheckDeviceConnection", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern bool CheckDeviceConnection32();

        [DllImport(ProgrammerDll64, EntryPoint = "CheckDeviceConnection", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "GetDeviceGeneralInf", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern IntPtr GetDeviceGeneralInf32();

        [DllImport(ProgrammerDll64, EntryPoint = "GetDeviceGeneralInf", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "ReadMemory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ReadMemory32(uint address, ref IntPtr data, uint size);

        [DllImport(ProgrammerDll64, EntryPoint = "ReadMemory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "WriteMemory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int WriteMemory32(uint address, IntPtr data, uint size);

        [DllImport(ProgrammerDll64, EntryPoint = "WriteMemory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        #region [WriteMemoryAndVerify]

        [DllImport(ProgrammerDll32, EntryPoint = "WriteMemoryAndVerify", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int WriteMemoryAndVerify32(uint address, IntPtr data, uint size);

        [DllImport(ProgrammerDll64, EntryPoint = "WriteMemoryAndVerify", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int WriteMemoryAndVerify64(uint address, IntPtr data, uint size);

        private static int WriteMemoryAndVerifyNative(uint address, IntPtr data, uint size)
        {
            return !Environment.Is64BitProcess
                ? WriteMemoryAndVerify32(address, data, size)
                : WriteMemoryAndVerify64(address, data, size);
        }

        internal static int WriteMemoryAndVerify(uint address, IntPtr data, uint size)
        {
            try
            {
                return WriteMemoryAndVerifyNative(address, data, size);
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

        #region [EditSector]

        [DllImport(ProgrammerDll32, EntryPoint = "EditSector", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int EditSector32(uint address, IntPtr data, uint size);

        [DllImport(ProgrammerDll64, EntryPoint = "EditSector", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int EditSector64(uint address, IntPtr data, uint size);

        private static int EditSectorNative(uint address, IntPtr data, uint size)
        {
            return !Environment.Is64BitProcess
                ? EditSector32(address, data, size)
                : EditSector64(address, data, size);
        }

        internal static int EditSector(uint address, IntPtr data, uint size)
        {
            try
            {
                return EditSectorNative(address, data, size);
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

        [DllImport(ProgrammerDll32, EntryPoint = "DownloadFile", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int DownloadFile32([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, [MarshalAs(UnmanagedType.LPWStr)] string binPath);

        [DllImport(ProgrammerDll64, EntryPoint = "DownloadFile", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "Execute", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int Execute32(uint address);

        [DllImport(ProgrammerDll64, EntryPoint = "Execute", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "MassErase", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int MassErase32(string sFlashMemName);

        [DllImport(ProgrammerDll64, EntryPoint = "MassErase", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "SectorErase", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SectorErase32(uint[] sectors, uint sectorNbr, string sFlashMemName);

        [DllImport(ProgrammerDll64, EntryPoint = "SectorErase", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "ReadUnprotect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ReadUnprotect32();

        [DllImport(ProgrammerDll64, EntryPoint = "ReadUnprotect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        #region [TzenRegression]

        [DllImport(ProgrammerDll32, EntryPoint = "TzenRegression", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int TzenRegression32();

        [DllImport(ProgrammerDll64, EntryPoint = "TzenRegression", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int TzenRegression64();

        private static int TzenRegressionNative()
        {
            return !Environment.Is64BitProcess
                ? TzenRegression32()
                : TzenRegression64();
        }

        internal static int TzenRegression()
        {
            try
            {
                return TzenRegressionNative();
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

        [DllImport(ProgrammerDll32, EntryPoint = "GetTargetInterfaceType", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetTargetInterfaceType32();

        [DllImport(ProgrammerDll64, EntryPoint = "GetTargetInterfaceType", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "GetCancelPointer", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern IntPtr GetCancelPointer32();

        [DllImport(ProgrammerDll64, EntryPoint = "GetCancelPointer", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "FileOpen", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr FileOpen32([MarshalAs(UnmanagedType.LPWStr)] string filePath);

        [DllImport(ProgrammerDll64, EntryPoint = "FileOpen", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "FreeFileData", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void FreeFileData32(IntPtr data);

        [DllImport(ProgrammerDll64, EntryPoint = "FreeFileData", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void FreeFileData64(IntPtr data);

        private static void FreeFileDataNative(IntPtr data)
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

        internal static void FreeFileData(IntPtr data)
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

        #region [FreeLibraryMemory]

        [DllImport(ProgrammerDll32, EntryPoint = "FreeLibraryMemory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void FreeLibraryMemory32(IntPtr ptr);

        [DllImport(ProgrammerDll64, EntryPoint = "FreeLibraryMemory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void FreeLibraryMemory64(IntPtr ptr);

        private static void FreeLibraryMemoryNative(IntPtr ptr)
        {
            if (!Environment.Is64BitProcess)
            {
                FreeLibraryMemory32(ptr);
            }
            else
            {
                FreeLibraryMemory64(ptr);
            }
        }

        internal static void FreeLibraryMemory(IntPtr ptr)
        {
            try
            {
                FreeLibraryMemoryNative(ptr);
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

        [DllImport(ProgrammerDll32, EntryPoint = "Verify", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int Verify32(IntPtr fileData, uint address);

        [DllImport(ProgrammerDll64, EntryPoint = "Verify", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int Verify64(IntPtr fileData, uint address);

        private static int VerifyNative(IntPtr fileData, uint address)
        {
            return !Environment.Is64BitProcess
                ? Verify32(fileData, address)
                : Verify64(fileData, address);
        }

        internal static int Verify(IntPtr fileData, uint address)
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

        [DllImport(ProgrammerDll32, EntryPoint = "VerifyMemory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int VerifyMemory32(uint address, IntPtr data, uint size);

        [DllImport(ProgrammerDll64, EntryPoint = "VerifyMemory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        #region [VerifyMemoryBySegment]

        [DllImport(ProgrammerDll32, EntryPoint = "VerifyMemoryBySegment", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int VerifyMemoryBySegment32(uint address, IntPtr data, uint size);

        [DllImport(ProgrammerDll64, EntryPoint = "VerifyMemoryBySegment", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int VerifyMemoryBySegment64(uint address, IntPtr data, uint size);

        private static int VerifyMemoryBySegmentNative(uint address, IntPtr data, uint size)
        {
            return !Environment.Is64BitProcess
                ? VerifyMemoryBySegment32(address, data, size)
                : VerifyMemoryBySegment64(address, data, size);
        }

        internal static int VerifyMemoryBySegment(uint address, IntPtr data, uint size)
        {
            try
            {
                return VerifyMemoryBySegmentNative(address, data, size);
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

        [DllImport(ProgrammerDll32, EntryPoint = "SaveFileToFile", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SaveFileToFile32(IntPtr fileData, [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        [DllImport(ProgrammerDll64, EntryPoint = "SaveFileToFile", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SaveFileToFile64(IntPtr fileData, [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        private static int SaveFileToFileNative(IntPtr fileData, string sFileName)
        {
            return !Environment.Is64BitProcess
                ? SaveFileToFile32(fileData, sFileName)
                : SaveFileToFile64(fileData, sFileName);
        }

        internal static int SaveFileToFile(IntPtr fileData, string sFileName)
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

        [DllImport(ProgrammerDll32, EntryPoint = "SaveMemoryToFile", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SaveMemoryToFile32(int address, int size, [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        [DllImport(ProgrammerDll64, EntryPoint = "SaveMemoryToFile", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "Disconnect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int Disconnect32();

        [DllImport(ProgrammerDll64, EntryPoint = "Disconnect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "DeleteInterfaceList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void DeleteInterfaceList32();

        [DllImport(ProgrammerDll64, EntryPoint = "DeleteInterfaceList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "AutomaticMode", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void AutomaticMode32([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, int isMassErase, string obCommand, int run);

        [DllImport(ProgrammerDll64, EntryPoint = "AutomaticMode", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
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

        #region [SerialNumberingAutomaticMode]

        [DllImport(ProgrammerDll32, EntryPoint = "SerialNumberingAutomaticMode", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void SerialNumberingAutomaticMode32([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, int isMassErase, string obCommand, int run, int enableSerialNumbering, int serialAddress, int serialSize, string serialInitialData);

        [DllImport(ProgrammerDll64, EntryPoint = "SerialNumberingAutomaticMode", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void SerialNumberingAutomaticMode64([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, int isMassErase, string obCommand, int run, int enableSerialNumbering, int serialAddress, int serialSize, string serialInitialData);

        private static void SerialNumberingAutomaticModeNative(string filePath, uint address, uint skipErase, uint verify, int isMassErase, string obCommand, int run, int enableSerialNumbering, int serialAddress, int serialSize, string serialInitialData)
        {
            if (!Environment.Is64BitProcess)
            {
                SerialNumberingAutomaticMode32(filePath, address, skipErase, verify, isMassErase, obCommand, run, enableSerialNumbering, serialAddress, serialSize, serialInitialData);
            }
            else
            {
                SerialNumberingAutomaticMode64(filePath, address, skipErase, verify, isMassErase, obCommand, run, enableSerialNumbering, serialAddress, serialSize, serialInitialData);
            }
        }

        internal static void SerialNumberingAutomaticMode(string filePath, uint address, uint skipErase, uint verify, int isMassErase, string obCommand, int run, int enableSerialNumbering, int serialAddress, int serialSize, string serialInitialData)
        {
            try
            {
                SerialNumberingAutomaticModeNative(filePath, address, skipErase, verify, isMassErase, obCommand, run, enableSerialNumbering, serialAddress, serialSize, serialInitialData);
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

        [DllImport(ProgrammerDll32, EntryPoint = "GetStorageStructure", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetStorageStructure32(ref IntPtr deviceStorageStruct);

        [DllImport(ProgrammerDll64, EntryPoint = "GetStorageStructure", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "SendOptionBytesCmd", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SendOptionBytesCmd32(string command);

        [DllImport(ProgrammerDll64, EntryPoint = "SendOptionBytesCmd", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "InitOptionBytesInterface", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern IntPtr InitOptionBytesInterface32();

        [DllImport(ProgrammerDll64, EntryPoint = "InitOptionBytesInterface", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "FastRomInitOptionBytesInterface", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern IntPtr FastRomInitOptionBytesInterface32(ushort deviceId);

        [DllImport(ProgrammerDll64, EntryPoint = "FastRomInitOptionBytesInterface", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "ObDisplay", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ObDisplay32();

        [DllImport(ProgrammerDll64, EntryPoint = "ObDisplay", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "SetLoadersPath", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void SetLoadersPath32(string path);

        [DllImport(ProgrammerDll64, EntryPoint = "SetLoadersPath", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "SetExternalLoaderPath", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void SetExternalLoaderPath32(string path, ref IntPtr externalLoaderInfo);

        [DllImport(ProgrammerDll64, EntryPoint = "SetExternalLoaderPath", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
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

        #region [SetExternalLoaderOBL]

        [DllImport(ProgrammerDll32, EntryPoint = "SetExternalLoaderOBL", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void SetExternalLoaderOBL32(string path, ref IntPtr externalLoaderInfo);

        [DllImport(ProgrammerDll64, EntryPoint = "SetExternalLoaderOBL", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void SetExternalLoaderOBL64(string path, ref IntPtr externalLoaderInfo);

        private static void SetExternalLoaderOBLNative(string path, ref IntPtr externalLoaderInfo)
        {
            if (!Environment.Is64BitProcess)
            {
                SetExternalLoaderOBL32(path, ref externalLoaderInfo);
            }
            else
            {
                SetExternalLoaderOBL64(path, ref externalLoaderInfo);
            }
        }

        internal static void SetExternalLoaderOBL(string path, ref IntPtr externalLoaderInfo)
        {
            try
            {
                SetExternalLoaderOBLNative(path, ref externalLoaderInfo);
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

        [DllImport(ProgrammerDll32, EntryPoint = "RemoveExternalLoader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void RemoveExternalLoader32(string path);

        [DllImport(ProgrammerDll64, EntryPoint = "RemoveExternalLoader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "DeleteLoaders", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void DeleteLoaders32();

        [DllImport(ProgrammerDll64, EntryPoint = "DeleteLoaders", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "GetUID64", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetUID6432(ref IntPtr data);

        [DllImport(ProgrammerDll64, EntryPoint = "GetUID64", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetUID6464(ref IntPtr data);

        private static int GetUID64Native(ref IntPtr data)
        {
            return !Environment.Is64BitProcess
                ? GetUID6432(ref data)
                : GetUID6464(ref data);
        }

        internal static int GetUID64(ref IntPtr data)
        {
            try
            {
                return GetUID64Native(ref data);
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

        [DllImport(ProgrammerDll32, EntryPoint = "FirmwareDelete", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int FirmwareDelete32();

        [DllImport(ProgrammerDll64, EntryPoint = "FirmwareDelete", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "FirmwareUpgrade", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int FirmwareUpgrade32([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint firstInstall, uint startStack, uint verify);

        [DllImport(ProgrammerDll64, EntryPoint = "FirmwareUpgrade", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int FirmwareUpgrade64([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint firstInstall, uint startStack, uint verify);

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

        [DllImport(ProgrammerDll32, EntryPoint = "StartWirelessStack", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int StartWirelessStack32();

        [DllImport(ProgrammerDll64, EntryPoint = "StartWirelessStack", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "UpdateAuthKey", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int UpdateAuthKey32([MarshalAs(UnmanagedType.LPWStr)] string filePath);

        [DllImport(ProgrammerDll64, EntryPoint = "UpdateAuthKey", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int UpdateAuthKey64([MarshalAs(UnmanagedType.LPWStr)] string filePath);

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

        [DllImport(ProgrammerDll32, EntryPoint = "AuthKeyLock", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int AuthKeyLock32();

        [DllImport(ProgrammerDll64, EntryPoint = "AuthKeyLock", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "WriteUserKey", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int WriteUserKey32([MarshalAs(UnmanagedType.LPWStr)] string filePath, byte keyType);

        [DllImport(ProgrammerDll64, EntryPoint = "WriteUserKey", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int WriteUserKey64([MarshalAs(UnmanagedType.LPWStr)] string filePath, byte keyType);

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

        [DllImport(ProgrammerDll32, EntryPoint = "AntiRollBack", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int AntiRollBack32();

        [DllImport(ProgrammerDll64, EntryPoint = "AntiRollBack", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        [DllImport(ProgrammerDll32, EntryPoint = "StartFus", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int StartFus32();

        [DllImport(ProgrammerDll64, EntryPoint = "StartFus", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
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

        #region [UnlockChip]

        [DllImport(ProgrammerDll32, EntryPoint = "UnlockChip", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int UnlockChip32();

        [DllImport(ProgrammerDll64, EntryPoint = "UnlockChip", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int UnlockChip64();

        private static int UnlockChipNative()
        {
            return !Environment.Is64BitProcess
                ? UnlockChip32()
                : UnlockChip64();
        }

        internal static int UnlockChip()
        {
            try
            {
                return UnlockChipNative();
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

        #region [STM32MP specific functions]

        #region [ProgramSsp]

        [DllImport(ProgrammerDll32, EntryPoint = "ProgramSsp", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ProgramSsp32([MarshalAs(UnmanagedType.LPWStr)] string sspFile, [MarshalAs(UnmanagedType.LPWStr)] string licenseFile, [MarshalAs(UnmanagedType.LPWStr)] string tfaFile, int hsmSlotId);

        [DllImport(ProgrammerDll64, EntryPoint = "ProgramSsp", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ProgramSsp64([MarshalAs(UnmanagedType.LPWStr)] string sspFile, [MarshalAs(UnmanagedType.LPWStr)] string licenseFile, [MarshalAs(UnmanagedType.LPWStr)] string tfaFile, int hsmSlotId);

        private static int ProgramSspNative(string sspFile, string licenseFile, string tfaFile, int hsmSlotId)
        {
            return !Environment.Is64BitProcess
                ? ProgramSsp32(sspFile, licenseFile, tfaFile, hsmSlotId)
                : ProgramSsp64(sspFile, licenseFile, tfaFile, hsmSlotId);
        }

        internal static int ProgramSsp(string sspFile, string licenseFile, string tfaFile, int hsmSlotId)
        {
            try
            {
                return ProgramSspNative(sspFile, licenseFile, tfaFile, hsmSlotId);
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

        #region [STM32 HSM specific functions]

        #region [GetHsmFirmwareID]

        [DllImport(ProgrammerDll32, EntryPoint = "GetHsmFirmwareID", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern string GetHsmFirmwareID32(int hsmSlotId);

        [DllImport(ProgrammerDll64, EntryPoint = "GetHsmFirmwareID", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern string GetHsmFirmwareID64(int hsmSlotId);

        private static string GetHsmFirmwareIDNative(int hsmSlotId)
        {
            return !Environment.Is64BitProcess
                ? GetHsmFirmwareID32(hsmSlotId)
                : GetHsmFirmwareID64(hsmSlotId);
        }

        internal static string GetHsmFirmwareID(int hsmSlotId)
        {
            try
            {
                return GetHsmFirmwareIDNative(hsmSlotId);
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

        #region [GetHsmCounter]

        [DllImport(ProgrammerDll32, EntryPoint = "GetHsmCounter", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern ulong GetHsmCounter32(int hsmSlotId);

        [DllImport(ProgrammerDll64, EntryPoint = "GetHsmCounter", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern ulong GetHsmCounter64(int hsmSlotId);

        private static ulong GetHsmCounterNative(int hsmSlotId)
        {
            return !Environment.Is64BitProcess
                ? GetHsmCounter32(hsmSlotId)
                : GetHsmCounter64(hsmSlotId);
        }

        internal static ulong GetHsmCounter(int hsmSlotId)
        {
            try
            {
                return GetHsmCounterNative(hsmSlotId);
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

        #region [GetHsmState]

        [DllImport(ProgrammerDll32, EntryPoint = "GetHsmState", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern string GetHsmState32(int hsmSlotId);

        [DllImport(ProgrammerDll64, EntryPoint = "GetHsmState", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern string GetHsmState64(int hsmSlotId);

        private static string GetHsmStateNative(int hsmSlotId)
        {
            return !Environment.Is64BitProcess
                ? GetHsmState32(hsmSlotId)
                : GetHsmState64(hsmSlotId);
        }

        internal static string GetHsmState(int hsmSlotId)
        {
            try
            {
                return GetHsmStateNative(hsmSlotId);
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

        #region [GetHsmVersion]

        [DllImport(ProgrammerDll32, EntryPoint = "GetHsmVersion", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern string GetHsmVersion32(int hsmSlotId);

        [DllImport(ProgrammerDll64, EntryPoint = "GetHsmVersion", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern string GetHsmVersion64(int hsmSlotId);

        private static string GetHsmVersionNative(int hsmSlotId)
        {
            return !Environment.Is64BitProcess
                ? GetHsmVersion32(hsmSlotId)
                : GetHsmVersion64(hsmSlotId);
        }

        internal static string GetHsmVersion(int hsmSlotId)
        {
            try
            {
                return GetHsmVersionNative(hsmSlotId);
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

        #region [GetHsmType]

        [DllImport(ProgrammerDll32, EntryPoint = "GetHsmType", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern string GetHsmType32(int hsmSlotId);

        [DllImport(ProgrammerDll64, EntryPoint = "GetHsmType", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern string GetHsmType64(int hsmSlotId);

        private static string GetHsmTypeNative(int hsmSlotId)
        {
            return !Environment.Is64BitProcess
                ? GetHsmType32(hsmSlotId)
                : GetHsmType64(hsmSlotId);
        }

        internal static string GetHsmType(int hsmSlotId)
        {
            try
            {
                return GetHsmTypeNative(hsmSlotId);
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

        #region [GetHsmLicense]

        [DllImport(ProgrammerDll32, EntryPoint = "GetHsmLicense", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetHsmLicense32(int hsmSlotId, [MarshalAs(UnmanagedType.LPWStr)] string outLicensePath);

        [DllImport(ProgrammerDll64, EntryPoint = "GetHsmLicense", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetHsmLicense64(int hsmSlotId, [MarshalAs(UnmanagedType.LPWStr)] string outLicensePath);

        private static int GetHsmLicenseNative(int hsmSlotId, string outLicensePath)
        {
            return !Environment.Is64BitProcess
                ? GetHsmLicense32(hsmSlotId, outLicensePath)
                : GetHsmLicense64(hsmSlotId, outLicensePath);
        }

        internal static int GetHsmLicense(int hsmSlotId, string outLicensePath)
        {
            try
            {
                return GetHsmLicenseNative(hsmSlotId, outLicensePath);
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
