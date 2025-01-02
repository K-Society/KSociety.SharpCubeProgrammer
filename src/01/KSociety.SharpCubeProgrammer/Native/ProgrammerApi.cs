// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Native
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using Enum;
    using Struct;

    internal static class ProgrammerApi
    {
        /// <summary>
        /// Synchronization object to protect loading the native library and its functions. This field is read-only.
        /// </summary>
        private static readonly object SyncRoot = new object();

        internal static SafeLibraryHandle HandleSTLinkDriver;
        internal static SafeLibraryHandle HandleProgrammer;

        internal static DisplayCallBacks DisplayCallBacks = new DisplayCallBacks();

        private const string ProgrammerDll = @"Programmer.dll";

        internal static bool EnsureNativeLibraryLoaded()
        {
            //if (HandleSTLinkDriver == null || HandleProgrammer == null)
            //{
            //    var currentDirectory = GetAssemblyDirectory();
            //    var target = Path.Combine(currentDirectory, "dll", Environment.Is64BitProcess ? "x64" : "x86");

            //    try
            //    { 
            //        var stLinkDriverResult = LoadStLinkDriver(target);

            //        if (stLinkDriverResult != null)
            //        {
            //            var programmerResult = LoadProgrammer(target);

            //            if (programmerResult != null)
            //            {
            //                return true;
            //            }
            //            else
            //            {
            //                return false;
            //            }
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    }catch(Exception ex)
            //    {
            //        throw new Exception("K-Society CubeProgrammer native library loading error!", ex);
            //    }
            //}

            return true;
        }

        private static SafeLibraryHandle LoadStLinkDriver(string target)
        {
            if (HandleSTLinkDriver == null)
            {
                lock (SyncRoot)
                {
                    if (HandleSTLinkDriver == null)
                    {
#if NET
                        if (!NativeLibrary.TryLoad(
                            target + @"\STLinkUSBDriver.dll",
                            typeof(CubeProgrammerApi).Assembly,
                            DllImportSearchPath.UserDirectories,
                            out IntPtr handle))
                        {
                            var error = Marshal.GetLastWin32Error();
                            HandleSTLinkDriver = null;

                            throw new Exception("K-Society CubeProgrammer StLinkDriver loading error: " + error);
                        }
                        else
                        {
                            HandleSTLinkDriver = new Native.SafeLibraryHandle(handle);
                        }
#else
                        // Check if the local machine has KB2533623 installed in order
                        // to use the more secure flags when calling LoadLibraryEx
                        bool hasKB2533623;

                        using (var hModule = Utility.LoadLibraryEx(Utility.KernelLibName, IntPtr.Zero, 0))
                        {
                            // If the AddDllDirectory function is found then the flags are supported.
                            hasKB2533623 = Utility.GetProcAddress(hModule, "AddDllDirectory") != IntPtr.Zero;
                        }

                        var dwFlags = 0;

                        if (hasKB2533623)
                        {
                            // If KB2533623 is installed then specify the more secure LOAD_LIBRARY_SEARCH_DEFAULT_DIRS in dwFlags.
                            dwFlags = Utility.LOAD_LIBRARY_SEARCH_DEFAULT_DIRS;
                        }

                        HandleSTLinkDriver = Utility.LoadLibraryEx(target + @"\STLinkUSBDriver.dll", IntPtr.Zero, dwFlags);

                        if (HandleSTLinkDriver.IsInvalid)
                        {
                            var error = Marshal.GetLastWin32Error();
                            HandleSTLinkDriver = null;

                            throw new Exception("K-Society CubeProgrammer StLinkDriver loading error: " + error);
                        }
#endif
                    }
                }
            }

            return HandleSTLinkDriver;
        }

        private static SafeLibraryHandle LoadProgrammer(string target)
        {
            if (HandleProgrammer == null)
            {
                lock (SyncRoot)
                {
                    if (HandleProgrammer == null)
                    {
#if NET
                        if (!NativeLibrary.TryLoad(
                            target + @"\Programmer.dll",
                            typeof(CubeProgrammerApi).Assembly,
                            DllImportSearchPath.UserDirectories,
                            out IntPtr handle))
                        {
                            var error = Marshal.GetLastWin32Error();
                            HandleProgrammer = null;

                            throw new Exception("K-Society CubeProgrammer Programmer loading error: " + error);
                        }
                        else
                        {
                            HandleProgrammer = new Native.SafeLibraryHandle(handle);
                        }
#else                   
                        var dwFlags = Utility.LOAD_WITH_ALTERED_SEARCH_PATH;

                        HandleProgrammer = Utility.LoadLibraryEx(target + @"\Programmer.dll", IntPtr.Zero, dwFlags);

                        if (HandleProgrammer.IsInvalid)
                        {
                            var error = Marshal.GetLastWin32Error();
                            HandleProgrammer = null;

                            throw new Exception("K-Society CubeProgrammer Programmer loading error: " + error);
                        }
#endif
                    }
                }
            }

            return HandleProgrammer;
        }

        private static string GetAssemblyDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().Location;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        #region [STLINK]

        #region [TryConnectStLink]

        [DllImport(ProgrammerDll, EntryPoint = "TryConnectStLink", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int TryConnectStLinkC(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode);

        private static int TryConnectStLinkNative(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode)
        {
            return TryConnectStLinkC(stLinkProbeIndex, shared, debugConnectMode);
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
        private static extern int GetStLinkListC(ref IntPtr stLinkList, int shared);

        private static int GetStLinkListNative(ref IntPtr stLinkList, int shared)
        {
            return GetStLinkListC(ref stLinkList, shared);
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
        private static extern int GetStLinkEnumerationListC(ref IntPtr stLinkList, int shared);

        private static int GetStLinkEnumerationListNative(ref IntPtr stLinkList, int shared)
        {
            return GetStLinkEnumerationListC(ref stLinkList, shared);
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
        private static extern int ConnectStLinkC(DebugConnectParameters debugParameters);

        private static int ConnectStLinkNative(DebugConnectParameters debugParameters)
        {
            return ConnectStLinkC(debugParameters);
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
        private static extern int ResetC([MarshalAs(UnmanagedType.U4)] DebugResetMode rstMode);

        private static int ResetNative(DebugResetMode rstMode)
        {
            return ResetC(rstMode);
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
        private static extern int GetUsartListC(ref IntPtr usartList);

        private static int GetUsartListNative(ref IntPtr usartList)
        {
            return GetUsartListC(ref usartList);
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
        private static extern int ConnectUsartBootloaderC(UsartConnectParameters usartParameters);

        private static int ConnectUsartBootloaderNative(UsartConnectParameters usartParameters)
        {
            return ConnectUsartBootloaderC(usartParameters);
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
        private static extern int SendByteUartC(int bytes);

        private static int SendByteUartNative(int bytes)
        {
            return SendByteUartC(bytes);
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
        private static extern int GetDfuDeviceListC(ref IntPtr dfuList, int iPID, int iVID);

        private static int GetDfuDeviceListNative(ref IntPtr dfuList, int iPID, int iVID)
        {
            return GetDfuDeviceListC(ref dfuList, iPID, iVID);
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
        private static extern int ConnectDfuBootloaderC(IntPtr usbIndex);

        private static int ConnectDfuBootloaderNative(IntPtr usbIndex)
        {
            return ConnectDfuBootloaderC(usbIndex);
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
        private static extern int ConnectDfuBootloader2C(DfuConnectParameters dfuParameters);

        private static int ConnectDfuBootloader2Native(DfuConnectParameters dfuParameters)
        {
            return ConnectDfuBootloader2C(dfuParameters);
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
        private static extern int ConnectSpiBootloaderC(SpiConnectParameters spiParameters);

        private static int ConnectSpiBootloaderNative(SpiConnectParameters spiParameters)
        {
            return ConnectSpiBootloaderC(spiParameters);
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
        private static extern int ConnectCanBootloaderC(CanConnectParameters canParameters);

        private static int ConnectCanBootloaderNative(CanConnectParameters canParameters)
        {
            return ConnectCanBootloaderC(canParameters);
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
        private static extern int ConnectI2cBootloaderC(I2CConnectParameters i2cParameters);

        private static int ConnectI2cBootloaderNative(I2CConnectParameters i2cParameters)
        {
            return ConnectI2cBootloaderC(i2cParameters);
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
        private static extern void SetDisplayCallbacksC(DisplayCallBacks c);

        private static void SetDisplayCallbacksNative(DisplayCallBacks c)
        {
            SetDisplayCallbacksC(c);
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
        private static extern void SetVerbosityLevelC(int level);

        private static void SetVerbosityLevelNative(int level)
        {
            SetVerbosityLevelC(level);
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
        private static extern bool CheckDeviceConnectionC();

        private static bool CheckDeviceConnectionNative()
        {
            return CheckDeviceConnectionC();
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
        private static extern IntPtr GetDeviceGeneralInfC();

        private static IntPtr GetDeviceGeneralInfNative()
        {
            return GetDeviceGeneralInfC();
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
        private static extern int ReadMemoryC(uint address, ref IntPtr data, uint size);

        private static int ReadMemoryNative(uint address, ref IntPtr data, uint size)
        {
            return ReadMemoryC(address, ref data, size);
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
        private static extern int WriteMemoryC(uint address, IntPtr data, uint size);

        private static int WriteMemoryNative(uint address, IntPtr data, uint size)
        {
            return WriteMemoryC(address, data, size);
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
        private static extern int WriteMemoryAutoFillC(uint address, IntPtr data, uint size);

        private static int WriteMemoryAutoFillNative(uint address, IntPtr data, uint size)
        {
            return WriteMemoryAutoFillC(address, data, size);
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
        private static extern int WriteMemoryAndVerifyC(uint address, IntPtr data, uint size);

        private static int WriteMemoryAndVerifyNative(uint address, IntPtr data, uint size)
        {
            return WriteMemoryAndVerifyC(address, data, size);
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
        private static extern int EditSectorC(uint address, IntPtr data, uint size);

        private static int EditSectorNative(uint address, IntPtr data, uint size)
        {
            return EditSectorC(address, data, size);
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
        private static extern int DownloadFileC([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, [MarshalAs(UnmanagedType.LPWStr)] string binPath);

        private static int DownloadFileNative(string filePath, uint address, uint skipErase, uint verify, string binPath)
        {
            return DownloadFileC(filePath, address, skipErase, verify, binPath);
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
        private static extern int ExecuteC(uint address);

        private static int ExecuteNative(uint address)
        {
            return ExecuteC(address);
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
        private static extern int MassEraseC(IntPtr sFlashMemName);

        private static int MassEraseNative(IntPtr sFlashMemName)
        {
            return MassEraseC(sFlashMemName);
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
        private static extern int SectorEraseC(uint[] sectors, uint sectorNbr, IntPtr sFlashMemName);

        private static int SectorEraseNative(uint[] sectors, uint sectorNbr, IntPtr sFlashMemName)
        {
            return SectorEraseC(sectors, sectorNbr, sFlashMemName);
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
        private static extern int ReadUnprotectC();

        private static int ReadUnprotectNative()
        {
            return ReadUnprotectC();
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
        private static extern int TzenRegressionC();

        private static int TzenRegressionNative()
        {
            return TzenRegressionC();
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
        private static extern int GetTargetInterfaceTypeC();

        private static int GetTargetInterfaceTypeNative()
        {
            return GetTargetInterfaceTypeC();
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
        private static extern int GetCancelPointerC();

        private static int GetCancelPointerNative()
        {
            return GetCancelPointerC();
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
        private static extern IntPtr FileOpenC([MarshalAs(UnmanagedType.LPWStr)] string filePath);

        private static IntPtr FileOpenNative(string filePath)
        {
            return FileOpenC(filePath);
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
        private static extern void FreeFileDataC(IntPtr data);

        private static void FreeFileDataNative(IntPtr data)
        {
            FreeFileDataC(data);
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
        private static extern void FreeLibraryMemoryC(IntPtr ptr);

        private static void FreeLibraryMemoryNative(IntPtr ptr)
        {
            FreeLibraryMemoryC(ptr);
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
        private static extern int VerifyC(IntPtr fileData, uint address);

        private static int VerifyNative(IntPtr fileData, uint address)
        {
            return VerifyC(fileData, address);
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
        private static extern int VerifyMemoryC(uint address, IntPtr data, uint size);

        private static int VerifyMemoryNative(uint address, IntPtr data, uint size)
        {
            return VerifyMemoryC(address, data, size);
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
        private static extern int VerifyMemoryBySegmentC(uint address, IntPtr data, uint size);

        private static int VerifyMemoryBySegmentNative(uint address, IntPtr data, uint size)
        {
            return VerifyMemoryBySegmentC(address, data, size);
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
        private static extern int SaveFileToFileC(IntPtr fileData, [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        private static int SaveFileToFileNative(IntPtr fileData, string sFileName)
        {
            return SaveFileToFileC(fileData, sFileName);
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
        private static extern int SaveMemoryToFileC(int address, int size, [MarshalAs(UnmanagedType.LPWStr)] string sFileName);

        private static int SaveMemoryToFileNative(int address, int size, string sFileName)
        {
            return SaveMemoryToFileC(address, size, sFileName);
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
        private static extern int DisconnectC();

        private static int DisconnectNative()
        {
            return DisconnectC();
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
        private static extern void DeleteInterfaceListC();

        private static void DeleteInterfaceListNative()
        {
            DeleteInterfaceListC();
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
        private static extern void AutomaticModeC([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, int isMassErase, IntPtr obCommand, int run);

        private static void AutomaticModeNative(string filePath, uint address, uint skipErase, uint verify, int isMassErase, IntPtr obCommand, int run)
        {
            AutomaticModeC(filePath, address, skipErase, verify, isMassErase, obCommand, run);
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
        private static extern void SerialNumberingAutomaticModeC([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint skipErase, uint verify, int isMassErase, IntPtr obCommand, int run, int enableSerialNumbering, int serialAddress, int serialSize, string serialInitialData);

        private static void SerialNumberingAutomaticModeNative(string filePath, uint address, uint skipErase, uint verify, int isMassErase, IntPtr obCommand, int run, int enableSerialNumbering, int serialAddress, int serialSize, string serialInitialData)
        {
            SerialNumberingAutomaticModeC(filePath, address, skipErase, verify, isMassErase, obCommand, run, enableSerialNumbering, serialAddress, serialSize, serialInitialData);
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
        private static extern int GetStorageStructureC(ref IntPtr deviceStorageStruct);

        private static int GetStorageStructureNative(ref IntPtr deviceStorageStruct)
        {
            return GetStorageStructureC(ref deviceStorageStruct);
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
        private static extern int SendOptionBytesCmdC(IntPtr command);

        private static int SendOptionBytesCmdNative(IntPtr command)
        { 
            return SendOptionBytesCmdC(command);
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
        private static extern IntPtr InitOptionBytesInterfaceC();

        private static IntPtr InitOptionBytesInterfaceNative()
        {
            return InitOptionBytesInterfaceC();
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
        private static extern IntPtr FastRomInitOptionBytesInterfaceC(ushort deviceId);

        private static IntPtr FastRomInitOptionBytesInterfaceNative(ushort deviceId)
        {
            return FastRomInitOptionBytesInterfaceC(deviceId);
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
        private static extern int ObDisplayC();

        private static int ObDisplayNative()
        {
            return ObDisplayC();
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
        private static extern void SetLoadersPathC(string path);

        private static void SetLoadersPathNative(string path)
        {
            SetLoadersPathC(path);
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
        private static extern void SetExternalLoaderPathC(string path, ref IntPtr externalLoaderInfo);

        private static void SetExternalLoaderPathNative(string path, ref IntPtr externalLoaderInfo)
        {
            SetExternalLoaderPathC(path, ref externalLoaderInfo);
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
        private static extern void SetExternalLoaderOBLC(string path, ref IntPtr externalLoaderInfo);

        private static void SetExternalLoaderOBLNative(string path, ref IntPtr externalLoaderInfo)
        {
            SetExternalLoaderOBLC(path, ref externalLoaderInfo);
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
        private static extern int GetExternalLoadersC(string path, ref IntPtr externalStorageNfo);

        private static int GetExternalLoadersNative(string path, ref IntPtr externalStorageNfo)
        {
            return GetExternalLoadersC(path, ref externalStorageNfo);
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
        private static extern void RemoveExternalLoaderC(string path);

        private static void RemoveExternalLoaderNative(string path)
        {
            RemoveExternalLoaderC(path);
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
        private static extern void DeleteLoadersC();

        private static void DeleteLoadersNative()
        {
            DeleteLoadersC();
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

        #region [STMCWB specific]

        #region [GetUID64]

        [DllImport(ProgrammerDll, EntryPoint = "GetUID64", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int GetUID64C(ref IntPtr data);

        private static int GetUID64Native(ref IntPtr data)
        {
            return GetUID64C(ref data);
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
        private static extern int FirmwareDeleteC();

        private static int FirmwareDeleteNative()
        {
            return FirmwareDeleteC();
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
        private static extern int FirmwareUpgradeC([MarshalAs(UnmanagedType.LPWStr)] string filePath, uint address, uint firstInstall, uint startStack, uint verify);

        private static int FirmwareUpgradeNative(string filePath, uint address, uint firstInstall, uint startStack, uint verify)
        {
            return FirmwareUpgradeC(filePath, address, firstInstall, startStack, verify);
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
        private static extern int StartWirelessStackC();

        private static int StartWirelessStackNative()
        {
            return StartWirelessStackC();
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
        private static extern int UpdateAuthKeyC([MarshalAs(UnmanagedType.LPWStr)] string filePath);

        private static int UpdateAuthKeyNative(string filePath)
        {
            return UpdateAuthKeyC(filePath);
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
        private static extern int AuthKeyLockC();

        private static int AuthKeyLockNative()
        {
            return AuthKeyLockC();
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
        private static extern int WriteUserKeyC([MarshalAs(UnmanagedType.LPWStr)] string filePath, byte keyType);

        private static int WriteUserKeyNative(string filePath, byte keyType)
        {
            return WriteUserKeyC(filePath, keyType);
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
        private static extern int AntiRollBackC();

        private static int AntiRollBackNative()
        {
            return AntiRollBackC();
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
        private static extern int StartFusC();

        private static int StartFusNative()
        {
            return StartFusC();
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
        private static extern int UnlockChipC();

        private static int UnlockChipNative()
        {
            return UnlockChipC();
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

        #region [STMCMP specific functions]

        #region [ProgramSsp]

        [DllImport(ProgrammerDll, EntryPoint = "ProgramSsp", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ProgramSspC([MarshalAs(UnmanagedType.LPWStr)] string sspFile, [MarshalAs(UnmanagedType.LPWStr)] string licenseFile, [MarshalAs(UnmanagedType.LPWStr)] string tfaFile, int hsmSlotId);

        private static int ProgramSspNative(string sspFile, string licenseFile, string tfaFile, int hsmSlotId)
        {
            return ProgramSspC(sspFile, licenseFile, tfaFile, hsmSlotId);
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

        #region [STMC HSM specific functions]

        #region [GetHsmFirmwareID]

        [DllImport(ProgrammerDll, EntryPoint = "GetHsmFirmwareID", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern string GetHsmFirmwareIDC(int hsmSlotId);

        private static string GetHsmFirmwareIDNative(int hsmSlotId)
        {
            return GetHsmFirmwareIDC(hsmSlotId);
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
        private static extern ulong GetHsmCounterC(int hsmSlotId);

        private static ulong GetHsmCounterNative(int hsmSlotId)
        {
            return GetHsmCounterC(hsmSlotId);
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
        private static extern string GetHsmStateC(int hsmSlotId);

        private static string GetHsmStateNative(int hsmSlotId)
        {
            return GetHsmStateC(hsmSlotId);
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
        private static extern string GetHsmVersionC(int hsmSlotId);

        private static string GetHsmVersionNative(int hsmSlotId)
        {
            return GetHsmVersionC(hsmSlotId);
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
        private static extern string GetHsmTypeC(int hsmSlotId);

        private static string GetHsmTypeNative(int hsmSlotId)
        {
            return GetHsmTypeC(hsmSlotId);
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
        private static extern int GetHsmLicenseC(int hsmSlotId, [MarshalAs(UnmanagedType.LPWStr)] string outLicensePath);

        private static int GetHsmLicenseNative(int hsmSlotId, string outLicensePath)
        {
            return GetHsmLicenseC(hsmSlotId, outLicensePath);
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
