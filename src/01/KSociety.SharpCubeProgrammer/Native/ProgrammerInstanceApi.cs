// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Native
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Struct;

    internal sealed partial class ProgrammerInstanceApi : IDisposable
    {
        private const int DisposedFlag = 1;
        private int _isDisposed;

        private readonly object SyncRoot = new object();

        internal volatile SafeLibraryHandle HandleSTLinkDriver;
        internal volatile SafeLibraryHandle HandleProgrammer;

        internal volatile IntPtr HandleSTLinkDriverIntPtr;
        internal volatile IntPtr HandleProgrammerIntPtr;

        internal DisplayCallBacks DisplayCallBacks = new DisplayCallBacks();
        internal DisplayCallBacksLinux DisplayCallBacksLinux = new DisplayCallBacksLinux();

        #region [Constructor]

        internal ProgrammerInstanceApi()
        {
            var libraryLoaded = this.EnsureNativeLibraryLoaded();

            if (libraryLoaded)
            {
                this.LoadDefaultLoaders();
            }
            else
            {
                throw new InvalidOperationException("K-Society ProgrammerInstanceApi: Native library could not be loaded.");
            }
        }

        #endregion

        #region [Init]

        private void LoadDefaultLoaders()
        {
            var currentDirectory = this.GetAssemblyDirectory();
            var target = Path.Combine(currentDirectory, "st", "Programmer");
            var targetAdapted = target.Replace(@"\", "/");
            this.SetLoadersPath(targetAdapted);
        }

        private bool EnsureNativeLibraryLoaded()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (this.HandleSTLinkDriver != null && this.HandleProgrammer != null)
                {
                    return true;
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (this.HandleSTLinkDriverIntPtr != IntPtr.Zero && this.HandleProgrammerIntPtr != IntPtr.Zero)
                {
                    return true;
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return false;
            }

            var currentDirectory = this.GetAssemblyDirectory();
            var target = String.Empty;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                target = Path.Combine(currentDirectory, "dll", Environment.Is64BitProcess ? "x64" : "x86");
            }else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                target = Path.Combine(currentDirectory, "so", "lib");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                target = Path.Combine(currentDirectory, "dylib", Environment.Is64BitProcess ? "x64" : "x86");
            }

            if (String.IsNullOrEmpty(target))
            {
                return false;
            }
            else
            {
                try
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        var stLinkDriverResult = this.LoadStLinkDriver(target);

                        if (stLinkDriverResult != null)
                        {
                            var programmerResult = this.LoadProgrammer(target);

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
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        var stLinkDriverResult = this.LoadStLinkDriverLinux(target);
                        if (stLinkDriverResult != IntPtr.Zero)
                        {
                            var programmerResult = this.LoadProgrammerLinux(target);
                            if (programmerResult != IntPtr.Zero)
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
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("K-Society ProgrammerInstanceApi native library loading error!", ex);
                }
            }
        }

        private SafeLibraryHandle LoadStLinkDriver(string target)
        {
            if (this.HandleSTLinkDriver == null)
            {
                lock (this.SyncRoot)
                {
                    if (this.HandleSTLinkDriver == null)
                    {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            // Check if the local machine has KB2533623 installed in order
                            // to use the more secure flags when calling LoadLibraryEx
                            bool hasKB2533623;

                            using (var hModule = WindowsUtility.LoadLibraryEx(WindowsUtility.KernelLibName, IntPtr.Zero, 0))
                            {
                                // If the AddDllDirectory function is found then the flags are supported.
                                hasKB2533623 = WindowsUtility.GetProcAddress(hModule, "AddDllDirectory") != IntPtr.Zero;
                            }

                            var dwFlags = 0;

                            if (hasKB2533623)
                            {
                                // If KB2533623 is installed then specify the more secure LOAD_LIBRARY_SEARCH_DEFAULT_DIRS in dwFlags.
                                dwFlags = WindowsUtility.LOAD_LIBRARY_SEARCH_DEFAULT_DIRS;
                            }

                            this.HandleSTLinkDriver = WindowsUtility.LoadLibraryEx(target + @"\STLinkUSBDriver.dll", IntPtr.Zero, dwFlags);

                            if (this.HandleSTLinkDriver.IsInvalid)
                            {
                                var error = Marshal.GetLastWin32Error();
                                this.HandleSTLinkDriver = null;

                                throw new Exception("K-Society ProgrammerInstanceApi StLinkDriver loading error: " + error);
                            }
                        }
                    }
                }
            }

            return this.HandleSTLinkDriver;
        }

        private IntPtr LoadStLinkDriverLinux(string target)
        {
            if (this.HandleSTLinkDriverIntPtr == IntPtr.Zero)
            {
                lock (this.SyncRoot)
                {
                    if (this.HandleSTLinkDriverIntPtr == IntPtr.Zero)
                    {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        {
                            this.HandleSTLinkDriverIntPtr = LinuxUtility.DlOpen(target + "/libSTLinkUSBDriver.so", 0x00102); // 0x00001 == RTLD_LEAZY, 0x00002 == RTLD_NOW, 0x00100 = RTLD_GLOBAL

                            var error = LinuxUtility.DlError();
                            if (error != IntPtr.Zero)
                            {
                                var message = Marshal.PtrToStringAnsi(error);
                                throw new Exception("K-Society ProgrammerInstanceApi StLinkDriver Linux loading error: " + message + " !");
                            }

                            if (this.HandleSTLinkDriverIntPtr == IntPtr.Zero)
                            {
                                throw new Exception("K-Society ProgrammerInstanceApi StLinkDriver Linux loading error: IntPtr.Zero ");
                            }
                        }
                    }
                }
            }

            return this.HandleSTLinkDriverIntPtr;
        }

        private SafeLibraryHandle LoadProgrammer(string target)
        {
            if (this.HandleProgrammer == null)
            {
                lock (this.SyncRoot)
                {
                    if (this.HandleProgrammer == null)
                    {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            var dwFlags = WindowsUtility.LOAD_WITH_ALTERED_SEARCH_PATH;

                            this.HandleProgrammer = WindowsUtility.LoadLibraryEx(target + @"\CubeProgrammer_API.dll", IntPtr.Zero, dwFlags);

                            if (this.HandleProgrammer.IsInvalid)
                            {
                                var error = Marshal.GetLastWin32Error();
                                this.HandleProgrammer = null;

                                throw new Exception("K-Society ProgrammerInstanceApi Programmer loading error: " + error);
                            }
                        }
                    }
                }
            }

            return this.HandleProgrammer;
        }

        private IntPtr LoadProgrammerLinux(string target)
        {
            if (this.HandleProgrammerIntPtr == IntPtr.Zero)
            {
                lock (this.SyncRoot)
                {
                    if (this.HandleProgrammerIntPtr == IntPtr.Zero)
                    {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        {
                            this.HandleProgrammerIntPtr = LinuxUtility.DlOpen(target + "/libCubeProgrammer_API.so.1", 0x00102); // 0x00002 == RTLD_NOW, 0x00100 = RTLD_GLOBAL

                            var error = LinuxUtility.DlError();
                            if (error != IntPtr.Zero)
                            {
                                var message = Marshal.PtrToStringAnsi(error);
                                throw new Exception("K-Society ProgrammerInstanceApi Programmer Linux loading error: " + message + " !");
                            }

                            if (this.HandleProgrammerIntPtr == IntPtr.Zero)
                            {
                                throw new Exception("K-Society ProgrammerInstanceApi Programmer Linux loading error: IntPtr.Zero ");
                            }
                        }
                    }
                }
            }

            return this.HandleProgrammerIntPtr;
        }

        private string GetAssemblyDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().Location;
            var uri = new UriBuilder()
            {
                Scheme = Uri.UriSchemeFile,
                Path = codeBase
            };
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        #endregion

        #region [CubeProgrammer API]

        #region [STLINK]

        internal int GetStLinkList(ref IntPtr stLinkList, int shared)
        {
            var function = this.EnsureFunction("getStLinkList", ref this._getStLinkList);

            if (function != null)
            {
                return function(ref stLinkList, shared);
            }

            return -99;
        }

        internal int GetStLinkEnumerationList(ref IntPtr stLinkList, int shared)
        {
            var function = this.EnsureFunction("getStLinkEnumerationList", ref this._getStLinkEnumerationList);

            if (function != null)
            {
                return function(ref stLinkList, shared);
            }
            
            return -99;
        }

        internal int ConnectStLink(DebugConnectParameters debugConnectParameters)
        {
            return this.EnsureFunctionAndInvoke(
                "connectStLink",
                ref this._connectStLink,
                (function) => function(debugConnectParameters));
        }

        internal int Reset(DebugResetMode rstMode)
        {
            return this.EnsureFunctionAndInvoke(
                "reset",
                ref this._reset,
                (function) => function(rstMode));
        }

        #endregion

        #region [Bootloader]

        internal int GetUsartList(ref IntPtr usartList)
        {
            var function = this.EnsureFunction("getUsartList", ref this._getUsartList);

            if (function != null)
            {
                return function(ref usartList);
            }

            return -99;
        }

        internal int ConnectUsartBootloader(UsartConnectParameters usartParameters)
        {
            return this.EnsureFunctionAndInvoke(
                "connectUsartBootloader",
                ref this._connectUsartBootloader,
                (function) => function(usartParameters));
        }

        internal int SendByteUart(int byteToSend)
        {
            return this.EnsureFunctionAndInvoke(
                "sendByteUart",
                ref this._sendByteUart,
                (function) => function(byteToSend));
        }

        internal int GetDfuDeviceList(ref IntPtr dfuList, int iPID, int iVID)
        {
            var function = this.EnsureFunction("getDfuDeviceList", ref this._getDfuDeviceList);

            if (function != null)
            {
                return function(ref dfuList, iPID, iVID);
            }

            return -99;
        }

        internal int ConnectDfuBootloader(string usbIndex)
        {
            var usbIndexPtr = IntPtr.Zero;
            try
            {
                usbIndexPtr = Marshal.StringToHGlobalAnsi(usbIndex);
                if (usbIndexPtr != IntPtr.Zero)
                {
                    var result = this.EnsureFunctionAndInvoke(
                        "connectDfuBootloader",
                        ref this._connectDfuBootloader,
                        (function) => function(usbIndexPtr));

                    return result;
                }
            }
            catch
            {
                // Nothing to do
            }
            finally
            {
                Marshal.FreeHGlobal(usbIndexPtr);
            }
            return -99;
        }

        internal int ConnectDfuBootloader2(DfuConnectParameters dfuParameters)
        {
            return this.EnsureFunctionAndInvoke(
                "connectDfuBootloader2",
                ref this._connectDfuBootloader2,
                (function) => function(dfuParameters));
        }

        internal int ConnectSpiBootloader(SpiConnectParameters spiParameters)
        {
            return this.EnsureFunctionAndInvoke(
                "connectSpiBootloader",
                ref this._connectSpiBootloader,
                (function) => function(spiParameters));
        }

        internal int ConnectCanBootloader(CanConnectParameters canParameters)
        {
            return this.EnsureFunctionAndInvoke(
                "connectCanBootloader",
                ref this._connectCanBootloader,
                (function) => function(canParameters));
        }

        internal int ConnectI2cBootloader(I2cConnectParameters i2cParameters)
        {
            return this.EnsureFunctionAndInvoke(
                "connectI2cBootloader",
                ref this._connectI2cBootloader,
                (function) => function(i2cParameters));
        }

        #endregion

        #region [General purposes]

        internal void SetDisplayCallbacks(DisplayCallBacks c)
        {
            this.EnsureFunctionAndInvoke(
            "setDisplayCallbacks",
            ref this._setDisplayCallbacks,
            (function) => function(c));
        }

        internal void SetDisplayCallbacks(DisplayCallBacksLinux c)
        {
            this.EnsureFunctionAndInvoke(
            "setDisplayCallbacks",
            ref this._setDisplayCallbacksLinux,
            (function) => function(c));
        }

        internal void SetVerbosityLevel(int level)
        {
            this.EnsureFunctionAndInvoke(
            "setVerbosityLevel",
            ref this._setVerbosityLevel,
            (function) => function(level));
        }

        internal int CheckDeviceConnection()
        {
            return this.EnsureFunctionAndInvoke(
                "checkDeviceConnection",
                ref this._checkDeviceConnection,
                (function) => function());
        }

        internal IntPtr GetDeviceGeneralInf()
        {
            return this.EnsureFunctionAndInvoke(
                "getDeviceGeneralInf",
                ref this._getDeviceGeneralInf,
                (function) => function());
        }

        internal int ReadMemory(uint address, ref IntPtr data, uint size)
        {
            var function = this.EnsureFunction("readMemory", ref this._readMemory);

            if (function != null)
            {
                return function(address, ref data, size);
            }

            return -99;
        }

        internal int WriteMemory(uint address, IntPtr data, uint size)
        {
            if (data != IntPtr.Zero)
            {
                return this.EnsureFunctionAndInvoke(
                "writeMemory",
                ref this._writeMemory,
                (function) => function(address, data, size));
            }

            return -99;
        }

        internal int EditSector(uint address, IntPtr data, uint size)
        {
            if (data != IntPtr.Zero)
            {
                return this.EnsureFunctionAndInvoke(
                    "editSector",
                    ref this._editSector,
                    (function) => function(address, data, size));
            }

            return -99;
        }

        internal int DownloadFile(string filePath, uint address, uint skipErase, uint verify, string binPath)
        {
            return this.EnsureFunctionAndInvoke(
                "downloadFile",
                ref this._downloadFile,
                (function) => function(filePath, address, skipErase, verify, binPath));
        }

        internal int Execute(uint address)
        {
            return this.EnsureFunctionAndInvoke(
                "execute",
                ref this._execute,
                (function) => function(address));
        }

        internal int MassErase(string sFlashMemName)
        {
            var sFlashMemNamePtr = IntPtr.Zero;

            try
            {
                sFlashMemNamePtr = Marshal.StringToHGlobalAnsi(sFlashMemName);
                if (sFlashMemNamePtr != IntPtr.Zero)
                {
                    var result = this.EnsureFunctionAndInvoke(
                    "massErase",
                    ref this._massErase,
                    (function) => function(sFlashMemNamePtr));
                    
                    return result;
                }
            }catch
            {
                // Nothing to do
            }
            finally
            {
                if (sFlashMemNamePtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(sFlashMemNamePtr);
                }
            }

            return -99;
        }

        internal int SectorErase(uint[] sectors, uint sectorNbr, string sFlashMemName)
        {
            var sFlashMemNamePtr = IntPtr.Zero;
            try
            {
                sFlashMemNamePtr = Marshal.StringToHGlobalAnsi(sFlashMemName);
                if (sFlashMemNamePtr != IntPtr.Zero)
                {
                    var result = this.EnsureFunctionAndInvoke(
                    "sectorErase",
                    ref this._sectorErase,
                    (function) => function(sectors, sectorNbr, sFlashMemNamePtr));
                    
                    return result;
                }
            }catch
            {
                // Nothing to do
            }
            finally
            {
                if (sFlashMemNamePtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(sFlashMemNamePtr);
                }
            }

            return -99;
        }

        internal int ReadUnprotect()
        {
            return this.EnsureFunctionAndInvoke(
                "readUnprotect",
                ref this._readUnprotect,
                (function) => function());
        }

        internal int TzenRegression()
        {
            return this.EnsureFunctionAndInvoke(
                "tzenRegression",
                ref this._tzenRegression,
                (function) => function());
        }

        internal int GetTargetInterfaceType()
        {
            return this.EnsureFunctionAndInvoke(
                "getTargetInterfaceType",
                ref this._getTargetInterfaceType,
                (function) => function());
        }

        internal int GetCancelPointer()
        {
            return this.EnsureFunctionAndInvoke(
                "getCancelPointer",
                ref this._getCancelPointer,
                (function) => function());
        }

        internal IntPtr FileOpen(string filePath)
        {
            return this.EnsureFunctionAndInvoke(
                "fileOpen",
                ref this._fileOpen,
                (function) => function(filePath));
        }

        internal void FreeFileData(IntPtr data)
        {
            if (data != IntPtr.Zero)
            {
                this.EnsureFunctionAndInvoke(
                "freeFileData",
                ref this._freeFileData,
                (function) => function(data));
            }
        }

        internal void FreeLibraryMemory(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
            {
                this.EnsureFunctionAndInvoke(
                "freeLibraryMemory",
                ref this._freeLibraryMemory,
                (function) => function(ptr));
            }
        }

        internal int Verify(IntPtr fileData, uint address)
        {
            if (fileData != IntPtr.Zero)
            {
                return this.EnsureFunctionAndInvoke(
                "verify",
                ref this._verify,
                (function) => function(fileData, address));
            }

            return -99;
        }

        internal int SaveFileToFile(IntPtr fileData, string sFileName)
        {
            if (fileData != IntPtr.Zero)
            {
                return this.EnsureFunctionAndInvoke(
                    "saveFileToFile",
                    ref this._saveFileToFile,
                    (function) => function(fileData, sFileName));
            }

            return -99;
        }

        internal int SaveMemoryToFile(int address, int size, string sFileName)
        {
            return this.EnsureFunctionAndInvoke(
                "saveMemoryToFile",
                ref this._saveMemoryToFile,
                (function) => function(address, size, sFileName));
        }

        internal int Disconnect()
        {
            return this.EnsureFunctionAndInvoke(
                "disconnect",
                ref this._disconnect,
                (function) => function());
        }

        internal void DeleteInterfaceList()
        {
            this.EnsureFunctionAndInvoke(
                "deleteInterfaceList",
                ref this._deleteInterfaceList,
                (function) => function());
        }

        internal void AutomaticMode(string filePath, uint address, uint skipErase, uint verify, int isMassErase, IntPtr obCommand, int run)
        {
            this.EnsureFunctionAndInvoke(
                "automaticMode",
                ref this._automaticMode,
                (function) => function(filePath, address, skipErase, verify, isMassErase, obCommand, run));
        }

        internal void SerialNumberingAutomaticMode(string filePath, uint address, uint skipErase, uint verify, int isMassErase, IntPtr obCommand, int run, int enableSerialNumbering, int serialAddress, int serialSize, string serialInitialData)
        {
            this.EnsureFunctionAndInvoke(
                "serialNumberingAutomaticMode",
                ref this._serialNumberingAutomaticMode,
                (function) => function(filePath, address, skipErase, verify, isMassErase, obCommand, run, enableSerialNumbering, serialAddress, serialSize, serialInitialData));
        }

        internal int GetStorageStructure(ref IntPtr deviceStorageStruct)
        {
            var function = this.EnsureFunction("getStorageStructure", ref this._getStorageStructure);

            if (function != null)
            {
                return function(ref deviceStorageStruct);
            }

            return -99;
        }

        #endregion

        #region [Option Bytes functions]
        internal int SendOptionBytesCmd(string command)
        {
            var commandPtr = IntPtr.Zero;
            try
            {
                commandPtr = Marshal.StringToHGlobalAnsi(command);
                if (commandPtr != IntPtr.Zero)
                {
                    var result = this.EnsureFunctionAndInvoke(
                    "sendOptionBytesCmd",
                    ref this._sendOptionBytesCmd,
                    (function) => function(commandPtr));
                    
                    return result;
                }
            }catch
            {
                // Nothing to do
            }
            finally
            {
                Marshal.FreeHGlobal(commandPtr);
            }

            return -99;
        }

        internal IntPtr InitOptionBytesInterface()
        {
            return this.EnsureFunctionAndInvoke(
                "initOptionBytesInterface",
                ref this._initOptionBytesInterface,
                (function) => function());
        }

        internal IntPtr FastRomInitOptionBytesInterface(ushort deviceId)
        {
            return this.EnsureFunctionAndInvoke(
                "fastRomInitOptionBytesInterface",
                ref this._fastRomInitOptionBytesInterface,
                (function) => function(deviceId));
        }

        internal int ObDisplay()
        {
            return this.EnsureFunctionAndInvoke(
                "obDisplay",
                ref this._obDisplay,
                (function) => function());
        }

        #endregion

        #region [Loaders functions]

        internal void SetLoadersPath(string path)
        {
            this.EnsureFunctionAndInvoke(
            "setLoadersPath",
            ref this._setLoadersPath,
            (function) => function(path));
        }

        internal void SetExternalLoaderPath(string path, ref IntPtr externalLoaderInfo)
        {
            var function = this.EnsureFunction("setExternalLoaderPath", ref this._setExternalLoaderPath);

            function?.Invoke(path, ref externalLoaderInfo);
        }

        internal void SetExternalLoaderOBL(string path, ref IntPtr externalLoaderInfo)
        {
            var function = this.EnsureFunction("setExternalLoaderOBL", ref this._setExternalLoaderOBL);

            function?.Invoke(path, ref externalLoaderInfo);
        }

        internal int GetExternalLoaders(string path, ref IntPtr externalStorageNfo)
        {
            var function = this.EnsureFunction("getExternalLoaders", ref this._getExternalLoaders);

            if (function != null)
            {
                return function(path, ref externalStorageNfo);
            }
            
            return -99;
        }

        internal void RemoveExternalLoader(string path)
        {
            this.EnsureFunctionAndInvoke(
            "removeExternalLoader",
            ref this._removeExternalLoader,
            (function) => function(path));
        }

        internal void DeleteLoaders()
        {
            this.EnsureFunctionAndInvoke(
            "deleteLoaders",
            ref this._deleteLoaders,
            (function) => function());
        }

        #endregion

        #region [STM32WB specific functions]

        internal int GetUID64(ref IntPtr data)
        {
            var function = this.EnsureFunction("getUID64", ref this._getUID64);

            if (function != null)
            {
                return function(ref data);
            }

            return -99;
        }

        internal bool FirmwareDelete()
        {
            return this.EnsureFunctionAndInvoke(
                "firmwareDelete",
                ref this._firmwareDelete,
                (function) => function());
        }

        internal bool FirmwareUpgrade(string filePath, uint address, uint firstInstall, uint startStack, uint verify)
        {
            return this.EnsureFunctionAndInvoke(
                "firmwareUpgrade",
                ref this._firmwareUpgrade,
                (function) => function(filePath, address, firstInstall, startStack, verify));
        }

        internal bool StartWirelessStack()
        {
            return this.EnsureFunctionAndInvoke(
                "startWirelessStack",
                ref this._startWirelessStack,
                (function) => function());
        }

        internal bool UpdateAuthKey(string filePath)
        {
            return this.EnsureFunctionAndInvoke(
                "updateAuthKey",
                ref this._updateAuthKey,
                (function) => function(filePath));
        }

        internal int AuthKeyLock()
        {
            return this.EnsureFunctionAndInvoke(
                "authKeyLock",
                ref this._authKeyLock,
                (function) => function());
        }

        internal int WriteUserKey(string filePath, byte keyType)
        {
            return this.EnsureFunctionAndInvoke(
                "writeUserKey",
                ref this._writeUserKey,
                (function) => function(filePath, keyType));
        }

        internal bool AntiRollBack()
        {
            return this.EnsureFunctionAndInvoke(
                "antiRollBack",
                ref this._antiRollBack,
                (function) => function());
        }

        internal bool StartFus()
        {
            return this.EnsureFunctionAndInvoke(
                "startFus",
                ref this._startFus,
                (function) => function());
        }

        internal int UnlockChip()
        {
            return this.EnsureFunctionAndInvoke(
                "unlockchip",
                ref this._unlockChip,
                (function) => function());
        }

        #endregion

        #region [STM32MP specific functions]

        internal int ProgramSsp(string sspFile, string licenseFile, string tfaFile, int hsmSlotId)
        {
            return this.EnsureFunctionAndInvoke(
                "programSsp",
                ref this._programSsp,
                (function) => function(sspFile, licenseFile, tfaFile, hsmSlotId));
        }

        #endregion

        #region [STM32 HSM specific functions]

        internal string GetHsmFirmwareID(int hsmSlotId)
        {
            return this.EnsureFunctionAndInvoke(
                "getHsmFirmwareID",
                ref this._getHsmFirmwareID,
                (function) => function(hsmSlotId));
        }

        internal ulong GetHsmCounter(int hsmSlotId)
        {
            return this.EnsureFunctionAndInvoke(
                "getHsmCounter",
                ref this._getHsmCounter,
                (function) => function(hsmSlotId));
        }

        internal string GetHsmState(int hsmSlotId)
        {
            return this.EnsureFunctionAndInvoke(
                "getHsmState",
                ref this._getHsmState,
                (function) => function(hsmSlotId));
        }

        internal string GetHsmVersion(int hsmSlotId)
        {
            return this.EnsureFunctionAndInvoke(
                "getHsmVersion",
                ref this._getHsmVersion,
                (function) => function(hsmSlotId));
        }

        internal string GetHsmType(int hsmSlotId)
        {
            return this.EnsureFunctionAndInvoke(
                "getHsmType",
                ref this._getHsmType,
                (function) => function(hsmSlotId));
        }

        internal int GetHsmLicense(int hsmSlotId, string outLicensePath)
        {
            return this.EnsureFunctionAndInvoke(
                "getHsmLicense",
                ref this._getHsmLicense,
                (function) => function(hsmSlotId, outLicensePath));
        }

        #endregion

        #region [EXTENDED]

        internal void CpuHalt()
        {
            this.EnsureFunctionAndInvoke(
                "Halt",
                ref this._cpuHalt,
                (function) => function());
        }

        internal void CpuRun()
        {
            this.EnsureFunctionAndInvoke(
                "Run",
                ref this._cpuRun,
                (function) => function());
        }

        internal void CpuStep()
        {
            this.EnsureFunctionAndInvoke(
                "Step",
                ref this._cpuStep,
                (function) => function());
        }

        #endregion

        #endregion

        #region [Utils]

        private T EnsureFunction<T>(string functionName, ref T function)
            where T : class, Delegate
        {
            if (function == null)
            {
                lock (this.SyncRoot)
                {
                    if (function == null)
                    {
                        function = this.GetDelegate<T>(functionName);
                    }
                }
            }

            return function;
        }

        private void EnsureFunctionAndInvoke<T>(string functionName, ref T function, Action<T> callback)
            where T : class, Delegate
        {
            function = this.EnsureFunction(functionName, ref function);

            if (function != null)
            {
                callback(function);
            }
        }

        private bool EnsureFunctionAndInvoke<T>(string functionName, ref T function, Func<T, bool> callback)
            where T : class, Delegate
        {
            function = this.EnsureFunction(functionName, ref function);

            if (function != null)
            {
                return callback(function);
            }

            return false;
        }

        private int EnsureFunctionAndInvoke<T>(string functionName, ref T function, Func<T, int> callback)
            where T : class, Delegate
        {
            function = this.EnsureFunction(functionName, ref function);

            if (function != null)
            {
                return callback(function);
            }

            return -99;
        }

        private ulong EnsureFunctionAndInvoke<T>(string functionName, ref T function, Func<T, ulong> callback)
            where T : class, Delegate
        {
            function = this.EnsureFunction(functionName, ref function);

            if (function != null)
            {
                return callback(function);
            }

            return 0UL;
        }

        private string EnsureFunctionAndInvoke<T>(string functionName, ref T function, Func<T, string> callback)
            where T : class, Delegate
        {
            function = this.EnsureFunction(functionName, ref function);

            if (function != null)
            {
                return callback(function);
            }

            return String.Empty;
        }

        private IntPtr EnsureFunctionAndInvoke<T>(string functionName, ref T function, Func<T, IntPtr> callback)
            where T : class, Delegate
        {
            function = this.EnsureFunction(functionName, ref function);

            if (function != null)
            {
                return callback(function);
            }

            return IntPtr.Zero;
        }

        private T GetDelegate<T>(string functionName)
            where T : class, Delegate
        {
            if (this.EnsureNativeLibraryLoaded())
            {
                var address = IntPtr.Zero;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    if (this.HandleProgrammer == null)
                    {
                        return null;
                    }
                    address = WindowsUtility.GetProcAddress(this.HandleProgrammer, functionName);
                }else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    if (this.HandleProgrammerIntPtr == IntPtr.Zero)
                    {
                        return null;
                    }
                    address = LinuxUtility.DlSym(this.HandleProgrammerIntPtr, functionName);
                }

                if (address == IntPtr.Zero)
                {
                    return null;
                }

                return Marshal.GetDelegateForFunctionPointer<T>(address);
            }

            return null;
        }

        #endregion

        #region [Dispose]

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Dispose is implemented correctly, FxCop just doesn't see it.")]
        public void Dispose()
        {
            var wasDisposed = Interlocked.Exchange(ref this._isDisposed, DisposedFlag);
            if (wasDisposed == DisposedFlag)
            {
                return;
            }

            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        internal void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free any other managed objects here.
                
            }

            this.DeleteInterfaceList();

            // Free any unmanaged objects here.

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (this.HandleProgrammer != null)
                {
                    this.HandleProgrammer?.Dispose();
                    this.HandleProgrammer = null;
                }

                if (this.HandleSTLinkDriver != null)
                {
                    this.HandleSTLinkDriver?.Dispose();
                    this.HandleSTLinkDriver = null;
                }
            }else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (this.HandleProgrammerIntPtr != IntPtr.Zero)
                {
                    LinuxUtility.DlClose(this.HandleProgrammerIntPtr);
                    this.HandleProgrammerIntPtr = IntPtr.Zero;
                }

                if (this.HandleSTLinkDriverIntPtr != IntPtr.Zero)
                {
                    LinuxUtility.DlClose(this.HandleSTLinkDriverIntPtr);
                    this.HandleSTLinkDriverIntPtr = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current instance has been disposed.
        /// </summary>
        internal bool IsDisposed
        {
            get
            {
                Interlocked.MemoryBarrier();
                return this._isDisposed == DisposedFlag;
            }
        }

        #endregion

        #region [Destructor]

        ~ProgrammerInstanceApi()
        {
            this.Dispose(false);
        }

        #endregion
    }
}
