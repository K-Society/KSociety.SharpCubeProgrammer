// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Native
{
    using System;
    using System.Runtime.InteropServices;
    using Enum;
    using Struct;

    internal static class ProgrammerApi
    {
        //private const string ProgrammerDll32 = @".\dll\x86\Programmer.dll";
        //private const string ProgrammerDll64 = @".\dll\x64\Programmer.dll";

        private const string ProgrammerDll = @"Programmer.dll";
        //private const string ProgrammerDll64 = @"Programmer.dll";

        #region [STLINK]

        #region [TryConnectStLink]

        [DllImport(ProgrammerDll, EntryPoint = "TryConnectStLink", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int TryConnectStLink32(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode);

        //[DllImport(ProgrammerDll64, EntryPoint = "TryConnectStLink", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int TryConnectStLink64(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode);

        private static int TryConnectStLinkNative(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode)
        {
            return TryConnectStLink32(stLinkProbeIndex, shared, debugConnectMode);
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

        [DllImport(ProgrammerDll, EntryPoint = "GetStLinkList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetStLinkList32(ref IntPtr stLinkList, int shared);

        //[DllImport(ProgrammerDll64, EntryPoint = "GetStLinkList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int GetStLinkList64(ref IntPtr stLinkList, int shared);

        private static int GetStLinkListNative(ref IntPtr stLinkList, int shared)
        {
            return GetStLinkList32(ref stLinkList, shared);
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

        [DllImport(ProgrammerDll, EntryPoint = "GetStLinkEnumerationList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetStLinkEnumerationList32(ref IntPtr stLinkList, int shared);

        //[DllImport(ProgrammerDll64, EntryPoint = "GetStLinkEnumerationList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int GetStLinkEnumerationList64(ref IntPtr stLinkList, int shared);

        private static int GetStLinkEnumerationListNative(ref IntPtr stLinkList, int shared)
        {
            return GetStLinkEnumerationList32(ref stLinkList, shared);
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

        [DllImport(ProgrammerDll, EntryPoint = "ConnectStLink", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ConnectStLink32(DebugConnectParameters debugParameters);

        //[DllImport(ProgrammerDll64, EntryPoint = "ConnectStLink", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int ConnectStLink64(DebugConnectParameters debugParameters);

        private static int ConnectStLinkNative(DebugConnectParameters debugParameters)
        {
            return ConnectStLink32(debugParameters);
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

        [DllImport(ProgrammerDll, EntryPoint = "Reset", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int Reset32([MarshalAs(UnmanagedType.U4)] DebugResetMode rstMode);

        //[DllImport(ProgrammerDll64, EntryPoint = "Reset", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int Reset64([MarshalAs(UnmanagedType.U4)] DebugResetMode rstMode);

        private static int ResetNative(DebugResetMode rstMode)
        {
            return Reset32(rstMode);
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

        [DllImport(ProgrammerDll, EntryPoint = "GetUsartList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetUsartList32(ref IntPtr usartList);

        //[DllImport(ProgrammerDll64, EntryPoint = "GetUsartList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int GetUsartList64(ref IntPtr usartList);

        private static int GetUsartListNative(ref IntPtr usartList)
        {
            return GetUsartList32(ref usartList);
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

        [DllImport(ProgrammerDll, EntryPoint = "ConnectUsartBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ConnectUsartBootloader32(UsartConnectParameters usartParameters);

        //[DllImport(ProgrammerDll64, EntryPoint = "ConnectUsartBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int ConnectUsartBootloader64(UsartConnectParameters usartParameters);

        private static int ConnectUsartBootloaderNative(UsartConnectParameters usartParameters)
        {
            return ConnectUsartBootloader32(usartParameters);
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

        [DllImport(ProgrammerDll, EntryPoint = "SendByteUart", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int SendByteUart32(int bytes);

        //[DllImport(ProgrammerDll64, EntryPoint = "SendByteUart", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int SendByteUart64(int bytes);

        private static int SendByteUartNative(int bytes)
        {
            return SendByteUart32(bytes);
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

        [DllImport(ProgrammerDll, EntryPoint = "GetDfuDeviceList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetDfuDeviceList32(ref IntPtr dfuList, int iPID, int iVID);

        //[DllImport(ProgrammerDll64, EntryPoint = "GetDfuDeviceList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int GetDfuDeviceList64(ref IntPtr dfuList, int iPID, int iVID);

        private static int GetDfuDeviceListNative(ref IntPtr dfuList, int iPID, int iVID)
        {
            return GetDfuDeviceList32(ref dfuList, iPID, iVID);
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

        [DllImport(ProgrammerDll, EntryPoint = "ConnectDfuBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ConnectDfuBootloader32(IntPtr usbIndex);

        //[DllImport(ProgrammerDll64, EntryPoint = "ConnectDfuBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int ConnectDfuBootloader64(IntPtr usbIndex);

        private static int ConnectDfuBootloaderNative(IntPtr usbIndex)
        {
            return ConnectDfuBootloader32(usbIndex);
        }

        internal static int ConnectDfuBootloader(string usbIndex)
        {
            var usbIndexPtr = IntPtr.Zero;

            try
            {
                usbIndexPtr = Marshal.StringToHGlobalAnsi(usbIndex);
                return ConnectDfuBootloaderNative(usbIndexPtr);
            }
            catch (OutOfMemoryException ex)
            {
                throw new Exception("K-Society CubeProgrammer out of memory exception.", ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new Exception("K-Society CubeProgrammer argument out of range exception.", ex);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
            finally
            {
                Marshal.FreeHGlobal(usbIndexPtr);
            }
        }

        #endregion

        #region [ConnectDfuBootloader2]

        [DllImport(ProgrammerDll, EntryPoint = "ConnectDfuBootloader2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ConnectDfuBootloader232(DfuConnectParameters dfuParameters);

        //[DllImport(ProgrammerDll64, EntryPoint = "ConnectDfuBootloader2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int ConnectDfuBootloader264(DfuConnectParameters dfuParameters);

        private static int ConnectDfuBootloader2Native(DfuConnectParameters dfuParameters)
        {
            return ConnectDfuBootloader232(dfuParameters);
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

        [DllImport(ProgrammerDll, EntryPoint = "ConnectSpiBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ConnectSpiBootloader32(SpiConnectParameters spiParameters);

        //[DllImport(ProgrammerDll64, EntryPoint = "ConnectSpiBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int ConnectSpiBootloader64(SpiConnectParameters spiParameters);

        private static int ConnectSpiBootloaderNative(SpiConnectParameters spiParameters)
        {
            return ConnectSpiBootloader32(spiParameters);
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

        [DllImport(ProgrammerDll, EntryPoint = "ConnectCanBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ConnectCanBootloader32(CanConnectParameters canParameters);

        //[DllImport(ProgrammerDll64, EntryPoint = "ConnectCanBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int ConnectCanBootloader64(CanConnectParameters canParameters);

        private static int ConnectCanBootloaderNative(CanConnectParameters canParameters)
        {
            return ConnectCanBootloader32(canParameters);
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

        [DllImport(ProgrammerDll, EntryPoint = "ConnectI2cBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ConnectI2cBootloader32(I2CConnectParameters i2cParameters);

        //[DllImport(ProgrammerDll64, EntryPoint = "ConnectI2cBootloader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int ConnectI2cBootloader64(I2CConnectParameters i2cParameters);

        private static int ConnectI2cBootloaderNative(I2CConnectParameters i2cParameters)
        {
            return ConnectI2cBootloader32(i2cParameters);
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

        [DllImport(ProgrammerDll, EntryPoint = "SetDisplayCallbacks", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void SetDisplayCallbacks32(DisplayCallBacks c);

        //[DllImport(ProgrammerDll64, EntryPoint = "SetDisplayCallbacks", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern void SetDisplayCallbacks64(DisplayCallBacks c);

        private static void SetDisplayCallbacksNative(DisplayCallBacks c)
        {
            SetDisplayCallbacks32(c);
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

        [DllImport(ProgrammerDll, EntryPoint = "SetVerbosityLevel", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void SetVerbosityLevel32(int level);

        //[DllImport(ProgrammerDll64, EntryPoint = "SetVerbosityLevel", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern void SetVerbosityLevel64(int level);

        private static void SetVerbosityLevelNative(int level)
        {
            SetVerbosityLevel32(level);
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

        [DllImport(ProgrammerDll, EntryPoint = "CheckDeviceConnection", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern bool CheckDeviceConnection32();

        //[DllImport(ProgrammerDll64, EntryPoint = "CheckDeviceConnection", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern bool CheckDeviceConnection64();

        private static bool CheckDeviceConnectionNative()
        {
            return CheckDeviceConnection32();
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

        [DllImport(ProgrammerDll, EntryPoint = "GetDeviceGeneralInf", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern IntPtr GetDeviceGeneralInf32();

        //[DllImport(ProgrammerDll64, EntryPoint = "GetDeviceGeneralInf", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern IntPtr GetDeviceGeneralInf64();

        private static IntPtr GetDeviceGeneralInfNative()
        {
            return GetDeviceGeneralInf32();
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

        [DllImport(ProgrammerDll, EntryPoint = "ReadMemory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ReadMemory32(uint address, ref IntPtr data, uint size);

        //[DllImport(ProgrammerDll64, EntryPoint = "ReadMemory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int ReadMemory64(uint address, ref IntPtr data, uint size);

        private static int ReadMemoryNative(uint address, ref IntPtr data, uint size)
        {
            return ReadMemory32(address, ref data, size);
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

        [DllImport(ProgrammerDll, EntryPoint = "WriteMemory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int WriteMemory32(uint address, IntPtr data, uint size);

        //[DllImport(ProgrammerDll64, EntryPoint = "WriteMemory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int WriteMemory64(uint address, IntPtr data, uint size);

        private static int WriteMemoryNative(uint address, IntPtr data, uint size)
        {
            return WriteMemory32(address, data, size);
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

        #region [WriteMemoryAutoFill]

        [DllImport(ProgrammerDll, EntryPoint = "WriteMemoryAutoFill", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int WriteMemoryAutoFill32(uint address, IntPtr data, uint size);

        //[DllImport(ProgrammerDll64, EntryPoint = "WriteMemoryAutoFill", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int WriteMemoryAutoFill64(uint address, IntPtr data, uint size);

        private static int WriteMemoryAutoFillNative(uint address, IntPtr data, uint size)
        {
            return WriteMemoryAutoFill32(address, data, size);
        }

        internal static int WriteMemoryAutoFill(uint address, IntPtr data, uint size)
        {
            try
            {
                return WriteMemoryAutoFillNative(address, data, size);
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

        [DllImport(ProgrammerDll, EntryPoint = "WriteMemoryAndVerify", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int WriteMemoryAndVerify32(uint address, IntPtr data, uint size);

        //[DllImport(ProgrammerDll64, EntryPoint = "WriteMemoryAndVerify", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int WriteMemoryAndVerify64(uint address, IntPtr data, uint size);

        private static int WriteMemoryAndVerifyNative(uint address, IntPtr data, uint size)
        {
            return WriteMemoryAndVerify32(address, data, size);
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

        [DllImport(ProgrammerDll, EntryPoint = "EditSector", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int EditSector32(uint address, IntPtr data, uint size);

        //[DllImport(ProgrammerDll64, EntryPoint = "EditSector", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int EditSector64(uint address, IntPtr data, uint size);

        private static int EditSectorNative(uint address, IntPtr data, uint size)
        {
            return EditSector32(address, data, size);
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

        [DllImport(ProgrammerDll, EntryPoint = "DownloadFile", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int DownloadFile32([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, [MarshalAs(UnmanagedType.LPWStr)] string binPath);

        //[DllImport(ProgrammerDll64, EntryPoint = "DownloadFile", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int DownloadFile64([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, [MarshalAs(UnmanagedType.LPWStr)] string binPath);

        private static int DownloadFileNative(string filePath, uint address, uint skipErase, uint verify, string binPath)
        {
            return DownloadFile32(filePath, address, skipErase, verify, binPath);
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

        [DllImport(ProgrammerDll, EntryPoint = "Execute", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int Execute32(uint address);

        //[DllImport(ProgrammerDll64, EntryPoint = "Execute", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int Execute64(uint address);

        private static int ExecuteNative(uint address)
        {
            return Execute32(address);
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

        [DllImport(ProgrammerDll, EntryPoint = "MassErase", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int MassErase32(IntPtr sFlashMemName);

        //[DllImport(ProgrammerDll64, EntryPoint = "MassErase", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int MassErase64(IntPtr sFlashMemName);

        private static int MassEraseNative(IntPtr sFlashMemName)
        {
            return MassErase32(sFlashMemName);
        }

        internal static int MassErase(string sFlashMemName)
        {
            var sFlashMemNamePtr = IntPtr.Zero;

            try
            {
                sFlashMemNamePtr = Marshal.StringToHGlobalAnsi(sFlashMemName);
                return MassEraseNative(sFlashMemNamePtr);
            }
            catch (OutOfMemoryException ex)
            {
                throw new Exception("K-Society CubeProgrammer out of memory exception.", ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new Exception("K-Society CubeProgrammer argument out of range exception.", ex);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
            finally
            {
                Marshal.FreeHGlobal(sFlashMemNamePtr);
            }
        }

        #endregion

        #region [SectorErase]

        [DllImport(ProgrammerDll, EntryPoint = "SectorErase", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SectorErase32(uint[] sectors, uint sectorNbr, IntPtr sFlashMemName);

        //[DllImport(ProgrammerDll64, EntryPoint = "SectorErase", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int SectorErase64(uint[] sectors, uint sectorNbr, IntPtr sFlashMemName);

        private static int SectorEraseNative(uint[] sectors, uint sectorNbr, IntPtr sFlashMemName)
        {
            return SectorErase32(sectors, sectorNbr, sFlashMemName);
        }

        internal static int SectorErase(uint[] sectors, uint sectorNbr, string sFlashMemName)
        {
            var sFlashMemNamePtr = IntPtr.Zero;

            try
            {
                sFlashMemNamePtr = Marshal.StringToHGlobalAnsi(sFlashMemName);
                return SectorEraseNative(sectors, sectorNbr, sFlashMemNamePtr);
            }
            catch (OutOfMemoryException ex)
            {
                throw new Exception("K-Society CubeProgrammer out of memory exception.", ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new Exception("K-Society CubeProgrammer argument out of range exception.", ex);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
            finally
            {
                Marshal.FreeHGlobal(sFlashMemNamePtr);
            }
        }

        #endregion

        #region [ReadUnprotect]

        [DllImport(ProgrammerDll, EntryPoint = "ReadUnprotect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ReadUnprotect32();

        //[DllImport(ProgrammerDll64, EntryPoint = "ReadUnprotect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int ReadUnprotect64();

        private static int ReadUnprotectNative()
        {
            return ReadUnprotect32();
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

        [DllImport(ProgrammerDll, EntryPoint = "TzenRegression", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int TzenRegression32();

        //[DllImport(ProgrammerDll64, EntryPoint = "TzenRegression", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int TzenRegression64();

        private static int TzenRegressionNative()
        {
            return TzenRegression32();
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

        [DllImport(ProgrammerDll, EntryPoint = "GetTargetInterfaceType", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetTargetInterfaceType32();

        //[DllImport(ProgrammerDll64, EntryPoint = "GetTargetInterfaceType", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int GetTargetInterfaceType64();

        private static int GetTargetInterfaceTypeNative()
        {
            return GetTargetInterfaceType32();
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

        [DllImport(ProgrammerDll, EntryPoint = "GetCancelPointer", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetCancelPointer32();

        //[DllImport(ProgrammerDll64, EntryPoint = "GetCancelPointer", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int GetCancelPointer64();

        private static int GetCancelPointerNative()
        {
            return GetCancelPointer32();
        }

        internal static int GetCancelPointer()
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

        [DllImport(ProgrammerDll, EntryPoint = "FileOpen", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr FileOpen32([MarshalAs(UnmanagedType.LPWStr)] string filePath);

        //[DllImport(ProgrammerDll64, EntryPoint = "FileOpen", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern IntPtr FileOpen64([MarshalAs(UnmanagedType.LPWStr)] string filePath);

        private static IntPtr FileOpenNative(string filePath)
        {
            return FileOpen32(filePath);
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

        [DllImport(ProgrammerDll, EntryPoint = "FreeFileData", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void FreeFileData32(IntPtr data);

        //[DllImport(ProgrammerDll64, EntryPoint = "FreeFileData", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern void FreeFileData64(IntPtr data);

        private static void FreeFileDataNative(IntPtr data)
        {
            FreeFileData32(data);
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

        [DllImport(ProgrammerDll, EntryPoint = "FreeLibraryMemory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void FreeLibraryMemory32(IntPtr ptr);

        //[DllImport(ProgrammerDll64, EntryPoint = "FreeLibraryMemory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern void FreeLibraryMemory64(IntPtr ptr);

        private static void FreeLibraryMemoryNative(IntPtr ptr)
        {
            FreeLibraryMemory32(ptr);
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

        [DllImport(ProgrammerDll, EntryPoint = "Verify", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int Verify32(IntPtr fileData, uint address);

        //[DllImport(ProgrammerDll64, EntryPoint = "Verify", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int Verify64(IntPtr fileData, uint address);

        private static int VerifyNative(IntPtr fileData, uint address)
        {
            return Verify32(fileData, address);
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

        [DllImport(ProgrammerDll, EntryPoint = "VerifyMemory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int VerifyMemory32(uint address, IntPtr data, uint size);

        //[DllImport(ProgrammerDll64, EntryPoint = "VerifyMemory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int VerifyMemory64(uint address, IntPtr data, uint size);

        private static int VerifyMemoryNative(uint address, IntPtr data, uint size)
        {
            return VerifyMemory32(address, data, size);
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

        [DllImport(ProgrammerDll, EntryPoint = "VerifyMemoryBySegment", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int VerifyMemoryBySegment32(uint address, IntPtr data, uint size);

        //[DllImport(ProgrammerDll64, EntryPoint = "VerifyMemoryBySegment", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int VerifyMemoryBySegment64(uint address, IntPtr data, uint size);

        private static int VerifyMemoryBySegmentNative(uint address, IntPtr data, uint size)
        {
            return VerifyMemoryBySegment32(address, data, size);
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

        [DllImport(ProgrammerDll, EntryPoint = "SaveFileToFile", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SaveFileToFile32(IntPtr fileData, [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        //[DllImport(ProgrammerDll64, EntryPoint = "SaveFileToFile", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int SaveFileToFile64(IntPtr fileData, [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        private static int SaveFileToFileNative(IntPtr fileData, string sFileName)
        {
            return SaveFileToFile32(fileData, sFileName);
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

        [DllImport(ProgrammerDll, EntryPoint = "SaveMemoryToFile", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SaveMemoryToFile32(int address, int size, [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        //[DllImport(ProgrammerDll64, EntryPoint = "SaveMemoryToFile", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int SaveMemoryToFile64(int address, int size, [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        private static int SaveMemoryToFileNative(int address, int size, string sFileName)
        {
            return SaveMemoryToFile32(address, size, sFileName);
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

        [DllImport(ProgrammerDll, EntryPoint = "Disconnect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int Disconnect32();

        //[DllImport(ProgrammerDll64, EntryPoint = "Disconnect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int Disconnect64();

        private static int DisconnectNative()
        {
            return Disconnect32();
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

        [DllImport(ProgrammerDll, EntryPoint = "DeleteInterfaceList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void DeleteInterfaceList32();

        //[DllImport(ProgrammerDll64, EntryPoint = "DeleteInterfaceList", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern void DeleteInterfaceList64();

        private static void DeleteInterfaceListNative()
        {
            DeleteInterfaceList32();
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

        [DllImport(ProgrammerDll, EntryPoint = "AutomaticMode", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void AutomaticMode32([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, int isMassErase, IntPtr obCommand, int run);

        //[DllImport(ProgrammerDll64, EntryPoint = "AutomaticMode", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern void AutomaticMode64([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, int isMassErase, IntPtr obCommand, int run);

        private static void AutomaticModeNative(string filePath, uint address, uint skipErase, uint verify, int isMassErase, IntPtr obCommand, int run)
        {
            AutomaticMode32(filePath, address, skipErase, verify, isMassErase, obCommand, run);
        }

        internal static void AutomaticMode(string filePath, uint address, uint skipErase, uint verify, int isMassErase, string obCommand, int run)
        {
            var obCommandPtr = IntPtr.Zero;
            try
            {
                obCommandPtr = Marshal.StringToHGlobalAnsi(obCommand);
                AutomaticModeNative(filePath, address, skipErase, verify, isMassErase, obCommandPtr, run);
            }
            catch (OutOfMemoryException ex)
            {
                throw new Exception("K-Society CubeProgrammer out of memory exception.", ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new Exception("K-Society CubeProgrammer argument out of range exception.", ex);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
            finally
            {
                Marshal.FreeHGlobal(obCommandPtr);
            }
        }

        #endregion

        #region [SerialNumberingAutomaticMode]

        [DllImport(ProgrammerDll, EntryPoint = "SerialNumberingAutomaticMode", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void SerialNumberingAutomaticMode32([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, int isMassErase, IntPtr obCommand, int run, int enableSerialNumbering, int serialAddress, int serialSize, string serialInitialData);

        //[DllImport(ProgrammerDll64, EntryPoint = "SerialNumberingAutomaticMode", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern void SerialNumberingAutomaticMode64([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, int isMassErase, IntPtr obCommand, int run, int enableSerialNumbering, int serialAddress, int serialSize, string serialInitialData);

        private static void SerialNumberingAutomaticModeNative(string filePath, uint address, uint skipErase, uint verify, int isMassErase, IntPtr obCommand, int run, int enableSerialNumbering, int serialAddress, int serialSize, string serialInitialData)
        {
            SerialNumberingAutomaticMode32(filePath, address, skipErase, verify, isMassErase, obCommand, run, enableSerialNumbering, serialAddress, serialSize, serialInitialData);
        }

        internal static void SerialNumberingAutomaticMode(string filePath, uint address, uint skipErase, uint verify, int isMassErase, string obCommand, int run, int enableSerialNumbering, int serialAddress, int serialSize, string serialInitialData)
        {
            var obCommandPtr = IntPtr.Zero;

            try
            {
                obCommandPtr = Marshal.StringToHGlobalAnsi(obCommand);
                SerialNumberingAutomaticModeNative(filePath, address, skipErase, verify, isMassErase, obCommandPtr, run, enableSerialNumbering, serialAddress, serialSize, serialInitialData);
            }
            catch (OutOfMemoryException ex)
            {
                throw new Exception("K-Society CubeProgrammer out of memory exception.", ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new Exception("K-Society CubeProgrammer argument out of range exception.", ex);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
            finally
            {
                Marshal.FreeHGlobal(obCommandPtr);
            }
        }

        #endregion

        #region [GetStorageStructure]

        [DllImport(ProgrammerDll, EntryPoint = "GetStorageStructure", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetStorageStructure32(ref IntPtr deviceStorageStruct);

        //[DllImport(ProgrammerDll64, EntryPoint = "GetStorageStructure", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int GetStorageStructure64(ref IntPtr deviceStorageStruct);

        private static int GetStorageStructureNative(ref IntPtr deviceStorageStruct)
        {
            return GetStorageStructure32(ref deviceStorageStruct);
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

        [DllImport(ProgrammerDll, EntryPoint = "SendOptionBytesCmd", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SendOptionBytesCmd32(IntPtr command);

        //[DllImport(ProgrammerDll64, EntryPoint = "SendOptionBytesCmd", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int SendOptionBytesCmd64(IntPtr command);

        private static int SendOptionBytesCmdNative(IntPtr command)
        { 
            return SendOptionBytesCmd32(command);
        }

        internal static int SendOptionBytesCmd(string command)
        {
            var commandPtr = IntPtr.Zero;

            try
            {
                commandPtr = Marshal.StringToHGlobalAnsi(command);
                return SendOptionBytesCmdNative(commandPtr);
            }
            catch (OutOfMemoryException ex)
            {
                throw new Exception("K-Society CubeProgrammer out of memory exception.", ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new Exception("K-Society CubeProgrammer argument out of range exception.", ex);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
            finally
            {
                Marshal.FreeHGlobal(commandPtr);
            }
        }

        #endregion

        #region [InitOptionBytesInterface]

        [DllImport(ProgrammerDll, EntryPoint = "InitOptionBytesInterface", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern IntPtr InitOptionBytesInterface32();

        //[DllImport(ProgrammerDll64, EntryPoint = "InitOptionBytesInterface", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern IntPtr InitOptionBytesInterface64();

        private static IntPtr InitOptionBytesInterfaceNative()
        {
            return InitOptionBytesInterface32();
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

        [DllImport(ProgrammerDll, EntryPoint = "FastRomInitOptionBytesInterface", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern IntPtr FastRomInitOptionBytesInterface32(ushort deviceId);

        //[DllImport(ProgrammerDll64, EntryPoint = "FastRomInitOptionBytesInterface", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern IntPtr FastRomInitOptionBytesInterface64(ushort deviceId);

        private static IntPtr FastRomInitOptionBytesInterfaceNative(ushort deviceId)
        {
            return FastRomInitOptionBytesInterface32(deviceId);
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

        [DllImport(ProgrammerDll, EntryPoint = "ObDisplay", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int ObDisplay32();

        //[DllImport(ProgrammerDll64, EntryPoint = "ObDisplay", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int ObDisplay64();

        private static int ObDisplayNative()
        {
            return ObDisplay32();
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

        [DllImport(ProgrammerDll, EntryPoint = "SetLoadersPath", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void SetLoadersPath32(string path);

        //[DllImport(ProgrammerDll64, EntryPoint = "SetLoadersPath", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern void SetLoadersPath64(string path);

        private static void SetLoadersPathNative(string path)
        {
            SetLoadersPath32(path);
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

        [DllImport(ProgrammerDll, EntryPoint = "SetExternalLoaderPath", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void SetExternalLoaderPath32(string path, ref IntPtr externalLoaderInfo);

        //[DllImport(ProgrammerDll64, EntryPoint = "SetExternalLoaderPath", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern void SetExternalLoaderPath64(string path, ref IntPtr externalLoaderInfo);

        private static void SetExternalLoaderPathNative(string path, ref IntPtr externalLoaderInfo)
        {
            SetExternalLoaderPath32(path, ref externalLoaderInfo);
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

        [DllImport(ProgrammerDll, EntryPoint = "SetExternalLoaderOBL", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void SetExternalLoaderOBL32(string path, ref IntPtr externalLoaderInfo);

        //[DllImport(ProgrammerDll64, EntryPoint = "SetExternalLoaderOBL", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern void SetExternalLoaderOBL64(string path, ref IntPtr externalLoaderInfo);

        private static void SetExternalLoaderOBLNative(string path, ref IntPtr externalLoaderInfo)
        {
            SetExternalLoaderOBL32(path, ref externalLoaderInfo);
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

        [DllImport(ProgrammerDll, EntryPoint = "GetExternalLoaders", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetExternalLoaders32(string path, ref IntPtr externalStorageNfo);

        //[DllImport(ProgrammerDll64, EntryPoint = "GetExternalLoaders", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int GetExternalLoaders64(string path, ref IntPtr externalStorageNfo);

        private static int GetExternalLoadersNative(string path, ref IntPtr externalStorageNfo)
        {
            return GetExternalLoaders32(path, ref externalStorageNfo);
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

        [DllImport(ProgrammerDll, EntryPoint = "RemoveExternalLoader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void RemoveExternalLoader32(string path);

        //[DllImport(ProgrammerDll64, EntryPoint = "RemoveExternalLoader", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern void RemoveExternalLoader64(string path);

        private static void RemoveExternalLoaderNative(string path)
        {
            RemoveExternalLoader32(path);
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

        [DllImport(ProgrammerDll, EntryPoint = "DeleteLoaders", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void DeleteLoaders32();

        //[DllImport(ProgrammerDll64, EntryPoint = "DeleteLoaders", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern void DeleteLoaders64();

        private static void DeleteLoadersNative()
        {
            DeleteLoaders32();
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

        [DllImport(ProgrammerDll, EntryPoint = "GetUID64", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetUID6432(ref IntPtr data);

        //[DllImport(ProgrammerDll64, EntryPoint = "GetUID64", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int GetUID6464(ref IntPtr data);

        private static int GetUID64Native(ref IntPtr data)
        {
            return GetUID6432(ref data);
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

        [DllImport(ProgrammerDll, EntryPoint = "FirmwareDelete", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int FirmwareDelete32();

        //[DllImport(ProgrammerDll64, EntryPoint = "FirmwareDelete", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int FirmwareDelete64();

        private static int FirmwareDeleteNative()
        {
            return FirmwareDelete32();
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

        [DllImport(ProgrammerDll, EntryPoint = "FirmwareUpgrade", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int FirmwareUpgrade32([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint firstInstall, uint startStack, uint verify);

        //[DllImport(ProgrammerDll64, EntryPoint = "FirmwareUpgrade", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int FirmwareUpgrade64([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint firstInstall, uint startStack, uint verify);

        private static int FirmwareUpgradeNative(string filePath, uint address, uint firstInstall, uint startStack, uint verify)
        {
            return FirmwareUpgrade32(filePath, address, firstInstall, startStack, verify);
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

        [DllImport(ProgrammerDll, EntryPoint = "StartWirelessStack", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int StartWirelessStack32();

        //[DllImport(ProgrammerDll64, EntryPoint = "StartWirelessStack", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int StartWirelessStack64();

        private static int StartWirelessStackNative()
        {
            return StartWirelessStack32();
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

        [DllImport(ProgrammerDll, EntryPoint = "UpdateAuthKey", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int UpdateAuthKey32([MarshalAs(UnmanagedType.LPWStr)] string filePath);

        //[DllImport(ProgrammerDll64, EntryPoint = "UpdateAuthKey", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int UpdateAuthKey64([MarshalAs(UnmanagedType.LPWStr)] string filePath);

        private static int UpdateAuthKeyNative(string filePath)
        {
            return UpdateAuthKey32(filePath);
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

        [DllImport(ProgrammerDll, EntryPoint = "AuthKeyLock", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int AuthKeyLock32();

        //[DllImport(ProgrammerDll64, EntryPoint = "AuthKeyLock", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int AuthKeyLock64();

        private static int AuthKeyLockNative()
        {
            return AuthKeyLock32();
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

        [DllImport(ProgrammerDll, EntryPoint = "WriteUserKey", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int WriteUserKey32([MarshalAs(UnmanagedType.LPWStr)] string filePath, byte keyType);

        //[DllImport(ProgrammerDll64, EntryPoint = "WriteUserKey", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int WriteUserKey64([MarshalAs(UnmanagedType.LPWStr)] string filePath, byte keyType);

        private static int WriteUserKeyNative(string filePath, byte keyType)
        {
            return WriteUserKey32(filePath, keyType);
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

        [DllImport(ProgrammerDll, EntryPoint = "AntiRollBack", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int AntiRollBack32();

        //[DllImport(ProgrammerDll64, EntryPoint = "AntiRollBack", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int AntiRollBack64();

        private static int AntiRollBackNative()
        {
            return AntiRollBack32();
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

        [DllImport(ProgrammerDll, EntryPoint = "StartFus", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int StartFus32();

        //[DllImport(ProgrammerDll64, EntryPoint = "StartFus", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int StartFus64();

        private static int StartFusNative()
        {
            return StartFus32();
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

        [DllImport(ProgrammerDll, EntryPoint = "UnlockChip", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int UnlockChip32();

        //[DllImport(ProgrammerDll64, EntryPoint = "UnlockChip", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern int UnlockChip64();

        private static int UnlockChipNative()
        {
            return UnlockChip32();
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

        [DllImport(ProgrammerDll, EntryPoint = "ProgramSsp", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ProgramSsp32([MarshalAs(UnmanagedType.LPWStr)] string sspFile, [MarshalAs(UnmanagedType.LPWStr)] string licenseFile, [MarshalAs(UnmanagedType.LPWStr)] string tfaFile, int hsmSlotId);

        //[DllImport(ProgrammerDll64, EntryPoint = "ProgramSsp", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int ProgramSsp64([MarshalAs(UnmanagedType.LPWStr)] string sspFile, [MarshalAs(UnmanagedType.LPWStr)] string licenseFile, [MarshalAs(UnmanagedType.LPWStr)] string tfaFile, int hsmSlotId);

        private static int ProgramSspNative(string sspFile, string licenseFile, string tfaFile, int hsmSlotId)
        {
            return ProgramSsp32(sspFile, licenseFile, tfaFile, hsmSlotId);
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

        [DllImport(ProgrammerDll, EntryPoint = "GetHsmFirmwareID", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern string GetHsmFirmwareID32(int hsmSlotId);

        //[DllImport(ProgrammerDll64, EntryPoint = "GetHsmFirmwareID", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern string GetHsmFirmwareID64(int hsmSlotId);

        private static string GetHsmFirmwareIDNative(int hsmSlotId)
        {
            return GetHsmFirmwareID32(hsmSlotId);
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

        [DllImport(ProgrammerDll, EntryPoint = "GetHsmCounter", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern ulong GetHsmCounter32(int hsmSlotId);

        //[DllImport(ProgrammerDll64, EntryPoint = "GetHsmCounter", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //private static extern ulong GetHsmCounter64(int hsmSlotId);

        private static ulong GetHsmCounterNative(int hsmSlotId)
        {
            return GetHsmCounter32(hsmSlotId);
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

        [DllImport(ProgrammerDll, EntryPoint = "GetHsmState", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern string GetHsmState32(int hsmSlotId);

        //[DllImport(ProgrammerDll64, EntryPoint = "GetHsmState", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern string GetHsmState64(int hsmSlotId);

        private static string GetHsmStateNative(int hsmSlotId)
        {
            return GetHsmState32(hsmSlotId);
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

        [DllImport(ProgrammerDll, EntryPoint = "GetHsmVersion", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern string GetHsmVersion32(int hsmSlotId);

        //[DllImport(ProgrammerDll64, EntryPoint = "GetHsmVersion", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern string GetHsmVersion64(int hsmSlotId);

        private static string GetHsmVersionNative(int hsmSlotId)
        {
            return GetHsmVersion32(hsmSlotId);
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

        [DllImport(ProgrammerDll, EntryPoint = "GetHsmType", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern string GetHsmType32(int hsmSlotId);

        //[DllImport(ProgrammerDll64, EntryPoint = "GetHsmType", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern string GetHsmType64(int hsmSlotId);

        private static string GetHsmTypeNative(int hsmSlotId)
        {
            return GetHsmType32(hsmSlotId);
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

        [DllImport(ProgrammerDll, EntryPoint = "GetHsmLicense", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetHsmLicense32(int hsmSlotId, [MarshalAs(UnmanagedType.LPWStr)] string outLicensePath);

        //[DllImport(ProgrammerDll64, EntryPoint = "GetHsmLicense", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern int GetHsmLicense64(int hsmSlotId, [MarshalAs(UnmanagedType.LPWStr)] string outLicensePath);

        private static int GetHsmLicenseNative(int hsmSlotId, string outLicensePath)
        {
            return GetHsmLicense32(hsmSlotId, outLicensePath);
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
