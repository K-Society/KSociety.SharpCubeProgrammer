// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using DeviceDataStructure;
    using Enum;
    using Interface;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Struct;

    public partial class CubeProgrammerApi : ICubeProgrammerApi
    {
        private readonly ILogger<CubeProgrammerApi> _logger;

        private const int DisposedFlag = 1;
        private int _isDisposed;

        #region [Constructor]

        public CubeProgrammerApi(ILogger<CubeProgrammerApi> logger = default)
        {
            if (logger == null)
            {
                logger = new NullLogger<CubeProgrammerApi>();
            }

            this._logger = logger;
        }

        #endregion

        #region [ST-LINK]

        //ST-LINK module groups debug ports JTAG/SWD functions together.

        /// <inheritdoc />
        public CubeProgrammerError TryConnectStLink(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;

            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var connectStLinkResult = Native.ProgrammerApi.TryConnectStLink(stLinkProbeIndex, shared, debugConnectMode);

                    output = this.CheckResult(connectStLinkResult);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "TryConnectStLink: ");
            }

            return output;
        }

        /// <inheritdoc />
        public IEnumerable<DebugConnectParameters> GetStLinkList(bool shared = false)
        {
            var listPtr = new IntPtr();
            var parametersList = new List<DebugConnectParameters>();

            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var size = Marshal.SizeOf<DebugConnectParameters>();
                    var numberOfItems = Native.ProgrammerApi.GetStLinkList(ref listPtr, shared ? 1 : 0);
                    if (listPtr != IntPtr.Zero)
                    {
                        for (var i = 0; i < numberOfItems; i++)
                        {
                            var currentItem = Marshal.PtrToStructure<DebugConnectParameters>(listPtr + (i * size));
                            parametersList.Add(currentItem);
                            Marshal.DestroyStructure<DebugConnectParameters>(listPtr + (i * size));
                        }
                    }
                    else
                    {
                        this._logger?.LogWarning("GetStLinkList IntPtr: {0}!", "Zero");
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "GetStLinkList: ");
            }

            return parametersList;
        }

        /// <inheritdoc />
        public IEnumerable<DebugConnectParameters> GetStLinkEnumerationList(bool shared = false)
        {
            var listPtr = new IntPtr();
            var parametersList = new List<DebugConnectParameters>();

            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var size = Marshal.SizeOf<DebugConnectParameters>();
                    var numberOfItems = Native.ProgrammerApi.GetStLinkEnumerationList(ref listPtr, shared ? 1 : 0);
                    if (listPtr != IntPtr.Zero)
                    {
                        for (var i = 0; i < numberOfItems; i++)
                        {
                            var currentItem = Marshal.PtrToStructure<DebugConnectParameters>(listPtr + (i * size));
                            parametersList.Add(currentItem);
                            Marshal.DestroyStructure<DebugConnectParameters>(listPtr + (i * size));
                        }
                    }
                    else
                    {
                        this._logger?.LogWarning("GetStLinkEnumerationList IntPtr: {0}!", "Zero");
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "GetStLinkEnumerationList: ");
            }

            return parametersList;
        }

        /// <inheritdoc />
        public CubeProgrammerError ConnectStLink(DebugConnectParameters debugConnectParameters)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;

            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var connectStLinkResult = Native.ProgrammerApi.ConnectStLink(debugConnectParameters);

                    output = this.CheckResult(connectStLinkResult);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "ConnectStLink: ");
            }

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError Reset(DebugResetMode rstMode)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;

            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var resetResult = Native.ProgrammerApi.Reset(rstMode);
                    output = this.CheckResult(resetResult);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "ConnectStLink: ");
            }
            return output;
        }

        #endregion

        #region [Bootloader]

        //Bootloader module is a way to group Serial interfaces USB/UART/SPI/I2C/CAN function together.

        /// <inheritdoc />
        public IEnumerable<UsartConnectParameters> GetUsartList()
        {
            var listPtr = new IntPtr();
            var parametersList = new List<UsartConnectParameters>();
            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var numberOfItems = Native.ProgrammerApi.GetUsartList(ref listPtr);
                    if (numberOfItems > 0)
                    {
                        if (listPtr != IntPtr.Zero)
                        {
                            var size = Marshal.SizeOf<UsartConnectParameters>();
                            for (var i = 0; i < numberOfItems; i++)
                            {
                                var currentItem = Marshal.PtrToStructure<UsartConnectParameters>(listPtr + (i * size));
                                parametersList.Add(currentItem);
                                Marshal.DestroyStructure<UsartConnectParameters>(listPtr + (i * size));
                            }
                        }
                        else
                        {
                            this._logger?.LogWarning("GetUsartList IntPtr: {0}!", "Zero");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "GetUsartList: ");
            }
            return parametersList;
        }

        /// <inheritdoc />
        public CubeProgrammerError ConnectUsartBootloader(UsartConnectParameters usartConnectParameters)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;

            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var connectUsartResult = Native.ProgrammerApi.ConnectUsartBootloader(usartConnectParameters);

                    output = this.CheckResult(connectUsartResult);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "ConnectUsartBootloader: ");
            }

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError SendByteUart(int bytes)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var connectUsartResult = Native.ProgrammerApi.SendByteUart(bytes);

                    output = this.CheckResult(connectUsartResult);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "SendByteUart: ");
            }
            return output;
        }

        /// <inheritdoc />
        public int GetDfuDeviceList(ref List<DfuDeviceInfo> dfuDeviceList)
        {
            var numberOfItems = 0;
            var listPtr = new IntPtr();

            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var size = Marshal.SizeOf<DfuDeviceInfo>();
                    numberOfItems = Native.ProgrammerApi.GetDfuDeviceList(ref listPtr, 0xdf11, 0x0483);

                    if (listPtr != IntPtr.Zero)
                    {
                        for (var i = 0; i < numberOfItems; i++)
                        {
                            var currentItem = Marshal.PtrToStructure<DfuDeviceInfo>(listPtr + (i * size));
                            dfuDeviceList.Add(currentItem);
                        }
                    }
                    else
                    {
                        this._logger?.LogWarning("GetDfuDeviceList IntPtr: {0}!", "Zero");
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "GetDfuDeviceList:");
            }

            return numberOfItems;
        }

        /// <inheritdoc />
        public CubeProgrammerError ConnectDfuBootloader(string usbIndex)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var connectDfuBootloaderResult = Native.ProgrammerApi.ConnectDfuBootloader(usbIndex);
                    if (connectDfuBootloaderResult != 0)
                    {
                        this.Disconnect();
                    }

                    output = this.CheckResult(connectDfuBootloaderResult);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "ConnectDfuBootloader: ");
            }

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError ConnectDfuBootloader2(DfuConnectParameters dfuParameters)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var connectDfuBootloader2Result = Native.ProgrammerApi.ConnectDfuBootloader2(dfuParameters);
                    if (connectDfuBootloader2Result != 0)
                    {
                        this.Disconnect();
                    }

                    output = this.CheckResult(connectDfuBootloader2Result);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "ConnectDfuBootloader2: ");
            }
            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError ConnectSpiBootloader(SpiConnectParameters spiParameters)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var connectSpiBootloaderResult = Native.ProgrammerApi.ConnectSpiBootloader(spiParameters);
                    if (connectSpiBootloaderResult != 0)
                    {
                        this.Disconnect();
                    }

                    output = this.CheckResult(connectSpiBootloaderResult);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "ConnectSpiBootloader: ");
            }
            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError ConnectCanBootloader(CanConnectParameters canParameters)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var connectCanBootloaderResult = Native.ProgrammerApi.ConnectCanBootloader(canParameters);
                    if (connectCanBootloaderResult != 0)
                    {
                        this.Disconnect();
                    }

                    output = this.CheckResult(connectCanBootloaderResult);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "ConnectCanBootloader: ");
            }
            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError ConnectI2CBootloader(I2CConnectParameters i2CParameters)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var connectI2CBootloaderResult = Native.ProgrammerApi.ConnectI2cBootloader(i2CParameters);
                    if (connectI2CBootloaderResult != 0)
                    {
                        this.Disconnect();
                    }

                    output = this.CheckResult(connectI2CBootloaderResult);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "ConnectI2CBootloader: ");
            }
            return output;
        }

        #endregion

        #region [General purposes]

        // General module groups general purposes functions used by any interface.

        /// <inheritdoc />
        public DisplayCallBacks SetDisplayCallbacks(InitProgressBar initProgressBar, LogMessageReceived messageReceived, ProgressBarUpdateReceived progressBarUpdate)
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                Native.ProgrammerApi.DisplayCallBacks.InitProgressBar = initProgressBar;
                Native.ProgrammerApi.DisplayCallBacks.LogMessage = messageReceived;
                Native.ProgrammerApi.DisplayCallBacks.LoadBar = progressBarUpdate;
                Native.ProgrammerApi.SetDisplayCallbacks(Native.ProgrammerApi.DisplayCallBacks);
            }

            return Native.ProgrammerApi.DisplayCallBacks;
        }

        /// <inheritdoc />
        public DisplayCallBacks SetDisplayCallbacks(DisplayCallBacks callbacksHandle)
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                Native.ProgrammerApi.DisplayCallBacks = callbacksHandle;
                Native.ProgrammerApi.SetDisplayCallbacks(Native.ProgrammerApi.DisplayCallBacks);
            }

            return Native.ProgrammerApi.DisplayCallBacks;
        }

        /// <inheritdoc />
        public void SetVerbosityLevel(VerbosityLevel level)
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                Native.ProgrammerApi.SetVerbosityLevel((int)level);
            }
        }

        /// <inheritdoc />
        public bool CheckDeviceConnection()
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var checkDeviceConnectionResult = Native.ProgrammerApi.CheckDeviceConnection();
                return checkDeviceConnectionResult;
            }

            return false;
        }

        /// <inheritdoc />
        public GeneralInf? GetDeviceGeneralInf()
        {
            GeneralInf? generalInf = null;
            
            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var pointer = Native.ProgrammerApi.GetDeviceGeneralInf();
                    generalInf = Marshal.PtrToStructure<GeneralInf>(pointer);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "GetDeviceGeneralInf: ");
            }

            return generalInf;
        }

        /// <inheritdoc />
        public (CubeProgrammerError, byte[]) ReadMemory(string address, int byteSize)
        {
            var uintAddress = this.HexConverterToUint(address);
            var result = CubeProgrammerError.CubeprogrammerErrorOther;
            var buffer = new byte[byteSize];

            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var bufferPtr = IntPtr.Zero;
                    var readMemoryResult =
                        Native.ProgrammerApi.ReadMemory(uintAddress, ref bufferPtr, Convert.ToUInt32(byteSize));
                    result = this.CheckResult(readMemoryResult);
                    if (bufferPtr != IntPtr.Zero)
                    {
                        Marshal.Copy(bufferPtr, buffer, 0, byteSize);
                        this.FreeLibraryMemory(bufferPtr);
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "ReadMemory: ");
            }

            return (result, buffer);
        }

        /// <inheritdoc />
        public CubeProgrammerError WriteMemory(string address, byte[] data)
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;

            if (!String.IsNullOrEmpty(address) && data.Length > 0)
            {
                var uintAddress = this.HexConverterToUint(address);

                try
                {
                    if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                    {
                        var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                        var writeMemoryResult = Native.ProgrammerApi.WriteMemory(uintAddress, gch.AddrOfPinnedObject(), (uint)data.Length);
                        gch.Free();
                        result = this.CheckResult(writeMemoryResult);

                        return result;
                    }
                }
                catch (Exception ex)
                {
                    this._logger?.LogError(ex, "WriteMemory: ");
                }
            }

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError WriteMemoryAutoFill(string address, byte[] data)
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;

            if (!String.IsNullOrEmpty(address) && data.Length > 0)
            {
                var uintAddress = this.HexConverterToUint(address);

                try
                {
                    if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                    {
                        var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                        var writeMemoryResult = Native.ProgrammerApi.WriteMemoryAutoFill(uintAddress, gch.AddrOfPinnedObject(), (uint)data.Length);
                        gch.Free();
                        result = this.CheckResult(writeMemoryResult);

                        return result;
                    }
                }
                catch (Exception ex)
                {
                    this._logger?.LogError(ex, "WriteMemoryAutoFill: ");
                }
            }

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError WriteMemoryAndVerify(string address, byte[] data)
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;

            if (!String.IsNullOrEmpty(address) && data.Length > 0)
            {
                var uintAddress = this.HexConverterToUint(address);

                try
                {
                    if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                    {
                        var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                        var writeMemoryResult = Native.ProgrammerApi.WriteMemoryAndVerify(uintAddress, gch.AddrOfPinnedObject(), (uint)data.Length);
                        gch.Free();
                        result = this.CheckResult(writeMemoryResult);

                        return result;
                    }
                }
                catch (Exception ex)
                {
                    this._logger?.LogError(ex, "WriteMemoryAndVerify: ");
                }
            }

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError EditSector(string address, byte[] data)
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;

            if (!String.IsNullOrEmpty(address) && data.Length > 0)
            {
                var uintAddress = this.HexConverterToUint(address);

                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var gch = GCHandle.Alloc(data, GCHandleType.Pinned);

                    var writeMemoryResult = Native.ProgrammerApi.EditSector(uintAddress, gch.AddrOfPinnedObject(), (uint)data.Length);
                    gch.Free();
                    result = this.CheckResult(writeMemoryResult);

                    return result;
                }
            }

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError DownloadFile(string inputFilePath, string address = "0x08000000", uint skipErase = 0U, uint verify = 1U)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            var extension = Path.GetExtension(inputFilePath);
            var binPath = "";

            string filePath;
            switch (extension)
            {
                case ".hex":
                    filePath = inputFilePath;
                    break;

                case ".bin":
                    filePath = inputFilePath;
                    binPath = inputFilePath;
                    break;

                default:
                    return output;
            }

            var uintAddress = this.HexConverterToUint(address);
            var filePathAdapted = String.IsNullOrEmpty(filePath) ? "" : filePath.Replace(@"\", "/");
            var binPathAdapted = String.IsNullOrEmpty(binPath) ? "" : binPath.Replace(@"\", "/");

            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var downloadFileResult = Native.ProgrammerApi.DownloadFile(
                    filePathAdapted,
                    uintAddress,
                    skipErase,
                    verify,
                    binPathAdapted
                    );
                    output = this.CheckResult(downloadFileResult);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "DownloadFile: ");
            }

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError Execute(string address = "0x08000000")
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;

            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var uintAddress = this.HexConverterToUint(address);
                    var executeResult = Native.ProgrammerApi.Execute(uintAddress);

                    output = this.CheckResult(executeResult);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "Execute: ");
            }

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError MassErase(string sFlashMemName = "")
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var massEraseResult = Native.ProgrammerApi.MassErase(sFlashMemName);
                output = this.CheckResult(massEraseResult);
            }

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError SectorErase(uint[] sectors, uint sectorNbr, string sFlashMemName = "")
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var sectorEraseResult = Native.ProgrammerApi.SectorErase(sectors, sectorNbr, sFlashMemName);
                output = this.CheckResult(sectorEraseResult);
            }

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError ReadUnprotect()
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var result = Native.ProgrammerApi.ReadUnprotect();
                output = this.CheckResult(result);
            }

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError TzenRegression()
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var result = Native.ProgrammerApi.TzenRegression();
                output = this.CheckResult(result);
            }

            return output;
        }

        /// <inheritdoc />
        public TargetInterfaceType? GetTargetInterfaceType()
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var result = Native.ProgrammerApi.GetTargetInterfaceType();

                if (result == -1)
                {
                    return null;
                }

                return (TargetInterfaceType)result;
            }

            return null;
        }

        /// <inheritdoc />
        public int GetCancelPointer()
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                return Native.ProgrammerApi.GetCancelPointer();
            }

            return 0;
        }

        /// <inheritdoc />
        public DeviceFileDataC? FileOpen(string filePath)
        {
            var segmentSize = Marshal.SizeOf<SegmentDataC>();
            var deviceSegmentData = new DeviceFileDataC();
            if (!String.IsNullOrEmpty(filePath))
            {
                var filePathAdapted = filePath.Replace(@"\", "/");
                var filePointer = IntPtr.Zero;
                try
                {
                    if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                    {
                        filePointer = Native.ProgrammerApi.FileOpen(filePathAdapted);
                        if (!filePointer.Equals(IntPtr.Zero))
                        {
                            var fileData = Marshal.PtrToStructure<FileDataC>(filePointer);
                            deviceSegmentData.Type = fileData.Type;
                            deviceSegmentData.segmentsNbr = fileData.segmentsNbr;
                            deviceSegmentData.segments = new List<DeviceSegmentDataC>();

                            if (fileData.segments != IntPtr.Zero)
                            {
                                for (var i = 0; i < fileData.segmentsNbr; i++)
                                {
                                    var deviceSegment = new DeviceSegmentDataC();
                                    var segment =
                                        Marshal.PtrToStructure<SegmentDataC>(fileData.segments + (i * segmentSize));
                                    deviceSegment.address = segment.address;
                                    deviceSegment.size = segment.size;
                                    if (segment.data != IntPtr.Zero)
                                    {
                                        deviceSegment.data = new byte[segment.size];
                                        Marshal.Copy(segment.data, deviceSegment.data, 0, segment.size);
                                    }

                                    deviceSegmentData.segments.Add(deviceSegment);
                                }
                            }

                            return deviceSegmentData;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._logger?.LogError(ex, "FileOpen: ");
                }
                finally
                {
                    if (!filePointer.Equals(IntPtr.Zero))
                    {
                        this.FreeFileData(filePointer);
                    }
                }
            }
            return null;
        }

        /// <inheritdoc />
        public IntPtr FileOpenAsPointer(string filePath)
        {
            if (!String.IsNullOrEmpty(filePath))
            {
                try
                {
                    if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                    {
                        var filePathAdapted = filePath.Replace(@"\", "/");

                        return Native.ProgrammerApi.FileOpen(filePathAdapted);
                    }
                }
                catch (Exception ex)
                {
                    this._logger?.LogError(ex, "FileOpen: ");
                }
            }
            return IntPtr.Zero;
        }

        /// <inheritdoc />
        public void FreeFileData(IntPtr data)
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                if (data != IntPtr.Zero)
                {
                    Native.ProgrammerApi.FreeFileData(data);
                }
            }
        }

        /// <inheritdoc />
        public void FreeLibraryMemory(IntPtr ptr)
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                if (ptr != IntPtr.Zero)
                {
                    Native.ProgrammerApi.FreeLibraryMemory(ptr);
                }
            }
        }

        /// <inheritdoc />
        public CubeProgrammerError Verify(IntPtr fileData, string address)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var uintAddress = this.HexConverterToUint(address);

                var verifyResult = Native.ProgrammerApi.Verify(fileData, uintAddress);
                output = this.CheckResult(verifyResult);
            }

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError VerifyMemory(string address, byte[] data)
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                if (!String.IsNullOrEmpty(address) && data.Length > 0)
                {
                    var uintAddress = this.HexConverterToUint(address);

                    var gch = GCHandle.Alloc(data, GCHandleType.Pinned);

                    var verifyMemoryResult =
                        Native.ProgrammerApi.VerifyMemory(uintAddress, gch.AddrOfPinnedObject(), (uint)data.Length);
                    gch.Free();
                    result = this.CheckResult(verifyMemoryResult);

                    return result;
                }
            }

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError VerifyMemoryBySegment(string address, byte[] data)
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                if (!String.IsNullOrEmpty(address) && data.Length > 0)
                {
                    var uintAddress = this.HexConverterToUint(address);

                    var gch = GCHandle.Alloc(data, GCHandleType.Pinned);

                    var verifyMemoryResult =
                        Native.ProgrammerApi.VerifyMemoryBySegment(uintAddress, gch.AddrOfPinnedObject(), (uint)data.Length);
                    gch.Free();
                    result = this.CheckResult(verifyMemoryResult);

                    return result;
                }
            }

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError SaveFileToFile(IntPtr fileData, string sFileName)
        {
            var sFileNameAdapted = String.IsNullOrEmpty(sFileName) ? "" : sFileName.Replace(@"\", "/");
            var output = CubeProgrammerError.CubeprogrammerErrorOther;

            if (String.IsNullOrEmpty(sFileNameAdapted))
            {
                return output;
            }

            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var saveFileToFileResult = Native.ProgrammerApi.SaveFileToFile(fileData, sFileNameAdapted);
                    output = this.CheckResult(saveFileToFileResult);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "SaveFileToFile: ");
            }

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError SaveMemoryToFile(string address, string size, string fileName)
        {
            var intAddress = this.HexConverterToInt(address);
            var intSize = this.HexConverterToInt(size);
            var fileNameAdapted = String.IsNullOrEmpty(fileName) ? "" : fileName.Replace(@"\", "/");
            var output = CubeProgrammerError.CubeprogrammerErrorOther;

            if (String.IsNullOrEmpty(fileNameAdapted))
            {
                return output;
            }

            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var saveMemoryToFileResult =
                    Native.ProgrammerApi.SaveMemoryToFile(intAddress, intSize, fileNameAdapted);

                    output = this.CheckResult(saveMemoryToFileResult);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "SaveMemoryToFile: ");
            }

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError Disconnect()
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;

            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var result = Native.ProgrammerApi.Disconnect();

                output = this.CheckResult(result);
            }
            return output;
        }

        /// <inheritdoc />
        public void DeleteInterfaceList()
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                Native.ProgrammerApi.DeleteInterfaceList();
            }
        }

        /// <inheritdoc />
        public void AutomaticMode(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1)
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                if (!String.IsNullOrEmpty(filePath))
                {
                    var filePathAdapted = filePath.Replace(@"\", "/");
                    if (!String.IsNullOrEmpty(address))
                    {
                        var uintAddress = this.HexConverterToUint(address);

                        Native.ProgrammerApi.AutomaticMode(filePathAdapted, uintAddress, skipErase, verify, isMassErase, obCommand, run);
                    }
                }
            }
        }

        /// <inheritdoc />
        public void SerialNumberingAutomaticMode(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1, int enableSerialNumbering = 0, int serialAddress = 0, int serialSize = 0, string serialInitialData = "")
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                if (!String.IsNullOrEmpty(filePath))
                {
                    var filePathAdapted = filePath.Replace(@"\", "/");
                    if (!String.IsNullOrEmpty(address))
                    {
                        var uintAddress = this.HexConverterToUint(address);

                        Native.ProgrammerApi.SerialNumberingAutomaticMode(filePathAdapted, uintAddress, skipErase, verify, isMassErase, obCommand, run, enableSerialNumbering, serialAddress, serialSize, serialInitialData);
                    }
                }
            }
        }

        /// <inheritdoc />
        public (CubeProgrammerError, DeviceStorageStructure) GetStorageStructure()
        {
            var deviceStorageStructure = new DeviceStorageStructure();
            var deviceBankSize = Marshal.SizeOf<DeviceBank>();
            var bankSectorSize = Marshal.SizeOf<BankSector>();
            var storageStructurePtr = new IntPtr();

            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var result = Native.ProgrammerApi.GetStorageStructure(ref storageStructurePtr);

                    output = this.CheckResult(result);

                    if (output.Equals(CubeProgrammerError.CubeprogrammerNoError))
                    {
                        if (storageStructurePtr != IntPtr.Zero)
                        {
                            var storageStructure = Marshal.PtrToStructure<StorageStructure>(storageStructurePtr);

                            deviceStorageStructure.BanksNumber = storageStructure.BanksNumber;
                            var deviceBankList = new List<DeviceDeviceBank>();
                            for (var i = 0; i < storageStructure.BanksNumber; i++)
                            {

                                if (storageStructure.Banks != IntPtr.Zero)
                                {
                                    var deviceBank = Marshal.PtrToStructure<DeviceBank>(storageStructure.Banks + (i * deviceBankSize));
                                    var bankSectorList = new List<BankSector>();
                                    if (deviceBank.Sectors != IntPtr.Zero)
                                    {
                                        for (var ii = 0; ii < deviceBank.SectorsNumber; ii++)
                                        {
                                            var bankSector =
                                                Marshal.PtrToStructure<BankSector>(deviceBank.Sectors +
                                                                                   (ii * bankSectorSize));

                                            bankSectorList.Add(bankSector);

                                            Marshal.DestroyStructure<BankSector>(deviceBank.Sectors +
                                                                                 (ii * bankSectorSize));
                                        }
                                    }

                                    var deviceDeviceBank = new DeviceDeviceBank
                                    {
                                        SectorsNumber = deviceBank.SectorsNumber,
                                        Sectors = bankSectorList
                                    };

                                    deviceBankList.Add(deviceDeviceBank);

                                    Marshal.DestroyStructure<DeviceBank>(storageStructure.Banks + (i * deviceBankSize));
                                }
                            }

                            deviceStorageStructure.Banks = deviceBankList;
                            Marshal.DestroyStructure<StorageStructure>(storageStructurePtr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "GetStorageStructure:");
            }

            return (output, deviceStorageStructure);
        }

        #endregion

        #region [Option Bytes]

        //OB module groups option bytes functions used by any interface.

        /// <inheritdoc />
        public CubeProgrammerError SendOptionBytesCmd(string command)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var result = Native.ProgrammerApi.SendOptionBytesCmd(command);
                output = this.CheckResult(result);
            }
            return output;
        }

        /// <inheritdoc />
        public DevicePeripheralC? InitOptionBytesInterface()
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var pointer = Native.ProgrammerApi.InitOptionBytesInterface();

                return pointer != IntPtr.Zero ? this.DevicePeripheralCHandler(pointer) : null;
            }

            return null;
        }

        /// <inheritdoc />
        public DevicePeripheralC? FastRomInitOptionBytesInterface(ushort deviceId)
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var pointer = Native.ProgrammerApi.FastRomInitOptionBytesInterface(deviceId);

                return pointer != IntPtr.Zero ? this.DevicePeripheralCHandler(pointer) : null;
            }

            return null;
        }

        /// <inheritdoc />
        private DevicePeripheralC? DevicePeripheralCHandler(IntPtr pointer)
        {
            var pointerSize = Marshal.SizeOf<IntPtr>();

            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    PeripheralC? peripheralC = Marshal.PtrToStructure<PeripheralC>(pointer);

                    var bankCList = new List<DeviceBankC>();
                    for (var i = 0; i < peripheralC.Value.BanksNbr; i++)
                    {
                        if (peripheralC.Value.Banks != IntPtr.Zero)
                        {
                            var bankCItemPointer = Marshal.ReadIntPtr(peripheralC.Value.Banks + (i * pointerSize));
                            var bankCItem = Marshal.PtrToStructure<BankC>(bankCItemPointer);

                            if (bankCItem.Categories != IntPtr.Zero)
                            {
                                var categoryCList = new List<DeviceCategoryC>();
                                for (var ii = 0; ii < bankCItem.CategoriesNbr; ii++)
                                {
                                    var categoryCItemPointer =
                                        Marshal.ReadIntPtr(bankCItem.Categories + (ii * pointerSize));
                                    var categoryCItem = Marshal.PtrToStructure<CategoryC>(categoryCItemPointer);

                                    if (categoryCItem.Bits != IntPtr.Zero)
                                    {
                                        var bitCList = new List<DeviceBitC>();
                                        for (var iii = 0; iii < categoryCItem.BitsNbr; iii++)
                                        {
                                            var bitCItemPointer =
                                                Marshal.ReadIntPtr(categoryCItem.Bits + (iii * pointerSize));
                                            var bitCItem = Marshal.PtrToStructure<BitC>(bitCItemPointer);

                                            if (bitCItem.Values != IntPtr.Zero)
                                            {
                                                var bitValueCList = new List<BitValueC>();
                                                for (var iiii = 0; iiii < bitCItem.ValuesNbr; iiii++)
                                                {
                                                    var bitValueCItemPointer =
                                                        Marshal.ReadIntPtr(bitCItem.Values + (iiii * pointerSize));
                                                    var bitValueCItem =
                                                        Marshal.PtrToStructure<BitValueC>(bitValueCItemPointer);
                                                    bitValueCList.Add(bitValueCItem);

                                                    Marshal.DestroyStructure<BitValueC>(bitValueCItemPointer);
                                                }

                                                var deviceBitC = new DeviceBitC
                                                {
                                                    Name = bitCItem.Name,
                                                    Description = bitCItem.Description,
                                                    WordOffset = bitCItem.WordOffset,
                                                    BitOffset = bitCItem.BitOffset,
                                                    BitWidth = bitCItem.BitWidth,
                                                    Access = bitCItem.Access,
                                                    ValuesNbr = bitCItem.ValuesNbr,
                                                    Values = bitValueCList,
                                                    Equation = bitCItem.Equation,
                                                    Reference = bitCItem.Reference,
                                                    BitValue = bitCItem.BitValue
                                                };
                                                bitCList.Add(deviceBitC);

                                                Marshal.DestroyStructure<BitC>(bitCItemPointer);
                                            }
                                        }

                                        var deviceCategoryC = new DeviceCategoryC
                                        {
                                            Name = categoryCItem.Name,
                                            BitsNbr = categoryCItem.BitsNbr,
                                            Bits = bitCList
                                        };
                                        categoryCList.Add(deviceCategoryC);
                                        Marshal.DestroyStructure<CategoryC>(categoryCItemPointer);
                                    }
                                }

                                var deviceBankC = new DeviceBankC
                                {
                                    Size = bankCItem.Size,
                                    Address = bankCItem.Address,
                                    Access = bankCItem.Access,
                                    CategoriesNbr = bankCItem.CategoriesNbr,
                                    Categories = categoryCList
                                };
                                bankCList.Add(deviceBankC);
                                Marshal.DestroyStructure<BankC>(bankCItemPointer);
                            }
                        }
                    }

                    var devicePeripheralC = new DevicePeripheralC
                    {
                        Name = peripheralC.Value.Name,
                        Description = peripheralC.Value.Description,
                        BanksNbr = peripheralC.Value.BanksNbr,
                        Banks = bankCList
                    };

                    return devicePeripheralC;
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "DevicePeripheralCHandler: ");
            }
            finally
            {
                Marshal.DestroyStructure<PeripheralC>(pointer);
            }

            return null;
        }

        /// <inheritdoc />
        public CubeProgrammerError ObDisplay()
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var obDisplayResult = Native.ProgrammerApi.ObDisplay();

                    output = this.CheckResult(obDisplayResult);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "ObDisplay: ");
            }

            return output;
        }

        #endregion

        #region [Loaders]

        //Loaders module groups loaders functions.

        /// <inheritdoc />
        public void SetLoadersPath(string path)
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var pathAdapted = path.Replace(@"\", "/");
                Native.ProgrammerApi.SetLoadersPath(pathAdapted);
            }
        }

        /// <inheritdoc />
        public DeviceExternalLoader? SetExternalLoaderPath(string path)
        {
            var pathAdapted = path.Replace(@"\", "/");
            var externalLoaderPtr = new IntPtr();
            var deviceSectorSize = Marshal.SizeOf<DeviceSector>();
            var output = new DeviceExternalLoader();

            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    Native.ProgrammerApi.SetExternalLoaderPath(pathAdapted, ref externalLoaderPtr);
                    if (externalLoaderPtr != IntPtr.Zero)
                    {
                        var externalLoaderStructure = Marshal.PtrToStructure<ExternalLoader>(externalLoaderPtr);

                        output.filePath = externalLoaderStructure.filePath;
                        output.deviceName = externalLoaderStructure.deviceName;
                        output.deviceType = externalLoaderStructure.deviceType;
                        output.deviceStartAddress = externalLoaderStructure.deviceStartAddress;
                        output.deviceSize = externalLoaderStructure.deviceSize;
                        output.pageSize = externalLoaderStructure.pageSize;
                        output.sectorsTypeNbr = externalLoaderStructure.sectorsTypeNbr;

                        if (externalLoaderStructure.sectors != IntPtr.Zero)
                        {
                            var deviceSectorList = new List<DeviceSector>();
                            for (var i = 0; i < externalLoaderStructure.sectorsTypeNbr; i++)
                            {
                                var deviceSectorItem = Marshal.PtrToStructure<DeviceSector>(externalLoaderStructure.sectors + (i * deviceSectorSize));
                                deviceSectorList.Add(deviceSectorItem);
                            }

                            output.sectors = deviceSectorList;

                            return output;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "SetExternalLoaderPath: ");
            }

            return null;
        }

        /// <inheritdoc />
        public DeviceExternalLoader? SetExternalLoaderOBL(string path)
        {
            var pathAdapted = path.Replace(@"\", "/");
            var externalLoaderPtr = new IntPtr();
            var deviceSectorSize = Marshal.SizeOf<DeviceSector>();
            var output = new DeviceExternalLoader();

            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    Native.ProgrammerApi.SetExternalLoaderOBL(pathAdapted, ref externalLoaderPtr);
                    if (externalLoaderPtr != IntPtr.Zero)
                    {
                        var externalLoaderStructure = Marshal.PtrToStructure<ExternalLoader>(externalLoaderPtr);

                        output.filePath = externalLoaderStructure.filePath;
                        output.deviceName = externalLoaderStructure.deviceName;
                        output.deviceType = externalLoaderStructure.deviceType;
                        output.deviceStartAddress = externalLoaderStructure.deviceStartAddress;
                        output.deviceSize = externalLoaderStructure.deviceSize;
                        output.pageSize = externalLoaderStructure.pageSize;
                        output.sectorsTypeNbr = externalLoaderStructure.sectorsTypeNbr;

                        if (externalLoaderStructure.sectors != IntPtr.Zero)
                        {
                            var deviceSectorList = new List<DeviceSector>();
                            for (var i = 0; i < externalLoaderStructure.sectorsTypeNbr; i++)
                            {
                                var deviceSectorItem = Marshal.PtrToStructure<DeviceSector>(externalLoaderStructure.sectors + (i * deviceSectorSize));
                                deviceSectorList.Add(deviceSectorItem);
                            }

                            output.sectors = deviceSectorList;

                            return output;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "SetExternalLoaderOBL: ");
            }

            return null;
        }

        /// <inheritdoc />
        public DeviceExternalStorageInfo? GetExternalLoaders(string path = @".\st\Programmer")
        {
            var pathAdapted = path.Replace(@"\", "/");
            var externalLoaderList = new List<DeviceExternalLoader>();
            var externalStorageInfoPtr = new IntPtr();
            var deviceSectorSize = Marshal.SizeOf<DeviceSector>();
            var output = new DeviceExternalStorageInfo();

            try
            {
                if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
                {
                    var result = Native.ProgrammerApi.GetExternalLoaders(pathAdapted, ref externalStorageInfoPtr);
                    if (result.Equals(0))
                    {
                        var externalLoaderSize = Marshal.SizeOf<ExternalLoader>();
                        var externalStorageInfoStructure = Marshal.PtrToStructure<ExternalStorageInfo>(externalStorageInfoPtr);

                        output.ExternalLoaderNbr = externalStorageInfoStructure.ExternalLoaderNbr;

                        if (externalStorageInfoStructure.ExternalLoader != IntPtr.Zero)
                        {
                            for (var i = 0; i < externalStorageInfoStructure.ExternalLoaderNbr; i++)
                            {
                                var externalLoaderStructure = Marshal.PtrToStructure<ExternalLoader>(externalStorageInfoStructure.ExternalLoader + (i * externalLoaderSize));

                                var deviceExternalLoader = new DeviceExternalLoader
                                {
                                    filePath = externalLoaderStructure.filePath,
                                    deviceName = externalLoaderStructure.deviceName,
                                    deviceType = externalLoaderStructure.deviceType,
                                    deviceStartAddress = externalLoaderStructure.deviceStartAddress,
                                    deviceSize = externalLoaderStructure.deviceSize,
                                    pageSize = externalLoaderStructure.pageSize,
                                    sectorsTypeNbr = externalLoaderStructure.sectorsTypeNbr
                                };

                                if (externalLoaderStructure.sectors != IntPtr.Zero)
                                {
                                    var deviceSectorList = new List<DeviceSector>();
                                    for (var ii = 0; ii < externalLoaderStructure.sectorsTypeNbr; ii++)
                                    {
                                        var deviceSectorItem = Marshal.PtrToStructure<DeviceSector>(externalLoaderStructure.sectors + (ii * deviceSectorSize));
                                        deviceSectorList.Add(deviceSectorItem);
                                    }

                                    deviceExternalLoader.sectors = deviceSectorList;
                                }

                                externalLoaderList.Add(deviceExternalLoader);
                            }

                            output.ExternalLoader = externalLoaderList;

                            return output;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "GetExternalLoaders: ");
            }

            return null;
        }

        /// <inheritdoc />
        public void RemoveExternalLoader(string path)
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var pathAdapted = path.Replace(@"\", "/");
                Native.ProgrammerApi.RemoveExternalLoader(pathAdapted);
            }
        }

        /// <inheritdoc />
        public void DeleteLoaders()
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                Native.ProgrammerApi.DeleteLoaders();
            }
        }

        #endregion

        #region [STM32WB specific]

        /// Specific APIs used exclusively for STM32WB series to manage BLE Stack, and they are available only through USB DFU and UART bootloader interfaces,
        /// except for the "firmwareDelete" and the "firmwareUpgrade", available through USB DFU, UART and SWD interfaces.
        /// Connection under Reset is mandatory.

        /// <inheritdoc />
        public (CubeProgrammerError, byte[]) GetUID64()
        {
            var buffer = new byte[8];
            var bufferPtr = new IntPtr();
            var result = CubeProgrammerError.CubeprogrammerErrorOther;

            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var getUID64Result = Native.ProgrammerApi.GetUID64(ref bufferPtr);

                result = this.CheckResult(getUID64Result);
                if (result.Equals(CubeProgrammerError.CubeprogrammerNoError) && bufferPtr != IntPtr.Zero)
                {
                    Marshal.Copy(bufferPtr, buffer, 0, 8);
                }
            }

            return (result, buffer);
        }

        /// <inheritdoc />
        public CubeProgrammerError FirmwareDelete()
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var firmwareDeleteResult = Native.ProgrammerApi.FirmwareDelete();

                result = this.CheckResult(firmwareDeleteResult);
            }

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError FirmwareUpgrade(string filePath, string address, uint firstInstall, uint startStack, uint verify)
        {
            var uintAddress = this.HexConverterToUint(address);
            var filePathAdapted = String.IsNullOrEmpty(filePath) ? "" : filePath.Replace(@"\", "/");
            var result = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var firmwareUpgradeResult =
                Native.ProgrammerApi.FirmwareUpgrade(filePathAdapted, uintAddress, firstInstall, startStack, verify);

                result = this.CheckResult(firmwareUpgradeResult);
            }

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError StartWirelessStack()
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var startWirelessStackResult = Native.ProgrammerApi.StartWirelessStack();

                result = this.CheckResult(startWirelessStackResult);
            }

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError UpdateAuthKey(string filePath)
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var updateAuthKeyResult = Native.ProgrammerApi.UpdateAuthKey(filePath);

                result = this.CheckResult(updateAuthKeyResult);
            }

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError AuthKeyLock()
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var authKeyLockResult = Native.ProgrammerApi.AuthKeyLock();

                result = this.CheckResult(authKeyLockResult);
            }

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError WriteUserKey(string filePath, byte keyType)
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var filePathAdapted = String.IsNullOrEmpty(filePath) ? "" : filePath.Replace(@"\", "/");

                var writeUserKeyResult = Native.ProgrammerApi.WriteUserKey(filePathAdapted, keyType);

                result = this.CheckResult(writeUserKeyResult);
            }

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError AntiRollBack()
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var antiRollBackResult = Native.ProgrammerApi.AntiRollBack();

                result = this.CheckResult(antiRollBackResult);
            }

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError StartFus()
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var startFusResult = Native.ProgrammerApi.StartFus();

                result = this.CheckResult(startFusResult);
            }

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError UnlockChip()
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var unlockChipResult = Native.ProgrammerApi.UnlockChip();

                result = this.CheckResult(unlockChipResult);
            }

            return result;
        }

        #endregion

        #region [STM32MP specific functions]

        /// <inheritdoc />
        public CubeProgrammerError ProgramSsp(string sspFile, string licenseFile, string tfaFile, int hsmSlotId)
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var sspFileAdapted = String.IsNullOrEmpty(sspFile) ? "" : sspFile.Replace(@"\", "/");
                var licenseFileAdapted = String.IsNullOrEmpty(licenseFile) ? "" : licenseFile.Replace(@"\", "/");
                var tfaFileAdapted = String.IsNullOrEmpty(tfaFile) ? "" : tfaFile.Replace(@"\", "/");
                var programSspResult = Native.ProgrammerApi.ProgramSsp(sspFileAdapted, licenseFileAdapted, tfaFileAdapted, hsmSlotId);

                result = this.CheckResult(programSspResult);
            }

            return result;
        }

        #endregion

        #region [STM32 HSM specific functions]

        /// <inheritdoc />
        public string GetHsmFirmwareID(int hsmSlotId)
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                return Native.ProgrammerApi.GetHsmFirmwareID(hsmSlotId);
            }

            return String.Empty;
        }

        /// <inheritdoc />
        public ulong GetHsmCounter(int hsmSlotId)
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                return Native.ProgrammerApi.GetHsmCounter(hsmSlotId);
            }

            return 0UL;
        }

        /// <inheritdoc />
        public string GetHsmState(int hsmSlotId)
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                return Native.ProgrammerApi.GetHsmState(hsmSlotId);
            }

            return String.Empty;
        }

        /// <inheritdoc />
        public string GetHsmVersion(int hsmSlotId)
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                return Native.ProgrammerApi.GetHsmVersion(hsmSlotId);
            }

            return String.Empty;
        }

        /// <inheritdoc />
        public string GetHsmType(int hsmSlotId)
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                return Native.ProgrammerApi.GetHsmType(hsmSlotId);
            }

            return String.Empty;
        }

        /// <inheritdoc />
        public CubeProgrammerError GetHsmLicense(int hsmSlotId, string outLicensePath)
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                var outLicensePathAdapted = String.IsNullOrEmpty(outLicensePath) ? "" : outLicensePath.Replace(@"\", "/");
                var getHsmLicenseResult = Native.ProgrammerApi.GetHsmLicense(hsmSlotId, outLicensePathAdapted);

                result = this.CheckResult(getHsmLicenseResult);
            }

            return result;
        }

        #endregion

        #region [EXTENDED]

        //public string VersionAPI()
        //{
        //    if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
        //    {
        //        return Native.ProgrammerApi.VersionAPI();
        //    }

        //    return String.Empty;
        //}

        public void Halt()
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                Native.ProgrammerApi.CpuHalt();
            }
        }

        public void Run()
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                Native.ProgrammerApi.CpuRun();
            }
        }

        public void Step()
        {
            if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
            {
                Native.ProgrammerApi.CpuStep();
            }
        }

        //public string WindowsVersion()
        //{
        //    if (Native.ProgrammerApi.EnsureNativeLibraryLoaded())
        //    {
        //        return Native.ProgrammerApi.WindowsVersion();
        //    }

        //    return String.Empty;
        //}

        #endregion

        #region [Util]

        /// <inheritdoc />
        public uint HexConverterToUint(string hex)
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture.NumberFormat;
            var parseResult = UInt32.TryParse(StringFilter(hex), NumberStyles.HexNumber, formatProvider, out var result);

            return parseResult ? result : 0;
        }

        /// <inheritdoc />
        public int HexConverterToInt(string hex)
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture.NumberFormat;
            var parseResult = Int32.TryParse(StringFilter(hex), NumberStyles.HexNumber, formatProvider, out var result);

            return parseResult ? result : 0;
        }

        /// <inheritdoc />
        public string HexConverterToString(uint hex)
        {
            var output = "0x" + hex.ToString("X");
            return output;
        }

        /// <inheritdoc />
        public string HexConverterToString(int hex)
        {
            var output = "0x" + hex.ToString("X");
            return output;
        }

        private static string StringFilter(string hex)
        {
            if (String.IsNullOrEmpty(hex))
            {
                return String.Empty;
            }

            if (hex.StartsWith("0x") || hex.StartsWith("0X"))
            {
                return hex.Split('x', 'X')[1];
            }

            return hex;
        }

        #endregion

        private CubeProgrammerError CheckResult(int result)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;

            try
            {
                if (System.Enum.IsDefined(typeof(CubeProgrammerError), result))
                {
                    output = (CubeProgrammerError) result;
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "CheckResult: ");
            }

            return output;
        }

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
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            if (Native.ProgrammerApi.HandleProgrammer != null)
            {
                Native.ProgrammerApi.HandleProgrammer?.Dispose();
                Native.ProgrammerApi.HandleProgrammer = null;
            }

            if (Native.ProgrammerApi.HandleSTLinkDriver != null)
            {
                Native.ProgrammerApi.HandleSTLinkDriver?.Dispose();
                Native.ProgrammerApi.HandleSTLinkDriver = null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current instance has been disposed.
        /// </summary>
        protected bool IsDisposed
        {
            get
            {
                Interlocked.MemoryBarrier();
                return this._isDisposed == DisposedFlag;
            }
        }

        #endregion

        #region [Destructor]

        ~CubeProgrammerApi()
        {
            this.Dispose(false);
        }

        #endregion
    } // CubeProgrammerApi.
}
