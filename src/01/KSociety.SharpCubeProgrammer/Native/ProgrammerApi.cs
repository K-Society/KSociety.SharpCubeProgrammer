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
            if (HandleSTLinkDriver == null || HandleProgrammer == null)
            {
                var currentDirectory = GetAssemblyDirectory();
                var target = Path.Combine(currentDirectory, "dll", Environment.Is64BitProcess ? "x64" : "x86");

                try
                { 
                    var stLinkDriverResult = LoadStLinkDriver(target);

                    if (stLinkDriverResult != null)
                    {
                        var programmerResult = LoadProgrammer(target);

                        if (programmerResult != null)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }catch(Exception ex)
                {
                    throw new Exception("K-Society CubeProgrammer native library loading error!", ex);
                }
            }

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

        internal static int TryConnectStLink(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode)
        {
            try
            {
                return TryConnectStLinkC(stLinkProbeIndex, shared, debugConnectMode);
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

        internal static int GetStLinkList(ref IntPtr stLinkList, int shared)
        {
            try
            {
                return GetStLinkListC(ref stLinkList, shared);
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

        internal static int GetStLinkEnumerationList(ref IntPtr stLinkList, int shared)
        {
            try
            {
                return GetStLinkEnumerationListC(ref stLinkList, shared);
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

        internal static int ConnectStLink(DebugConnectParameters debugParameters)
        {
            try
            {
                return ConnectStLinkC(debugParameters);
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

        internal static int Reset(DebugResetMode rstMode)
        {
            try
            {
                return ResetC(rstMode);
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

        internal static int GetUsartList(ref IntPtr usartList)
        {
            try
            {
                return GetUsartListC(ref usartList);
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

        internal static int ConnectUsartBootloader(UsartConnectParameters usartParameters)
        {
            try
            {
                return ConnectUsartBootloaderC(usartParameters);
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

        internal static int SendByteUart(int bytes)
        {
            try
            {
                return SendByteUartC(bytes);
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

        internal static int GetDfuDeviceList(ref IntPtr dfuList, int iPID, int iVID)
        {
            try
            {
                return GetDfuDeviceListC(ref dfuList, iPID, iVID);
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

        internal static int ConnectDfuBootloader(string usbIndex)
        {
            var usbIndexPtr = IntPtr.Zero;

            try
            {
                usbIndexPtr = Marshal.StringToHGlobalAnsi(usbIndex);
                return ConnectDfuBootloaderC(usbIndexPtr);
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

        internal static int ConnectDfuBootloader2(DfuConnectParameters dfuParameters)
        {
            try
            {
                return ConnectDfuBootloader2C(dfuParameters);
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

        internal static int ConnectSpiBootloader(SpiConnectParameters spiParameters)
        {
            try
            {
                return ConnectSpiBootloaderC(spiParameters);
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

        internal static int ConnectCanBootloader(CanConnectParameters canParameters)
        {
            try
            {
                return ConnectCanBootloaderC(canParameters);
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

        internal static int ConnectI2cBootloader(I2CConnectParameters i2cParameters)
        {
            try
            {
                return ConnectI2cBootloaderC(i2cParameters);
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

        internal static void SetDisplayCallbacks(DisplayCallBacks c)
        {
            try
            {
                SetDisplayCallbacksC(c);
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

        internal static void SetVerbosityLevel(int level)
        {
            try
            {
                SetVerbosityLevelC(level);
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

        internal static bool CheckDeviceConnection()
        {
            try
            {
                return CheckDeviceConnectionC();
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

        internal static IntPtr GetDeviceGeneralInf()
        {
            try
            {
                return GetDeviceGeneralInfC();
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

        internal static int ReadMemory(uint address, ref IntPtr data, uint size)
        {
            try
            {
                return ReadMemoryC(address, ref data, size);
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

        internal static int WriteMemory(uint address, IntPtr data, uint size)
        {
            try
            {
                return WriteMemoryC(address, data, size);
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

        internal static int WriteMemoryAutoFill(uint address, IntPtr data, uint size)
        {
            try
            {
                return WriteMemoryAutoFillC(address, data, size);
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

        internal static int WriteMemoryAndVerify(uint address, IntPtr data, uint size)
        {
            try
            {
                return WriteMemoryAndVerifyC(address, data, size);
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

        internal static int EditSector(uint address, IntPtr data, uint size)
        {
            try
            {
                return EditSectorC(address, data, size);
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

        internal static int DownloadFile(string filePath, uint address, uint skipErase, uint verify, string binPath)
        {
            try
            {
                return DownloadFileC(filePath, address, skipErase, verify, binPath);
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

        internal static int Execute(uint address)
        {
            try
            {
                return ExecuteC(address);
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

        internal static int MassErase(string sFlashMemName)
        {
            var sFlashMemNamePtr = IntPtr.Zero;

            try
            {
                sFlashMemNamePtr = Marshal.StringToHGlobalAnsi(sFlashMemName);
                return MassEraseC(sFlashMemNamePtr);
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

        internal static int SectorErase(uint[] sectors, uint sectorNbr, string sFlashMemName)
        {
            var sFlashMemNamePtr = IntPtr.Zero;

            try
            {
                sFlashMemNamePtr = Marshal.StringToHGlobalAnsi(sFlashMemName);
                return SectorEraseC(sectors, sectorNbr, sFlashMemNamePtr);
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

        internal static int ReadUnprotect()
        {
            try
            {
                return ReadUnprotectC();
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

        internal static int TzenRegression()
        {
            try
            {
                return TzenRegressionC();
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

        internal static int GetTargetInterfaceType()
        {
            try
            {
                return GetTargetInterfaceTypeC();
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

        internal static int GetCancelPointer()
        {
            try
            {
                return GetCancelPointerC();
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

        internal static IntPtr FileOpen(string filePath)
        {
            try
            {
                return FileOpenC(filePath);
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

        internal static void FreeFileData(IntPtr data)
        {
            try
            {
                FreeFileDataC(data);
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

        internal static void FreeLibraryMemory(IntPtr ptr)
        {
            try
            {
                FreeLibraryMemoryC(ptr);
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

        internal static int Verify(IntPtr fileData, uint address)
        {
            try
            {
                return VerifyC(fileData, address);
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

        internal static int VerifyMemory(uint address, IntPtr data, uint size)
        {
            try
            {
                return VerifyMemoryC(address, data, size);
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

        internal static int VerifyMemoryBySegment(uint address, IntPtr data, uint size)
        {
            try
            {
                return VerifyMemoryBySegmentC(address, data, size);
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

        internal static int SaveFileToFile(IntPtr fileData, string sFileName)
        {
            try
            {
                return SaveFileToFileC(fileData, sFileName);
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

        internal static int SaveMemoryToFile(int address, int size, string sFileName)
        {
            try
            {
                return SaveMemoryToFileC(address, size, sFileName);
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

        internal static int Disconnect()
        {
            try
            {
                return DisconnectC();
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

        internal static void DeleteInterfaceList()
        {
            try
            {
                DeleteInterfaceListC();
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

        internal static void AutomaticMode(string filePath, uint address, uint skipErase, uint verify, int isMassErase, string obCommand, int run)
        {
            var obCommandPtr = IntPtr.Zero;
            try
            {
                obCommandPtr = Marshal.StringToHGlobalAnsi(obCommand);
                AutomaticModeC(filePath, address, skipErase, verify, isMassErase, obCommandPtr, run);
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

        internal static void SerialNumberingAutomaticMode(string filePath, uint address, uint skipErase, uint verify, int isMassErase, string obCommand, int run, int enableSerialNumbering, int serialAddress, int serialSize, string serialInitialData)
        {
            var obCommandPtr = IntPtr.Zero;

            try
            {
                obCommandPtr = Marshal.StringToHGlobalAnsi(obCommand);
                SerialNumberingAutomaticModeC(filePath, address, skipErase, verify, isMassErase, obCommandPtr, run, enableSerialNumbering, serialAddress, serialSize, serialInitialData);
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

        internal static int GetStorageStructure(ref IntPtr deviceStorageStruct)
        {
            try
            {
                return GetStorageStructureC(ref deviceStorageStruct);
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

        internal static int SendOptionBytesCmd(string command)
        {
            var commandPtr = IntPtr.Zero;

            try
            {
                commandPtr = Marshal.StringToHGlobalAnsi(command);
                return SendOptionBytesCmdC(commandPtr);
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

        internal static IntPtr InitOptionBytesInterface()
        {
            try
            {
                return InitOptionBytesInterfaceC();
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

        internal static IntPtr FastRomInitOptionBytesInterface(ushort deviceId)
        {
            try
            {
                return FastRomInitOptionBytesInterfaceC(deviceId);
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

        internal static int ObDisplay()
        {
            try
            {
                return ObDisplayC();
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

        internal static void SetLoadersPath(string path)
        {
            try
            {
                SetLoadersPathC(path);
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

        internal static void SetExternalLoaderPath(string path, ref IntPtr externalLoaderInfo)
        {
            try
            {
                SetExternalLoaderPathC(path, ref externalLoaderInfo);
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

        internal static void SetExternalLoaderOBL(string path, ref IntPtr externalLoaderInfo)
        {
            try
            {
                SetExternalLoaderOBLC(path, ref externalLoaderInfo);
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

        internal static int GetExternalLoaders(string path, ref IntPtr externalStorageNfo)
        {
            try
            {
                return GetExternalLoadersC(path, ref externalStorageNfo);
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

        internal static void RemoveExternalLoader(string path)
        {
            try
            {
                RemoveExternalLoaderC(path);
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

        internal static void DeleteLoaders()
        {
            try
            {
                DeleteLoadersC();
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

        internal static int GetUID64(ref IntPtr data)
        {
            try
            {
                return GetUID64C(ref data);
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

        internal static int FirmwareDelete()
        {
            try
            {
                return FirmwareDeleteC();
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

        internal static int FirmwareUpgrade(string filePath, uint address, uint firstInstall, uint startStack, uint verify)
        {
            try
            {
                return FirmwareUpgradeC(filePath, address, firstInstall, startStack, verify);
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

        internal static int StartWirelessStack()
        {
            try
            {
                return StartWirelessStackC();
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

        internal static int UpdateAuthKey(string filePath)
        {
            try
            {
                return UpdateAuthKeyC(filePath);
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

        internal static int AuthKeyLock()
        {
            try
            {
                return AuthKeyLockC();
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

        internal static int WriteUserKey(string filePath, byte keyType)
        {
            try
            {
                return WriteUserKeyC(filePath, keyType);
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

        internal static int AntiRollBack()
        {
            try
            {
                return AntiRollBackC();
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

        internal static int StartFus()
        {
            try
            {
                return StartFusC();
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

        internal static int UnlockChip()
        {
            try
            {
                return UnlockChipC();
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

        internal static int ProgramSsp(string sspFile, string licenseFile, string tfaFile, int hsmSlotId)
        {
            try
            {
                return ProgramSspC(sspFile, licenseFile, tfaFile, hsmSlotId);
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

        internal static string GetHsmFirmwareID(int hsmSlotId)
        {
            try
            {
                return GetHsmFirmwareIDC(hsmSlotId);
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

        internal static ulong GetHsmCounter(int hsmSlotId)
        {
            try
            {
                return GetHsmCounterC(hsmSlotId);
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

        internal static string GetHsmState(int hsmSlotId)
        {
            try
            {
                return GetHsmStateC(hsmSlotId);
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

        internal static string GetHsmVersion(int hsmSlotId)
        {
            try
            {
                return GetHsmVersionC(hsmSlotId);
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

        internal static string GetHsmType(int hsmSlotId)
        {
            try
            {
                return GetHsmTypeC(hsmSlotId);
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

        internal static int GetHsmLicense(int hsmSlotId, string outLicensePath)
        {
            try
            {
                return GetHsmLicenseC(hsmSlotId, outLicensePath);
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

        #region [EXTENDED]

        #region [VersionAPI]

        //[DllImport(ProgrammerDll, EntryPoint = "VersionAPI", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        //private static extern string VersionAPIC();

        //internal static string VersionAPI()
        //{
        //    try
        //    {
        //        return VersionAPIC();
        //    }
        //    catch (DllNotFoundException ex)
        //    {
        //        throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
        //    }
        //    catch (EntryPointNotFoundException ex)
        //    {
        //        throw new Exception("K-Society CubeProgrammer operation not found.", ex);
        //    }
        //}

        #endregion

        #region [CpuHalt]

        [DllImport(ProgrammerDll, EntryPoint = "CpuHalt", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void CpuHaltC();

        internal static void CpuHalt()
        {
            try
            {
                CpuHaltC();
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

        #region [CpuRun]

        [DllImport(ProgrammerDll, EntryPoint = "CpuRun", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void CpuRunC();

        internal static void CpuRun()
        {
            try
            {
                CpuRunC();
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

        #region [CpuStep]

        [DllImport(ProgrammerDll, EntryPoint = "CpuStep", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void CpuStepC();

        internal static void CpuStep()
        {
            try
            {
                CpuStepC();
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
