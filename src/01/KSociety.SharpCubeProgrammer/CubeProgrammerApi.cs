// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using DeviceDataStructure;
    using Enum;
    using Interface;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using SharpCubeProgrammer.Native;
    using Struct;

    public partial class CubeProgrammerApi : ICubeProgrammerApi
    {
        private readonly ILogger<CubeProgrammerApi> _logger;

        private const int DisposedFlag = 1;
        private int _isDisposed;

        private readonly ProgrammerInstanceApi _programmerInstanceApi;

        #region [Constructor]

        public CubeProgrammerApi(ILogger<CubeProgrammerApi> logger = default)
        {
            if (logger == null)
            {
                logger = new NullLogger<CubeProgrammerApi>();
            }

            this._logger = logger;
            this._programmerInstanceApi = new ProgrammerInstanceApi();
            this._programmerInstanceApi.EnsureNativeLibraryLoaded();
            this._programmerInstanceApi.LoadDefaultLoaders();
        }

        #endregion

        #region [ST-LINK]

        //ST-LINK module groups debug ports JTAG/SWD functions together.

        /// <inheritdoc />
        public CubeProgrammerError TryConnectStLink(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            var listPtr = new IntPtr();
            var parametersList = new List<DebugConnectParameters>();

            try
            {
                var size = Marshal.SizeOf<DebugConnectParameters>();
                var numberOfItems = this._programmerInstanceApi.GetStLinkEnumerationList(ref listPtr, shared);
                if (listPtr != IntPtr.Zero)
                {
                    for (var i = 0; i < numberOfItems; i++)
                    {
                        var currentItem = Marshal.PtrToStructure<DebugConnectParameters>(listPtr + (i * size));
                        parametersList.Add(currentItem);
                        Marshal.DestroyStructure<DebugConnectParameters>(listPtr + (i * size));
                    }

                    if (numberOfItems > 0 && stLinkProbeIndex < numberOfItems)
                    {
                        var dbg = parametersList[stLinkProbeIndex];
                        dbg.ConnectionMode = debugConnectMode;
                        dbg.Shared = shared;

                        var connectStLinkResult = this._programmerInstanceApi.ConnectStLink(dbg);
                        if (connectStLinkResult != 0)
                        {
                            this.Disconnect();
                                
                        }
                        output = this.CheckResult(connectStLinkResult);
                    }
                }
                else
                {
                    this._logger?.LogWarning("TryConnectStLink IntPtr: {0}!", "Zero");
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
                var size = Marshal.SizeOf<DebugConnectParameters>();
                var numberOfItems = this._programmerInstanceApi.GetStLinkList(ref listPtr, shared ? 1 : 0);
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
                var size = Marshal.SizeOf<DebugConnectParameters>();
                var numberOfItems = this._programmerInstanceApi.GetStLinkEnumerationList(ref listPtr, shared ? 1 : 0);
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
                var connectStLinkResult = this._programmerInstanceApi.ConnectStLink(debugConnectParameters);

                output = this.CheckResult(connectStLinkResult);
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
                var resetResult = this._programmerInstanceApi.Reset(rstMode);
                output = this.CheckResult(resetResult);
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "Reset: ");
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
                var numberOfItems = this._programmerInstanceApi.GetUsartList(ref listPtr);
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
                var connectUsartResult = this._programmerInstanceApi.ConnectUsartBootloader(usartConnectParameters);

                output = this.CheckResult(connectUsartResult);
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
                var connectUsartResult = this._programmerInstanceApi.SendByteUart(bytes);

                output = this.CheckResult(connectUsartResult);
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "SendByteUart: ");
            }
            return output;
        }

        /// <inheritdoc />
        public IEnumerable<DfuDeviceInfo> GetDfuDeviceList(int iPID = 0xdf11, int iVID = 0x0483)
        {
            var listPtr = new IntPtr();
            var dfuDeviceList = new List<DfuDeviceInfo>();
            try
            {
                var size = Marshal.SizeOf<DfuDeviceInfo>();
                var numberOfItems = this._programmerInstanceApi.GetDfuDeviceList(ref listPtr, iPID, iVID);

                if (listPtr != IntPtr.Zero)
                {
                    for (var i = 0; i < numberOfItems; i++)
                    {
                        var currentItem = Marshal.PtrToStructure<DfuDeviceInfo>(listPtr + (i * size));
                        dfuDeviceList.Add(currentItem);
                        Marshal.DestroyStructure<DfuDeviceInfo>(listPtr + (i * size));
                    }
                }
                else
                {
                    this._logger?.LogWarning("GetDfuDeviceList IntPtr: {0}!", "Zero");
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "GetDfuDeviceList:");
            }

            return dfuDeviceList;
        }

        /// <inheritdoc />
        public CubeProgrammerError ConnectDfuBootloader(string usbIndex)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            try
            {
                var connectDfuBootloaderResult = this._programmerInstanceApi.ConnectDfuBootloader(usbIndex);
                if (connectDfuBootloaderResult != 0)
                {
                    this.Disconnect();
                }

                output = this.CheckResult(connectDfuBootloaderResult);
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
                if ((dfuParameters.rdu == 0x00 || dfuParameters.rdu == 0x01) && (dfuParameters.tzenreg == 0x00 || dfuParameters.tzenreg == 0x01))
                {
                    var connectDfuBootloader2Result = this._programmerInstanceApi.ConnectDfuBootloader2(dfuParameters);
                    if (connectDfuBootloader2Result != 0)
                    {
                        this.Disconnect();
                    }

                    output = this.CheckResult(connectDfuBootloader2Result);
                }
                else
                {
                    this._logger?.LogWarning("ConnectDfuBootloader2 parameters rdu or tzenreg not valid!");
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "ConnectDfuBootloader2: ");
            }
            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError ConnectDfuBootloader2(string usbIndex, byte rdu, byte tzenreg)
        {
            var dfuParameters = new DfuConnectParameters
            {
                usb_index = usbIndex,
                rdu = rdu,
                tzenreg = tzenreg
            };

            return this.ConnectDfuBootloader2(dfuParameters);
        }

        /// <inheritdoc />
        public CubeProgrammerError ConnectSpiBootloader(SpiConnectParameters spiParameters)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            try
            {
                var connectSpiBootloaderResult = this._programmerInstanceApi.ConnectSpiBootloader(spiParameters);
                if (connectSpiBootloaderResult != 0)
                {
                    this.Disconnect();
                }

                output = this.CheckResult(connectSpiBootloaderResult);
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
                var connectCanBootloaderResult = this._programmerInstanceApi.ConnectCanBootloader(canParameters);
                if (connectCanBootloaderResult != 0)
                {
                    this.Disconnect();
                }

                output = this.CheckResult(connectCanBootloaderResult);
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
                var connectI2CBootloaderResult = this._programmerInstanceApi.ConnectI2cBootloader(i2CParameters);
                if (connectI2CBootloaderResult != 0)
                {
                    this.Disconnect();
                }

                output = this.CheckResult(connectI2CBootloaderResult);
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
            this._programmerInstanceApi.DisplayCallBacks.InitProgressBar = initProgressBar;
            this._programmerInstanceApi.DisplayCallBacks.LogMessage = messageReceived;
            this._programmerInstanceApi.DisplayCallBacks.LoadBar = progressBarUpdate;
            this._programmerInstanceApi.SetDisplayCallbacks(this._programmerInstanceApi.DisplayCallBacks);

            return this._programmerInstanceApi.DisplayCallBacks;
        }

        /// <inheritdoc />
        public DisplayCallBacks SetDisplayCallbacks(DisplayCallBacks callbacksHandle)
        {
            this._programmerInstanceApi.DisplayCallBacks.InitProgressBar = callbacksHandle.InitProgressBar;
            this._programmerInstanceApi.DisplayCallBacks.LogMessage = callbacksHandle.LogMessage;
            this._programmerInstanceApi.DisplayCallBacks.LoadBar = callbacksHandle.LoadBar;
            this._programmerInstanceApi.SetDisplayCallbacks(callbacksHandle);

            return this._programmerInstanceApi.DisplayCallBacks;
        }

        /// <inheritdoc />
        public void SetVerbosityLevel(VerbosityLevel level)
        {
            this._programmerInstanceApi.SetVerbosityLevel((int)level);
        }

        /// <inheritdoc />
        public bool CheckDeviceConnection()
        {
            var checkDeviceConnectionResult = this._programmerInstanceApi.CheckDeviceConnection();

            if (checkDeviceConnectionResult == 1)
            {  return true; }

            return false;
        }

        /// <inheritdoc />
        public GeneralInf? GetDeviceGeneralInf()
        {
            GeneralInf? generalInf = null;
            
            try
            {
                var ptr = IntPtr.Zero;
                ptr = this._programmerInstanceApi.GetDeviceGeneralInf();

                if (ptr != IntPtr.Zero)
                {
                    generalInf = Marshal.PtrToStructure<GeneralInf>(ptr);
                    Marshal.DestroyStructure<GeneralInf>(ptr);
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
                var bufferPtr = IntPtr.Zero;
                var readMemoryResult =
                    this._programmerInstanceApi.ReadMemory(uintAddress, ref bufferPtr, Convert.ToUInt32(byteSize));
                result = this.CheckResult(readMemoryResult);
                if (bufferPtr != IntPtr.Zero && result.Equals(CubeProgrammerError.CubeprogrammerNoError))
                {
                    Marshal.Copy(bufferPtr, buffer, 0, byteSize);
                    this.FreeLibraryMemory(bufferPtr);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "ReadMemory: ");
            }

            return (result, buffer);
        }

        /// <inheritdoc />
        public CubeProgrammerError WriteMemory(string address, byte[] data, int size = 0)
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;

            if (!String.IsNullOrEmpty(address) && data.Length > 0)
            {
                var uintAddress = this.HexConverterToUint(address);
                var length = data.Length;
                if (size != 0 && size <= data.Length)
                {
                    length = size;
                }

                try
                {
                    var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                    var writeMemoryResult = this._programmerInstanceApi.WriteMemory(uintAddress, gch.AddrOfPinnedObject(), Convert.ToUInt32(length));
                    gch.Free();
                    result = this.CheckResult(writeMemoryResult);

                    return result;
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
                    var remainder = data.Length % 8;

                    if (remainder > 0)
                    {
                        var filling = 8 - remainder;
                        var newSize = data.Length + filling;
                        Array.Resize(ref data, newSize);

                        for (var i = data.Length - filling; i < data.Length; i++)
                        {
                            data[i] = 0xFF;
                        }
                    }

                    var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                    var writeMemoryResult = this._programmerInstanceApi.WriteMemory(uintAddress, gch.AddrOfPinnedObject(), (uint)data.Length);
                    gch.Free();
                    result = this.CheckResult(writeMemoryResult);

                    return result;
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
                    var remainder = data.Length % 8;

                    if (remainder > 0)
                    {
                        var filling = 8 - remainder;
                        var newSize = data.Length + filling;
                        Array.Resize(ref data, newSize);

                        for (var i = data.Length - filling; i < data.Length; i++)
                        {
                            data[i] = 0xFF;
                        }
                    }

                    var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                    var writeMemoryResult = this._programmerInstanceApi.WriteMemory(uintAddress, gch.AddrOfPinnedObject(), (uint)data.Length);
                    gch.Free();
                    result = this.CheckResult(writeMemoryResult);

                    if (result.Equals(CubeProgrammerError.CubeprogrammerNoError))
                    {
                        var bufferPtr = IntPtr.Zero;
                        var buffer = new byte[data.Length];
                        var readMemoryResult =
                            this._programmerInstanceApi.ReadMemory(uintAddress, ref bufferPtr, (uint)data.Length);
                        result = this.CheckResult(readMemoryResult);

                        if (bufferPtr != IntPtr.Zero && result.Equals(CubeProgrammerError.CubeprogrammerNoError))
                        {
                            Marshal.Copy(bufferPtr, buffer, 0, data.Length);
                            this.FreeLibraryMemory(bufferPtr);

                            if (data.SequenceEqual(buffer))
                            {
                                
                                result = CubeProgrammerError.CubeprogrammerNoError;
                            }
                            else
                            {
                                result = CubeProgrammerError.CubeprogrammerErrorWriteMem;
                            }
                        }
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
                var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                var writeMemoryResult = this._programmerInstanceApi.EditSector(uintAddress, gch.AddrOfPinnedObject(), (uint)data.Length);
                gch.Free();
                result = this.CheckResult(writeMemoryResult);

                return result;
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
                var downloadFileResult = this._programmerInstanceApi.DownloadFile(
                filePathAdapted,
                uintAddress,
                skipErase,
                verify,
                binPathAdapted
                );
                output = this.CheckResult(downloadFileResult);
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
                var uintAddress = this.HexConverterToUint(address);
                var executeResult = this._programmerInstanceApi.Execute(uintAddress);

                output = this.CheckResult(executeResult);
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
            var massEraseResult = this._programmerInstanceApi.MassErase(sFlashMemName);
            var output = this.CheckResult(massEraseResult);
            
            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError SectorErase(uint[] sectors, uint sectorNbr, string sFlashMemName = "")
        {
            var sectorEraseResult = this._programmerInstanceApi.SectorErase(sectors, sectorNbr, sFlashMemName);
            var output = this.CheckResult(sectorEraseResult);

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError ReadUnprotect()
        {
            var result = this._programmerInstanceApi.ReadUnprotect();
            var output = this.CheckResult(result);

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError TzenRegression()
        {
            var result = this._programmerInstanceApi.TzenRegression();
            var output = this.CheckResult(result);

            return output;
        }

        /// <inheritdoc />
        public TargetInterfaceType? GetTargetInterfaceType()
        {
            var result = this._programmerInstanceApi.GetTargetInterfaceType();

            if (result == -1)
            {
                return null;
            }

            return (TargetInterfaceType)result;
        }

        /// <inheritdoc />
        public int GetCancelPointer()
        {
            return this._programmerInstanceApi.GetCancelPointer();
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
                    filePointer = this._programmerInstanceApi.FileOpen(filePathAdapted);
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
                    var filePathAdapted = filePath.Replace(@"\", "/");

                    return this._programmerInstanceApi.FileOpen(filePathAdapted);
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
            if (data != IntPtr.Zero)
            {
                this._programmerInstanceApi.FreeFileData(data);
            }
        }

        /// <inheritdoc />
        public void FreeLibraryMemory(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
            {
                this._programmerInstanceApi.FreeLibraryMemory(ptr);
            }
        }

        /// <inheritdoc />
        public CubeProgrammerError Verify(IntPtr fileData, string address)
        {
            var uintAddress = this.HexConverterToUint(address);

            var verifyResult = this._programmerInstanceApi.Verify(fileData, uintAddress);
            var output = this.CheckResult(verifyResult);
            
            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError VerifyMemory(string address, byte[] data)
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;
            var buffer = new byte[data.Length];
            var bufferPtr = IntPtr.Zero;

            if (!String.IsNullOrEmpty(address) && data.Length > 0)
            {
                var uintAddress = this.HexConverterToUint(address);
                var readMemoryResult =
                    this._programmerInstanceApi.ReadMemory(uintAddress, ref bufferPtr, (uint)data.Length);
                result = this.CheckResult(readMemoryResult);
                if (bufferPtr != IntPtr.Zero && result.Equals(CubeProgrammerError.CubeprogrammerNoError))
                {
                    Marshal.Copy(bufferPtr, buffer, 0, data.Length);
                    this.FreeLibraryMemory(bufferPtr);

                    if (data.SequenceEqual(buffer))
                    {
                        result = CubeProgrammerError.CubeprogrammerNoError;
                    }
                    else
                    {
                        result = CubeProgrammerError.CubeprogrammerErrorOther;
                    }
                }

                return result;
            }

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError VerifyMemoryBySegment(string address, byte[] data)
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;

            if (!String.IsNullOrEmpty(address) && data.Length > 0)
            {
                var intAddress = this.HexConverterToInt(address);
                var uintAddress = this.HexConverterToUint(address);
                var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                
                var segmetData = new SegmentDataC
                {
                    address = intAddress,
                    size = data.Length,
                    data = gch.AddrOfPinnedObject()
                };

                var segmentDataGch = GCHandle.Alloc(segmetData, GCHandleType.Pinned);

                var fileData = new FileDataC
                {
                    Type = 0,
                    segmentsNbr = 1,
                    segments = segmentDataGch.AddrOfPinnedObject()
                };

                var fileDataGch = GCHandle.Alloc(fileData, GCHandleType.Pinned);

                var verifyMemoryResult =
                    this._programmerInstanceApi.Verify(fileDataGch.AddrOfPinnedObject(), uintAddress);
                gch.Free();
                segmentDataGch.Free();
                fileDataGch.Free();

                result = this.CheckResult(verifyMemoryResult);

                return result;
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
                var saveFileToFileResult = this._programmerInstanceApi.SaveFileToFile(fileData, sFileNameAdapted);
                output = this.CheckResult(saveFileToFileResult);
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
                var saveMemoryToFileResult =
                this._programmerInstanceApi.SaveMemoryToFile(intAddress, intSize, fileNameAdapted);

                output = this.CheckResult(saveMemoryToFileResult);
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
            var result = this._programmerInstanceApi.Disconnect();
            var output = this.CheckResult(result);
            
            return output;
        }

        /// <inheritdoc />
        public void DeleteInterfaceList()
        {
            this._programmerInstanceApi.DeleteInterfaceList();
        }

        /// <inheritdoc />
        public void AutomaticMode(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1)
        {
            if (!String.IsNullOrEmpty(filePath))
            {
                var filePathAdapted = filePath.Replace(@"\", "/");
                if (!String.IsNullOrEmpty(address))
                {
                    var uintAddress = this.HexConverterToUint(address);

                    var obCommandPtr = Marshal.StringToHGlobalAnsi(obCommand);
                    this._programmerInstanceApi.AutomaticMode(filePathAdapted, uintAddress, skipErase, verify, isMassErase, obCommandPtr, run);
                    Marshal.FreeHGlobal(obCommandPtr);
                }
            }
        }

        /// <inheritdoc />
        public void SerialNumberingAutomaticMode(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1, int enableSerialNumbering = 0, int serialAddress = 0, int serialSize = 0, string serialInitialData = "")
        {
            if (!String.IsNullOrEmpty(filePath))
            {
                var filePathAdapted = filePath.Replace(@"\", "/");
                if (!String.IsNullOrEmpty(address))
                {
                    var uintAddress = this.HexConverterToUint(address);
                    var obCommandPtr = Marshal.StringToHGlobalAnsi(obCommand);
                    this._programmerInstanceApi.SerialNumberingAutomaticMode(filePathAdapted, uintAddress, skipErase, verify, isMassErase, obCommandPtr, run, enableSerialNumbering, serialAddress, serialSize, serialInitialData);
                    Marshal.FreeHGlobal(obCommandPtr);
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
                var result = this._programmerInstanceApi.GetStorageStructure(ref storageStructurePtr);

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
            var result = this._programmerInstanceApi.SendOptionBytesCmd(command);
            var output = this.CheckResult(result);
            
            return output;
        }

        /// <inheritdoc />
        public DevicePeripheralC? InitOptionBytesInterface()
        {
            var pointer = this._programmerInstanceApi.InitOptionBytesInterface();
            return pointer != IntPtr.Zero ? this.DevicePeripheralCHandler(pointer) : null;
        }

        /// <inheritdoc />
        public DevicePeripheralC? FastRomInitOptionBytesInterface(ushort deviceId)
        {
            var pointer = this._programmerInstanceApi.FastRomInitOptionBytesInterface(deviceId);
            return pointer != IntPtr.Zero ? this.DevicePeripheralCHandler(pointer) : null;
        }

        /// <inheritdoc />
        private DevicePeripheralC? DevicePeripheralCHandler(IntPtr pointer)
        {
            var pointerSize = Marshal.SizeOf<IntPtr>();

            try
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
                var obDisplayResult = this._programmerInstanceApi.ObDisplay();

                output = this.CheckResult(obDisplayResult);
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
            var pathAdapted = path.Replace(@"\", "/");
            this._programmerInstanceApi.SetLoadersPath(pathAdapted);
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
                this._programmerInstanceApi.SetExternalLoaderPath(pathAdapted, ref externalLoaderPtr);
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
                this._programmerInstanceApi.SetExternalLoaderOBL(pathAdapted, ref externalLoaderPtr);
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
                var result = this._programmerInstanceApi.GetExternalLoaders(pathAdapted, ref externalStorageInfoPtr);
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
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "GetExternalLoaders: ");
            }

            return null;
        }

        /// <inheritdoc />
        public void RemoveExternalLoader(string path)
        {
            var pathAdapted = path.Replace(@"\", "/");
            this._programmerInstanceApi.RemoveExternalLoader(pathAdapted);
        }

        /// <inheritdoc />
        public void DeleteLoaders()
        {
            this._programmerInstanceApi.DeleteLoaders();
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
            var getUID64Result = this._programmerInstanceApi.GetUID64(ref bufferPtr);

            var result = this.CheckResult(getUID64Result);
            if (result.Equals(CubeProgrammerError.CubeprogrammerNoError) && bufferPtr != IntPtr.Zero)
            {
                Marshal.Copy(bufferPtr, buffer, 0, 8);
            }

            return (result, buffer);
        }

        /// <inheritdoc />
        public bool FirmwareDelete()
        {
            return this._programmerInstanceApi.FirmwareDelete();
        }

        /// <inheritdoc />
        public bool FirmwareUpgrade(string filePath, string address, WbFunctionArguments firstInstall, WbFunctionArguments startStack, WbFunctionArguments verify)
        {
            var uintAddress = this.HexConverterToUint(address);
            var filePathAdapted = String.IsNullOrEmpty(filePath) ? "" : filePath.Replace(@"\", "/");

            return this._programmerInstanceApi.FirmwareUpgrade(filePathAdapted, uintAddress, (uint)firstInstall, (uint)startStack, (uint)verify);
        }

        /// <inheritdoc />
        public bool StartWirelessStack()
        {
            return this._programmerInstanceApi.StartWirelessStack();
        }

        /// <inheritdoc />
        public bool UpdateAuthKey(string filePath)
        {
            return this._programmerInstanceApi.UpdateAuthKey(filePath);
        }

        /// <inheritdoc />
        public CubeProgrammerError AuthKeyLock()
        {
            var authKeyLockResult = this._programmerInstanceApi.AuthKeyLock();
            var result = this.CheckResult(authKeyLockResult);

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError WriteUserKey(string filePath, byte keyType)
        {
            var filePathAdapted = String.IsNullOrEmpty(filePath) ? "" : filePath.Replace(@"\", "/");
            var writeUserKeyResult = this._programmerInstanceApi.WriteUserKey(filePathAdapted, keyType);
            var result = this.CheckResult(writeUserKeyResult);

            return result;
        }

        /// <inheritdoc />
        public bool AntiRollBack()
        {
            return this._programmerInstanceApi.AntiRollBack();
        }

        /// <inheritdoc />
        public bool StartFus()
        {
            return this._programmerInstanceApi.StartFus();
        }

        /// <inheritdoc />
        public CubeProgrammerError UnlockChip()
        {
            var unlockChipResult = this._programmerInstanceApi.UnlockChip();
            var result = this.CheckResult(unlockChipResult);

            return result;
        }

        #endregion

        #region [STM32MP specific functions]

        /// <inheritdoc />
        public CubeProgrammerError ProgramSsp(string sspFile, string licenseFile, string tfaFile, int hsmSlotId)
        {
            var sspFileAdapted = String.IsNullOrEmpty(sspFile) ? "" : sspFile.Replace(@"\", "/");
            var licenseFileAdapted = String.IsNullOrEmpty(licenseFile) ? "" : licenseFile.Replace(@"\", "/");
            var tfaFileAdapted = String.IsNullOrEmpty(tfaFile) ? "" : tfaFile.Replace(@"\", "/");
            var programSspResult = this._programmerInstanceApi.ProgramSsp(sspFileAdapted, licenseFileAdapted, tfaFileAdapted, hsmSlotId);

            var result = this.CheckResult(programSspResult);

            return result;
        }

        #endregion

        #region [STM32 HSM specific functions]

        /// <inheritdoc />
        public string GetHsmFirmwareID(int hsmSlotId)
        {
            return this._programmerInstanceApi.GetHsmFirmwareID(hsmSlotId);
        }

        /// <inheritdoc />
        public ulong GetHsmCounter(int hsmSlotId)
        {
            return this._programmerInstanceApi.GetHsmCounter(hsmSlotId);
        }

        /// <inheritdoc />
        public string GetHsmState(int hsmSlotId)
        {
            return this._programmerInstanceApi.GetHsmState(hsmSlotId);
        }

        /// <inheritdoc />
        public string GetHsmVersion(int hsmSlotId)
        {
            return this._programmerInstanceApi.GetHsmVersion(hsmSlotId);
        }

        /// <inheritdoc />
        public string GetHsmType(int hsmSlotId)
        {
            return this._programmerInstanceApi.GetHsmType(hsmSlotId);
        }

        /// <inheritdoc />
        public CubeProgrammerError GetHsmLicense(int hsmSlotId, string outLicensePath)
        {
            var outLicensePathAdapted = String.IsNullOrEmpty(outLicensePath) ? "" : outLicensePath.Replace(@"\", "/");
            var getHsmLicenseResult = this._programmerInstanceApi.GetHsmLicense(hsmSlotId, outLicensePathAdapted);

            var result = this.CheckResult(getHsmLicenseResult);

            return result;
        }

        #endregion

        #region [EXTENDED]

        public void Halt()
        {
            this._programmerInstanceApi.CpuHalt();
        }

        public void Run()
        {
            this._programmerInstanceApi.CpuRun();
        }

        public void Step()
        {
            this._programmerInstanceApi.CpuStep();
        }

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
                this._programmerInstanceApi.Dispose();
            }

            // Free any unmanaged objects here.
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
