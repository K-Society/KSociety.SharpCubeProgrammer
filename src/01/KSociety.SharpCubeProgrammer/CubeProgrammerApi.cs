// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using DeviceDataStructure;
    using Enumeration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using SharpCubeProgrammer.Abstraction;
    using SharpCubeProgrammer.Native;
    using Struct;

    /// <inheritdoc cref="ICubeProgrammerApi"/>
    public partial class CubeProgrammerApi : ICubeProgrammerApi
    {
        private readonly ILogger<CubeProgrammerApi> _logger;

        private const int DisposedFlag = 1;
        private int _isDisposed;

        private readonly ProgrammerInstanceApi _programmerInstanceApi;

        #region [Constructor]

        /// <summary>
        /// Initializes a new instance of the <see cref="CubeProgrammerApi"/> class.
        /// </summary>
        public CubeProgrammerApi()
        {
            this._logger = new NullLogger<CubeProgrammerApi>();
            this._programmerInstanceApi = new ProgrammerInstanceApi();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CubeProgrammerApi"/> class with a logger.
        /// </summary>
        /// <param name="logger"></param>
        public CubeProgrammerApi(ILogger<CubeProgrammerApi> logger)
        {
            if (logger == null)
            {
                this._logger = new NullLogger<CubeProgrammerApi>();
            }
            else
            {
                this._logger = logger;
            }

            this._programmerInstanceApi = new ProgrammerInstanceApi();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CubeProgrammerApi"/> class with a logger factory.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public CubeProgrammerApi(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                this._logger = new NullLogger<CubeProgrammerApi>();
            }
            else
            {
                this._logger = loggerFactory.CreateLogger<CubeProgrammerApi>();
            }

            this._programmerInstanceApi = new ProgrammerInstanceApi(loggerFactory);
        }

        #endregion

        #region [ST-LINK]

        //ST-LINK module groups debug ports JTAG/SWD functions together.

        public CubeProgrammerError TryConnectStLink(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode)
        {
            using (this._logger?.AppendMethodScope(nameof(TryConnectStLink)))
            {
                var output = CubeProgrammerError.Other;
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
                            output = HexConverterUtil.CheckResult(connectStLinkResult);
                        }
                    }
                    else
                    {
                        this._logger?.WarningMessage("TryConnectStLink IntPtr: Zero!");
                    }
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("TryConnectStLink: ", ex);
                    throw;
                }

                return output;
            }
        }
       
        public IEnumerable<DebugConnectParameters> GetStLinkList(bool shared = false)
        {
            using (this._logger?.AppendMethodScope(nameof(GetStLinkList)))
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
                        this._logger?.WarningMessage("GetStLinkList IntPtr: Zero!");
                    }
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("GetStLinkList: ", ex);
                    throw;
                }

                return parametersList;
            }
        }
      
        public IEnumerable<DebugConnectParameters> GetStLinkEnumerationList(bool shared = false)
        {
            using (this._logger?.AppendMethodScope(nameof(GetStLinkEnumerationList)))
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
                        this._logger?.WarningMessage("GetStLinkEnumerationList IntPtr: Zero!");
                    }
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("GetStLinkEnumerationList: ", ex);
                    throw;
                }

                return parametersList;
            }
        }

        public CubeProgrammerError ConnectStLink(DebugConnectParameters debugConnectParameters)
        {
            using (this._logger?.AppendMethodScope(nameof(ConnectStLink)))
            {
                var output = CubeProgrammerError.Other;

                try
                {
                    var connectStLinkResult = this._programmerInstanceApi.ConnectStLink(debugConnectParameters);

                    output = HexConverterUtil.CheckResult(connectStLinkResult);
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("ConnectStLink: ", ex);
                    throw;
                }

                return output;
            }
        }

        public CubeProgrammerError Reset(DebugResetMode rstMode)
        {
            using (this._logger?.AppendMethodScope(nameof(Reset)))
            {
                var output = CubeProgrammerError.Other;

                try
                {
                    var resetResult = this._programmerInstanceApi.Reset(rstMode);
                    output = HexConverterUtil.CheckResult(resetResult);
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("Reset: ", ex);
                    throw;
                }
                return output;
            }
        }

        #endregion

        #region [Bootloader]

        //Bootloader module is a way to group Serial interfaces USB/UART/SPI/I2C/CAN function together.

        public IEnumerable<UsartConnectParameters> GetUsartList()
        {
            using (this._logger?.AppendMethodScope(nameof(GetUsartList)))
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
                    }
                    else
                    {
                        this._logger?.WarningMessage("GetUsartList numberOfItems: Zero!");
                    }
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("GetUsartList: ", ex);
                    throw;
                }
                return parametersList;
            }
        }

        public CubeProgrammerError ConnectUsartBootloader(UsartConnectParameters usartConnectParameters)
        {
            using (this._logger?.AppendMethodScope(nameof(ConnectUsartBootloader)))
            {
                var output = CubeProgrammerError.Other;

                try
                {
                    var connectUsartResult = this._programmerInstanceApi.ConnectUsartBootloader(usartConnectParameters);

                    output = HexConverterUtil.CheckResult(connectUsartResult);
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("ConnectUsartBootloader: ", ex);
                    throw;
                }

                return output;
            }
        }

        public CubeProgrammerError SendByteUart(int @byte)
        {
            using (this._logger?.AppendMethodScope(nameof(SendByteUart)))
            {
                var output = CubeProgrammerError.Other;
                try
                {
                    var connectUsartResult = this._programmerInstanceApi.SendByteUart(@byte);

                    output = HexConverterUtil.CheckResult(connectUsartResult);
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("SendByteUart: ", ex);
                    throw;
                }
                return output;
            }
        }

        public IEnumerable<DfuDeviceInfo> GetDfuDeviceList(int iPID = 0xdf11, int iVID = 0x0483)
        {
            using (this._logger?.AppendMethodScope(nameof(GetDfuDeviceList)))
            {
                var listPtr = new IntPtr();
                var dfuDeviceList = new List<DfuDeviceInfo>();
                try
                {
                    var size = Marshal.SizeOf<DfuDeviceInfo>();
                    var numberOfItems = this._programmerInstanceApi.GetDfuDeviceList(ref listPtr, iPID, iVID);

                    if (numberOfItems > 0)
                    {
                        if (listPtr != IntPtr.Zero)
                        {
                            for (var i = 0; i < numberOfItems; i++)
                            {
                                var currentItem = Marshal.PtrToStructure<DfuDeviceInfo>(listPtr + (i * size));
                                dfuDeviceList.Add(currentItem);
                                Marshal.DestroyStructure<DfuDeviceInfo>(listPtr + (i * size));
                            }
                        }
                    }
                    else
                    {
                        this._logger?.WarningMessage("GetDfuDeviceList numberOfItems: Zero!");
                    }
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("GetDfuDeviceList: ", ex);
                    throw;
                }

                return dfuDeviceList;
            }
        }

        public CubeProgrammerError ConnectDfuBootloader(string usbIndex)
        {
            using (this._logger?.AppendMethodScope(nameof(ConnectDfuBootloader)))
            {
                var output = CubeProgrammerError.Other;
                try
                {
                    var connectDfuBootloaderResult = this._programmerInstanceApi.ConnectDfuBootloader(usbIndex);
                    if (connectDfuBootloaderResult != 0)
                    {
                        this.Disconnect();
                    }

                    output = HexConverterUtil.CheckResult(connectDfuBootloaderResult);
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("ConnectDfuBootloader: ", ex);
                    throw;
                }

                return output;
            }
        }

        public CubeProgrammerError ConnectDfuBootloader2(DfuConnectParameters dfuParameters)
        {
            using (this._logger?.AppendMethodScope(nameof(ConnectDfuBootloader2)))
            {
                var output = CubeProgrammerError.Other;

                try
                {
                    if ((dfuParameters.RequestReadUnprotect == 0x00 || dfuParameters.RequestReadUnprotect == 0x01) && (dfuParameters.Tzenreg == 0x00 || dfuParameters.Tzenreg == 0x01))
                    {
                        var connectDfuBootloader2Result = this._programmerInstanceApi.ConnectDfuBootloader2(dfuParameters);
                        if (connectDfuBootloader2Result != 0)
                        {
                            this.Disconnect();
                        }

                        output = HexConverterUtil.CheckResult(connectDfuBootloader2Result);
                    }
                    else
                    {
                        this._logger?.WarningMessage("ConnectDfuBootloader2 parameters rdu or tzenreg not valid!");
                    }
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("ConnectDfuBootloader2: ", ex);
                    throw;
                }
                return output;
            }
        }

        public CubeProgrammerError ConnectDfuBootloader2(string usbIndex, byte requestReadUnprotect, byte tzenreg, int usbTimeout = 30000)
        {
            var dfuParameters = new DfuConnectParameters
            {
                UsbIndex = usbIndex,
                RequestReadUnprotect = requestReadUnprotect,
                Tzenreg = tzenreg,
                UsbTimeout = usbTimeout
            };

            return this.ConnectDfuBootloader2(dfuParameters);
        }

        public CubeProgrammerError ConnectSpiBootloader(SpiConnectParameters spiParameters)
        {
            using (this._logger?.AppendMethodScope(nameof(ConnectSpiBootloader)))
            {
                var output = CubeProgrammerError.Other;
                try
                {
                    var connectSpiBootloaderResult = this._programmerInstanceApi.ConnectSpiBootloader(spiParameters);
                    if (connectSpiBootloaderResult != 0)
                    {
                        this.Disconnect();
                    }

                    output = HexConverterUtil.CheckResult(connectSpiBootloaderResult);
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("ConnectSpiBootloader: ", ex);
                    throw;
                }
                return output;
            }
        }

        public CubeProgrammerError ConnectCanBootloader(CanConnectParameters canParameters)
        {
            using (this._logger?.AppendMethodScope(nameof(ConnectCanBootloader)))
            {
                var output = CubeProgrammerError.Other;
                try
                {
                    var connectCanBootloaderResult = this._programmerInstanceApi.ConnectCanBootloader(canParameters);
                    if (connectCanBootloaderResult != 0)
                    {
                        this.Disconnect();
                    }

                    output = HexConverterUtil.CheckResult(connectCanBootloaderResult);
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("ConnectCanBootloader: ", ex);
                    throw;
                }
                return output;
            }
        }

        public CubeProgrammerError ConnectI2CBootloader(I2cConnectParameters i2CParameters)
        {
            using (this._logger?.AppendMethodScope(nameof(ConnectI2CBootloader)))
            {
                var output = CubeProgrammerError.Other;
                try
                {
                    var connectI2CBootloaderResult = this._programmerInstanceApi.ConnectI2cBootloader(i2CParameters);
                    if (connectI2CBootloaderResult != 0)
                    {
                        this.Disconnect();
                    }

                    output = HexConverterUtil.CheckResult(connectI2CBootloaderResult);
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("ConnectI2CBootloader: ", ex);
                    throw;
                }
                return output;
            }
        }

        #endregion

        #region [General purposes]

        // General module groups general purposes functions used by any interface.

        public DisplayCallBacks SetDisplayCallbacks(InitProgressBar initProgressBar, LogMessageReceived messageReceived, ProgressBarUpdateReceived progressBarUpdate)
        {
            this._programmerInstanceApi.DisplayCallBacks.InitProgressBar = initProgressBar;
            this._programmerInstanceApi.DisplayCallBacks.LogMessage = messageReceived;
            this._programmerInstanceApi.DisplayCallBacks.LoadBar = progressBarUpdate;
            this._programmerInstanceApi.SetDisplayCallbacks(this._programmerInstanceApi.DisplayCallBacks);

            return this._programmerInstanceApi.DisplayCallBacks;
        }

        public DisplayCallBacks SetDisplayCallbacks(DisplayCallBacks c)
        {
            this._programmerInstanceApi.DisplayCallBacks.InitProgressBar = c.InitProgressBar;
            this._programmerInstanceApi.DisplayCallBacks.LogMessage = c.LogMessage;
            this._programmerInstanceApi.DisplayCallBacks.LoadBar = c.LoadBar;
            this._programmerInstanceApi.SetDisplayCallbacks(c);

            return this._programmerInstanceApi.DisplayCallBacks;
        }

        public void SetVerbosityLevel(VerbosityLevel level)
        {
            this._programmerInstanceApi.SetVerbosityLevel((int)level);
        }

        public bool CheckDeviceConnection()
        {
            var checkDeviceConnectionResult = this._programmerInstanceApi.CheckDeviceConnection();

            if (checkDeviceConnectionResult == 1)
            {  return true; }

            return false;
        }

        public GeneralInf? GetDeviceGeneralInf()
        {
            using (this._logger?.AppendMethodScope(nameof(GetDeviceGeneralInf)))
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
                    this._logger?.ErrorMessage("GetDeviceGeneralInf: ", ex);
                    throw;
                }

                return generalInf;
            }
        }

        public (CubeProgrammerError, byte[]) ReadMemory(string address, int byteSize)
        {
            using (this._logger?.AppendMethodScope(nameof(ReadMemory)))
            {
                var uintAddress = HexConverterUtil.HexConverterToUint(address);
                var result = CubeProgrammerError.Other;
                var buffer = new byte[byteSize];

                try
                {
                    var bufferPtr = IntPtr.Zero;
                    var readMemoryResult =
                        this._programmerInstanceApi.ReadMemory(uintAddress, ref bufferPtr, Convert.ToUInt32(byteSize));
                    result = HexConverterUtil.CheckResult(readMemoryResult);
                    if (bufferPtr != IntPtr.Zero && result.Equals(CubeProgrammerError.NoError))
                    {
                        Marshal.Copy(bufferPtr, buffer, 0, byteSize);
                        this.FreeLibraryMemory(bufferPtr);
                    }
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("ReadMemory: ", ex);
                    throw;
                }

                return (result, buffer);
            }
        }

        public CubeProgrammerError WriteMemory(string address, byte[] data, int size = 0)
        {
            using (this._logger?.AppendMethodScope(nameof(WriteMemory)))
            {
                var result = CubeProgrammerError.Other;

                if (String.IsNullOrEmpty(address))
                {
                    throw new ArgumentNullException(nameof(address), "Address cannot be null or empty.");
                }

                if (data is null)
                {
                    throw new ArgumentNullException(nameof(data), "Data cannot be null.");
                }

                if (data.Length > 0)
                {
                    var uintAddress = HexConverterUtil.HexConverterToUint(address);
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
                        result = HexConverterUtil.CheckResult(writeMemoryResult);

                        return result;
                    }
                    catch (Exception ex)
                    {
                        this._logger?.ErrorMessage("WriteMemory: ", ex);
                        throw;
                    }
                }
                else
                {
                    this._logger?.WarningMessage("WriteMemory: Data length is Zero!");
                }

                return result;
            }
        }

        public CubeProgrammerError WriteMemoryAutoFill(string address, byte[] data, int size = 0)
        {
            using (this._logger?.AppendMethodScope(nameof(WriteMemory)))
            {
                var result = CubeProgrammerError.Other;

                if (String.IsNullOrEmpty(address))
                {
                    throw new ArgumentNullException(nameof(address), "Address cannot be null or empty.");
                }

                if (data is null)
                {
                    throw new ArgumentNullException(nameof(data), "Data cannot be null.");
                }

                if (data.Length > 0)
                {
                    var uintAddress = HexConverterUtil.HexConverterToUint(address);
                    var length = data.Length;
                    if (size != 0 && size <= data.Length)
                    {
                        length = size;
                    }

                    try
                    {
                        var remainder = length % 8;

                        if (remainder > 0)
                        {
                            var filling = 8 - remainder;
                            var newSize = length + filling;
                            Array.Resize(ref data, newSize);

                            for (var i = length - filling; i < length; i++)
                            {
                                data[i] = 0xFF;
                            }
                        }

                        var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                        var writeMemoryResult = this._programmerInstanceApi.WriteMemory(uintAddress, gch.AddrOfPinnedObject(), (uint)data.Length);
                        gch.Free();
                        result = HexConverterUtil.CheckResult(writeMemoryResult);

                        return result;
                    }
                    catch (Exception ex)
                    {
                        this._logger?.ErrorMessage("WriteMemoryAutoFill: ", ex);
                        throw;
                    }
                }
                else
                {
                    this._logger?.WarningMessage("WriteMemoryAutoFill: Data length is Zero!");
                }

                return result;
            }
        }

        public CubeProgrammerError WriteMemoryAndVerify(string address, byte[] data, int size = 0)
        {
            using (this._logger?.AppendMethodScope(nameof(WriteMemoryAndVerify)))
            {
                var result = CubeProgrammerError.Other;

                if (String.IsNullOrEmpty(address))
                {
                    throw new ArgumentNullException(nameof(address), "Address cannot be null or empty.");
                }

                if (data is null)
                {
                    throw new ArgumentNullException(nameof(data), "Data cannot be null.");
                }

                if (data.Length > 0)
                {
                    var uintAddress = HexConverterUtil.HexConverterToUint(address);
                    var length = data.Length;
                    if (size != 0 && size <= data.Length)
                    {
                        length = size;
                    }

                    try
                    {
                        var remainder = length % 8;

                        if (remainder > 0)
                        {
                            var filling = 8 - remainder;
                            var newSize = length + filling;
                            Array.Resize(ref data, newSize);

                            for (var i = length - filling; i < length; i++)
                            {
                                data[i] = 0xFF;
                            }
                        }

                        var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                        var writeMemoryResult = this._programmerInstanceApi.WriteMemory(uintAddress, gch.AddrOfPinnedObject(), (uint)data.Length);
                        gch.Free();
                        result = HexConverterUtil.CheckResult(writeMemoryResult);

                        if (result.Equals(CubeProgrammerError.NoError))
                        {
                            var bufferPtr = IntPtr.Zero;
                            var buffer = new byte[data.Length];
                            var readMemoryResult =
                                this._programmerInstanceApi.ReadMemory(uintAddress, ref bufferPtr, (uint)data.Length);
                            result = HexConverterUtil.CheckResult(readMemoryResult);

                            if (bufferPtr != IntPtr.Zero && result.Equals(CubeProgrammerError.NoError))
                            {
                                Marshal.Copy(bufferPtr, buffer, 0, data.Length);
                                this.FreeLibraryMemory(bufferPtr);

                                if (data.SequenceEqual(buffer))
                                {

                                    result = CubeProgrammerError.NoError;
                                }
                                else
                                {
                                    result = CubeProgrammerError.WriteMem;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this._logger?.ErrorMessage("WriteMemoryAndVerify: ", ex);
                        throw;
                    }
                }

                return result;
            }
        }

        public CubeProgrammerError EditSector(string address, byte[] data, int size = 0)
        {
            using (this._logger?.AppendMethodScope(nameof(EditSector)))
            {
                var result = CubeProgrammerError.Other;

                if (String.IsNullOrEmpty(address))
                {
                    throw new ArgumentNullException(nameof(address), "Address cannot be null or empty.");
                }

                if (data is null)
                {
                    throw new ArgumentNullException(nameof(data), "Data cannot be null.");
                }

                if (data.Length > 0)
                {
                    var length = data.Length;
                    if (size != 0 && size <= data.Length)
                    {
                        length = size;
                    }

                    var uintAddress = HexConverterUtil.HexConverterToUint(address);
                    var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                    var editSectorResult = this._programmerInstanceApi.EditSector(uintAddress, gch.AddrOfPinnedObject(), Convert.ToUInt32(length));
                    gch.Free();
                    result = HexConverterUtil.CheckResult(editSectorResult);

                    return result;
                }
                else
                {
                    this._logger?.WarningMessage("EditSector: Data length is Zero!");
                }

                return result;
            }
        }

        public CubeProgrammerError DownloadFile(string inputFilePath, string address = "0x08000000", uint skipErase = 0U, uint verify = 1U, string binFilePath = "")
        {
            using (this._logger?.AppendMethodScope(nameof(DownloadFile)))
            {
                var output = CubeProgrammerError.Other;

                var uintAddress = HexConverterUtil.HexConverterToUint(address);
                var filePathAdapted = String.IsNullOrEmpty(inputFilePath) ? "" : inputFilePath.Replace('\\', '/');
                var binPathAdapted = String.IsNullOrEmpty(binFilePath) ? "" : binFilePath.Replace('\\', '/');

                if (!String.IsNullOrEmpty(filePathAdapted))
                {
                    try
                    {
                        var downloadFileResult = this._programmerInstanceApi.DownloadFile(
                        filePathAdapted,
                        uintAddress,
                        skipErase,
                        verify,
                        binPathAdapted
                        );
                        output = HexConverterUtil.CheckResult(downloadFileResult);
                    }
                    catch (Exception ex)
                    {
                        this._logger?.ErrorMessage("DownloadFile: ", ex);
                        throw;
                    }
                }

                return output;
            }
        }

        public CubeProgrammerError Execute(string address = "0x08000000")
        {
            using (this._logger?.AppendMethodScope(nameof(Execute)))
            {
                var output = CubeProgrammerError.Other;

                if (String.IsNullOrEmpty(address))
                {
                    throw new ArgumentNullException(nameof(address), "Address cannot be null or empty.");
                }

                try
                {
                    var uintAddress = HexConverterUtil.HexConverterToUint(address);
                    var executeResult = this._programmerInstanceApi.Execute(uintAddress);

                    output = HexConverterUtil.CheckResult(executeResult);
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("Execute: ", ex);
                    throw;
                }

                return output;
            }
        }

        public CubeProgrammerError MassErase(string sFlashMemName = "")
        {
            using (this._logger?.AppendMethodScope(nameof(MassErase)))
            {
                var massEraseResult = this._programmerInstanceApi.MassErase(sFlashMemName);
                var output = HexConverterUtil.CheckResult(massEraseResult);

                return output;
            }
        }

        public CubeProgrammerError SectorErase(uint[] sectors, uint sectorNbr, string sFlashMemName = "")
        {
            using (this._logger?.AppendMethodScope(nameof(SectorErase)))
            {
                var sectorEraseResult = this._programmerInstanceApi.SectorErase(sectors, sectorNbr, sFlashMemName);
                var output = HexConverterUtil.CheckResult(sectorEraseResult);

                return output;
            }
        }

        public CubeProgrammerError ReadUnprotect()
        {
            using (this._logger?.AppendMethodScope(nameof(ReadUnprotect)))
            {
                var result = this._programmerInstanceApi.ReadUnprotect();
                var output = HexConverterUtil.CheckResult(result);

                return output;
            }
        }

        public CubeProgrammerError TzenRegression()
        {
            using (this._logger?.AppendMethodScope(nameof(TzenRegression)))
            {
                var result = this._programmerInstanceApi.TzenRegression();
                var output = HexConverterUtil.CheckResult(result);

                return output;
            }
        }

        public TargetInterfaceType? GetTargetInterfaceType()
        {
            using (this._logger?.AppendMethodScope(nameof(GetTargetInterfaceType)))
            {
                var result = this._programmerInstanceApi.GetTargetInterfaceType();

                if (result == -1)
                {
                    return null;
                }

                return (TargetInterfaceType)result;
            }
        }

        public int GetCancelPointer()
        {
            using (this._logger?.AppendMethodScope(nameof(GetCancelPointer)))
            {
                return this._programmerInstanceApi.GetCancelPointer();
            }
        }

        public DeviceFileDataC? FileOpen(string filePath)
        {
            using (this._logger?.AppendMethodScope(nameof(FileOpen)))
            {
                if (String.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentNullException(nameof(filePath), "File path cannot be null or empty.");
                }

                var segmentSize = Marshal.SizeOf<SegmentDataC>();
                var deviceSegmentData = new DeviceFileDataC();
                if (!String.IsNullOrEmpty(filePath))
                {
                    var filePathAdapted = filePath.Replace('\\', '/');
                    var filePointer = IntPtr.Zero;
                    try
                    {
                        filePointer = this._programmerInstanceApi.FileOpen(filePathAdapted);
                        if (!filePointer.Equals(IntPtr.Zero))
                        {
                            var fileData = Marshal.PtrToStructure<FileDataC>(filePointer);
                            deviceSegmentData.Type = fileData.Type;
                            deviceSegmentData.SegmentsNumber = fileData.SegmentsNumber;
                            deviceSegmentData.SetSegments(new List<DeviceSegmentDataC>());

                            if (fileData.Segments != IntPtr.Zero)
                            {
                                for (var i = 0; i < fileData.SegmentsNumber; i++)
                                {
                                    var deviceSegment = new DeviceSegmentDataC();
                                    var segment =
                                        Marshal.PtrToStructure<SegmentDataC>(fileData.Segments + (i * segmentSize));
                                    deviceSegment.Address = segment.Address;
                                    deviceSegment.Size = segment.Size;
                                    if (segment.Data != IntPtr.Zero)
                                    {
                                        var data = new byte[segment.Size];
                                        Marshal.Copy(segment.Data, data, 0, segment.Size);
                                        deviceSegment.SetData(data);
                                    }

                                    deviceSegmentData.Segments.Add(deviceSegment);
                                }
                            }

                            return deviceSegmentData;
                        }
                    }
                    catch (Exception ex)
                    {
                        this._logger?.ErrorMessage("FileOpen: ", ex);
                        throw;
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
        }

        public IntPtr FileOpenAsPointer(string filePath)
        {
            using (this._logger?.AppendMethodScope(nameof(FileOpenAsPointer)))
            {
                if (String.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentNullException(nameof(filePath), "File path cannot be null or empty.");
                }

                try
                {
                    var filePathAdapted = filePath.Replace('\\', '/');

                    return this._programmerInstanceApi.FileOpen(filePathAdapted);
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("FileOpen: ", ex);
                    throw;
                }
            }
        }

        public void FreeFileData(IntPtr data)
        {
            using (this._logger?.AppendMethodScope(nameof(FreeFileData)))
            {
                if (data != IntPtr.Zero)
                {
                    this._programmerInstanceApi.FreeFileData(data);
                }
            }
        }

        public void FreeLibraryMemory(IntPtr ptr)
        {
            using (this._logger?.AppendMethodScope(nameof(FreeLibraryMemory)))
            {
                if (ptr != IntPtr.Zero)
                {
                    this._programmerInstanceApi.FreeLibraryMemory(ptr);
                }
            }
        }

        public CubeProgrammerError Verify(IntPtr fileData, string address)
        {
            using (this._logger?.AppendMethodScope(nameof(FreeLibraryMemory)))
            {
                var output = CubeProgrammerError.Other;

                if (String.IsNullOrEmpty(address))
                {
                    throw new ArgumentNullException(nameof(address), "Address cannot be null or empty.");
                }

                var uintAddress = HexConverterUtil.HexConverterToUint(address);

                if (fileData != IntPtr.Zero)
                {
                    var verifyResult = this._programmerInstanceApi.Verify(fileData, uintAddress);
                    output = HexConverterUtil.CheckResult(verifyResult);
                }

                return output;
            }
        }

        public CubeProgrammerError VerifyMemory(string address, byte[] data)
        {
            using (this._logger?.AppendMethodScope(nameof(VerifyMemory)))
            {
                var result = CubeProgrammerError.Other;

                if (String.IsNullOrEmpty(address))
                {
                    throw new ArgumentNullException(nameof(address), "Address cannot be null or empty.");
                }

                if (data is null)
                {
                    throw new ArgumentNullException(nameof(data), "Data cannot be null.");
                }

                var buffer = new byte[data.Length];
                var bufferPtr = IntPtr.Zero;

                if (data.Length > 0)
                {
                    var uintAddress = HexConverterUtil.HexConverterToUint(address);
                    var readMemoryResult =
                        this._programmerInstanceApi.ReadMemory(uintAddress, ref bufferPtr, (uint)data.Length);
                    result = HexConverterUtil.CheckResult(readMemoryResult);
                    if (bufferPtr != IntPtr.Zero && result.Equals(CubeProgrammerError.NoError))
                    {
                        Marshal.Copy(bufferPtr, buffer, 0, data.Length);
                        this.FreeLibraryMemory(bufferPtr);

                        if (data.SequenceEqual(buffer))
                        {
                            result = CubeProgrammerError.NoError;
                        }
                        else
                        {
                            result = CubeProgrammerError.Other;
                        }
                    }

                    return result;
                }

                return result;
            }
        }

        public CubeProgrammerError VerifyMemoryBySegment(string address, byte[] data)
        {
            using (this._logger?.AppendMethodScope(nameof(VerifyMemoryBySegment)))
            {
                var result = CubeProgrammerError.Other;

                if (String.IsNullOrEmpty(address))
                {
                    throw new ArgumentNullException(nameof(address), "Address cannot be null or empty.");
                }

                if (data is null)
                {
                    throw new ArgumentNullException(nameof(data), "Data cannot be null.");
                }

                if (data.Length > 0)
                {
                    var intAddress = HexConverterUtil.HexConverterToInt(address);
                    var uintAddress = HexConverterUtil.HexConverterToUint(address);
                    var gch = GCHandle.Alloc(data, GCHandleType.Pinned);

                    var segmetData = new SegmentDataC
                    {
                        Address = intAddress,
                        Size = data.Length,
                        Data = gch.AddrOfPinnedObject()
                    };

                    var segmentDataGch = GCHandle.Alloc(segmetData, GCHandleType.Pinned);

                    var fileData = new FileDataC
                    {
                        Type = 0,
                        SegmentsNumber = 1,
                        Segments = segmentDataGch.AddrOfPinnedObject()
                    };

                    var fileDataGch = GCHandle.Alloc(fileData, GCHandleType.Pinned);

                    var verifyMemoryResult =
                        this._programmerInstanceApi.Verify(fileDataGch.AddrOfPinnedObject(), uintAddress);
                    gch.Free();
                    segmentDataGch.Free();
                    fileDataGch.Free();

                    result = HexConverterUtil.CheckResult(verifyMemoryResult);

                    return result;
                }

                return result;
            }
        }

        public CubeProgrammerError SaveFileToFile(IntPtr fileData, string sFileName)
        {
            using (this._logger?.AppendMethodScope(nameof(VerifyMemoryBySegment)))
            {
                var sFileNameAdapted = String.IsNullOrEmpty(sFileName) ? "" : sFileName.Replace('\\', '/');
                var output = CubeProgrammerError.Other;

                if (String.IsNullOrEmpty(sFileNameAdapted))
                {
                    return output;
                }

                try
                {
                    var saveFileToFileResult = this._programmerInstanceApi.SaveFileToFile(fileData, sFileNameAdapted);
                    output = HexConverterUtil.CheckResult(saveFileToFileResult);
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("SaveFileToFile: ", ex);
                    throw;
                }

                return output;
            }
        }

        public CubeProgrammerError SaveMemoryToFile(string address, string size, string fileName)
        {
            using (this._logger?.AppendMethodScope(nameof(SaveMemoryToFile)))
            {
                if (String.IsNullOrEmpty(address))
                {
                    throw new ArgumentNullException(nameof(address), "Address cannot be null or empty.");
                }

                if (String.IsNullOrEmpty(size))
                {
                    throw new ArgumentNullException(nameof(size), "Size cannot be null or empty.");
                }

                if (String.IsNullOrEmpty(fileName))
                {
                    throw new ArgumentNullException(nameof(fileName), "File name cannot be null or empty.");
                }

                var intAddress = HexConverterUtil.HexConverterToInt(address);
                var intSize = HexConverterUtil.HexConverterToInt(size);
                var fileNameAdapted = fileName.Replace('\\', '/');
                var output = CubeProgrammerError.Other;

                try
                {
                    var saveMemoryToFileResult =
                    this._programmerInstanceApi.SaveMemoryToFile(intAddress, intSize, fileNameAdapted);

                    output = HexConverterUtil.CheckResult(saveMemoryToFileResult);
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("SaveMemoryToFile: ", ex);
                    throw;
                }

                return output;
            }
        }

        public void Disconnect()
        {
            using (this._logger?.AppendMethodScope(nameof(Disconnect)))
            {
                this._programmerInstanceApi.Disconnect();
            }
        }

        public void DeleteInterfaceList()
        {
            using (this._logger?.AppendMethodScope(nameof(DeleteInterfaceList)))
            {
                this._programmerInstanceApi.DeleteInterfaceList();
            }
        }

        public void AutomaticMode(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1)
        {
            using (this._logger?.AppendMethodScope(nameof(AutomaticMode)))
            {
                if (!String.IsNullOrEmpty(filePath))
                {
                    var filePathAdapted = filePath.Replace('\\', '/');
                    if (!String.IsNullOrEmpty(address))
                    {
                        var uintAddress = HexConverterUtil.HexConverterToUint(address);

                        var obCommandPtr = Marshal.StringToHGlobalAnsi(obCommand);
                        this._programmerInstanceApi.AutomaticMode(filePathAdapted, uintAddress, skipErase, verify, isMassErase, obCommandPtr, run);
                        Marshal.FreeHGlobal(obCommandPtr);
                    }
                }
            }
        }

        public void SerialNumberingAutomaticMode(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1, int enableSerialNumbering = 0, int serialAddress = 0, int serialSize = 0, string serialInitialData = "")
        {
            using (this._logger?.AppendMethodScope(nameof(SerialNumberingAutomaticMode)))
            {
                if (!String.IsNullOrEmpty(filePath))
                {
                    var filePathAdapted = filePath.Replace('\\', '/');
                    if (!String.IsNullOrEmpty(address))
                    {
                        var uintAddress = HexConverterUtil.HexConverterToUint(address);
                        var obCommandPtr = Marshal.StringToHGlobalAnsi(obCommand);
                        this._programmerInstanceApi.SerialNumberingAutomaticMode(filePathAdapted, uintAddress, skipErase, verify, isMassErase, obCommandPtr, run, enableSerialNumbering, serialAddress, serialSize, serialInitialData);
                        Marshal.FreeHGlobal(obCommandPtr);
                    }
                }
            }
        }

        public (CubeProgrammerError, DeviceStorageStructure) GetStorageStructure()
        {
            using (this._logger?.AppendMethodScope(nameof(GetStorageStructure)))
            {
                var deviceStorageStructure = new DeviceStorageStructure();
                var deviceBankSize = Marshal.SizeOf<DeviceBank>();
                var bankSectorSize = Marshal.SizeOf<BankSector>();
                var storageStructurePtr = new IntPtr();

                var output = CubeProgrammerError.Other;
                try
                {
                    var result = this._programmerInstanceApi.GetStorageStructure(ref storageStructurePtr);

                    output = HexConverterUtil.CheckResult(result);

                    if (output.Equals(CubeProgrammerError.NoError))
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
                                        SectorsNumber = deviceBank.SectorsNumber
                                    };
                                    deviceDeviceBank.SetSectors(bankSectorList);

                                    deviceBankList.Add(deviceDeviceBank);

                                    Marshal.DestroyStructure<DeviceBank>(storageStructure.Banks + (i * deviceBankSize));
                                }
                            }

                            deviceStorageStructure.SetBanks(deviceBankList);
                            Marshal.DestroyStructure<StorageStructure>(storageStructurePtr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("GetStorageStructure: ", ex);
                    throw;
                }

                return (output, deviceStorageStructure);
            }
        }

        #endregion

        #region [Option Bytes]

        //OB module groups option bytes functions used by any interface.

        public CubeProgrammerError SendOptionBytesCmd(string command)
        {
            using (this._logger?.AppendMethodScope(nameof(SendOptionBytesCmd)))
            {
                if (String.IsNullOrEmpty(command))
                {
                    throw new ArgumentNullException(nameof(command), "Command cannot be null or empty.");
                }

                var result = this._programmerInstanceApi.SendOptionBytesCmd(command);
                var output = HexConverterUtil.CheckResult(result);

                return output;
            }
        }

        public DevicePeripheralC? InitOptionBytesInterface()
        {
            using (this._logger?.AppendMethodScope(nameof(InitOptionBytesInterface)))
            {
                var pointer = this._programmerInstanceApi.InitOptionBytesInterface();
                return pointer != IntPtr.Zero ? this.DevicePeripheralCHandler(pointer) : null;
            }
        }

        public DevicePeripheralC? FastRomInitOptionBytesInterface(ushort deviceId)
        {
            using (this._logger?.AppendMethodScope(nameof(FastRomInitOptionBytesInterface)))
            {
                var pointer = this._programmerInstanceApi.FastRomInitOptionBytesInterface(deviceId);
                return pointer != IntPtr.Zero ? this.DevicePeripheralCHandler(pointer) : null;
            }
        }

        private DevicePeripheralC? DevicePeripheralCHandler(IntPtr pointer)
        {
            using (this._logger?.AppendMethodScope(nameof(DevicePeripheralCHandler)))
            {
                var pointerSize = Marshal.SizeOf<IntPtr>();

                try
                {
                    PeripheralC? peripheralC = Marshal.PtrToStructure<PeripheralC>(pointer);

                    var bankCList = new List<DeviceBankC>();
                    var bankCItemPointer = IntPtr.Zero;
                    for (var i = 0; i < peripheralC.Value.BanksNumber; i++)
                    {
                        if (peripheralC.Value.Banks != IntPtr.Zero)
                        {
                            bankCItemPointer = Marshal.ReadIntPtr(peripheralC.Value.Banks, i * pointerSize);
                            if (bankCItemPointer != IntPtr.Zero)
                            {
                                var bankCItem = Marshal.PtrToStructure<BankC>(bankCItemPointer);

                                if (bankCItem.Categories != IntPtr.Zero)
                                {
                                    var categoryCList = new List<DeviceCategoryC>();
                                    var categoryCItemPointer = IntPtr.Zero;
                                    for (var ii = 0; ii < bankCItem.CategoriesNumber; ii++)
                                    {
                                        categoryCItemPointer =
                                            Marshal.ReadIntPtr(bankCItem.Categories, ii * pointerSize);
                                        if (categoryCItemPointer != IntPtr.Zero)
                                        {
                                            var categoryCItem = Marshal.PtrToStructure<CategoryC>(categoryCItemPointer);

                                            if (categoryCItem.Bits != IntPtr.Zero)
                                            {
                                                var bitCList = new List<DeviceBitC>();
                                                var bitCItemPointer = IntPtr.Zero;
                                                for (var iii = 0; iii < categoryCItem.BitsNumber; iii++)
                                                {
                                                    bitCItemPointer =
                                                    Marshal.ReadIntPtr(categoryCItem.Bits, iii * pointerSize);

                                                    if (bitCItemPointer != IntPtr.Zero)
                                                    {
                                                        var bitCItem = Marshal.PtrToStructure<BitC>(bitCItemPointer);

                                                        var bitValueCList = new List<BitValueC>();
                                                        var bitValueCItemPointer = IntPtr.Zero;
                                                        for (var iiii = 0; iiii < bitCItem.ValuesNumber; iiii++)
                                                        {
                                                            bitValueCItemPointer =
                                                                Marshal.ReadIntPtr(bitCItem.Values, iiii * pointerSize);
                                                            if (bitValueCItemPointer != IntPtr.Zero)
                                                            {
                                                                var bitValueCItem =
                                                                Marshal.PtrToStructure<BitValueC>(bitValueCItemPointer);
                                                                bitValueCList.Add(bitValueCItem);

                                                                Marshal.DestroyStructure<BitValueC>(bitValueCItemPointer);
                                                            }
                                                        }

                                                        var deviceBitC = new DeviceBitC
                                                        {
                                                            Name = bitCItem.Name,
                                                            Description = bitCItem.Description,
                                                            WordOffset = bitCItem.WordOffset,
                                                            BitOffset = bitCItem.BitOffset,
                                                            BitWidth = bitCItem.BitWidth,
                                                            Access = bitCItem.Access,
                                                            ValuesNumber = bitCItem.ValuesNumber,
                                                            Equation = bitCItem.Equation,
                                                            Reference = Marshal.PtrToStringAnsi(bitCItem.Reference),
                                                            BitValue = bitCItem.BitValue
                                                        };
                                                        deviceBitC.SetValues(bitValueCList);
                                                        bitCList.Add(deviceBitC);

                                                        Marshal.DestroyStructure<BitC>(bitCItemPointer);
                                                    }
                                                }

                                                var deviceCategoryC = new DeviceCategoryC
                                                {
                                                    Name = categoryCItem.Name,
                                                    BitsNumber = categoryCItem.BitsNumber
                                                };
                                                deviceCategoryC.SetBits(bitCList);

                                                categoryCList.Add(deviceCategoryC);
                                                Marshal.DestroyStructure<CategoryC>(categoryCItemPointer);
                                            }
                                        }
                                    }

                                    var deviceBankC = new DeviceBankC
                                    {
                                        Size = bankCItem.Size,
                                        Address = bankCItem.Address,
                                        Access = bankCItem.Access,
                                        CategoriesNumber = bankCItem.CategoriesNumber
                                    };
                                    deviceBankC.SetCategories(categoryCList);
                                    bankCList.Add(deviceBankC);
                                    Marshal.DestroyStructure<BankC>(bankCItemPointer);
                                }
                            }
                        }
                    }

                    var devicePeripheralC = new DevicePeripheralC
                    {
                        Name = peripheralC.Value.Name,
                        Description = peripheralC.Value.Description,
                        BanksNumber = peripheralC.Value.BanksNumber
                    };

                    devicePeripheralC.SetBanks(bankCList);

                    return devicePeripheralC;
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("DevicePeripheralCHandler: ", ex);
                    throw;
                }
                finally
                {
                    Marshal.DestroyStructure<PeripheralC>(pointer);
                }
            }
        }

        public CubeProgrammerError ObDisplay()
        {
            using (this._logger?.AppendMethodScope(nameof(ObDisplay)))
            {
                var output = CubeProgrammerError.Other;
                try
                {
                    var obDisplayResult = this._programmerInstanceApi.ObDisplay();

                    output = HexConverterUtil.CheckResult(obDisplayResult);
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("ObDisplay: ", ex);
                    throw;
                }

                return output;
            }
        }

        #endregion

        #region [Loaders]

        //Loaders module groups loaders functions.

        public void SetLoadersPath(string path)
        {
            using (this._logger?.AppendMethodScope(nameof(SetLoadersPath)))
            {
                if (String.IsNullOrEmpty(path))
                {
                    throw new ArgumentNullException(nameof(path), "Path cannot be null or empty.");
                }

                var pathAdapted = path.Replace('\\', '/');
                this._programmerInstanceApi.SetLoadersPath(pathAdapted);
            }
        }

        public DeviceExternalLoader? SetExternalLoaderPath(string path)
        {
            using (this._logger?.AppendMethodScope(nameof(SetExternalLoaderPath)))
            {
                if (String.IsNullOrEmpty(path))
                {
                    throw new ArgumentNullException(nameof(path), "Path cannot be null or empty.");
                }

                var pathAdapted = path.Replace('\\', '/');
                var externalLoaderPtr = new IntPtr();
                var deviceSectorSize = Marshal.SizeOf<DeviceSector>();
                var output = new DeviceExternalLoader();

                try
                {
                    this._programmerInstanceApi.SetExternalLoaderPath(pathAdapted, ref externalLoaderPtr);
                    if (externalLoaderPtr != IntPtr.Zero)
                    {
                        var externalLoaderStructure = Marshal.PtrToStructure<ExternalLoader>(externalLoaderPtr);

                        output.FilePath = externalLoaderStructure.FilePath;
                        output.DeviceName = externalLoaderStructure.DeviceName;
                        output.DeviceType = externalLoaderStructure.DeviceType;
                        output.DeviceStartAddress = externalLoaderStructure.DeviceStartAddress;
                        output.DeviceSize = externalLoaderStructure.DeviceSize;
                        output.PageSize = externalLoaderStructure.PageSize;
                        output.SectorsTypeNumber = externalLoaderStructure.SectorsTypeNumber;

                        if (externalLoaderStructure.Sectors != IntPtr.Zero)
                        {
                            var deviceSectorList = new List<DeviceSector>();
                            for (var i = 0; i < externalLoaderStructure.SectorsTypeNumber; i++)
                            {
                                var deviceSectorItem = Marshal.PtrToStructure<DeviceSector>(externalLoaderStructure.Sectors + (i * deviceSectorSize));
                                deviceSectorList.Add(deviceSectorItem);
                            }

                            output.SetSectors(deviceSectorList);

                            return output;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("SetExternalLoaderPath: ", ex);
                    throw;
                }

                return null;
            }
        }

        public DeviceExternalLoader? SetExternalLoaderOBL(string path)
        {
            using (this._logger?.AppendMethodScope(nameof(SetExternalLoaderOBL)))
            {
                if (String.IsNullOrEmpty(path))
                {
                    throw new ArgumentNullException(nameof(path), "Path cannot be null or empty.");
                }

                var pathAdapted = path.Replace('\\', '/');
                var externalLoaderPtr = new IntPtr();
                var deviceSectorSize = Marshal.SizeOf<DeviceSector>();
                var output = new DeviceExternalLoader();

                try
                {
                    this._programmerInstanceApi.SetExternalLoaderOBL(pathAdapted, ref externalLoaderPtr);
                    if (externalLoaderPtr != IntPtr.Zero)
                    {
                        var externalLoaderStructure = Marshal.PtrToStructure<ExternalLoader>(externalLoaderPtr);

                        output.FilePath = externalLoaderStructure.FilePath;
                        output.DeviceName = externalLoaderStructure.DeviceName;
                        output.DeviceType = externalLoaderStructure.DeviceType;
                        output.DeviceStartAddress = externalLoaderStructure.DeviceStartAddress;
                        output.DeviceSize = externalLoaderStructure.DeviceSize;
                        output.PageSize = externalLoaderStructure.PageSize;
                        output.SectorsTypeNumber = externalLoaderStructure.SectorsTypeNumber;

                        if (externalLoaderStructure.Sectors != IntPtr.Zero)
                        {
                            var deviceSectorList = new List<DeviceSector>();
                            for (var i = 0; i < externalLoaderStructure.SectorsTypeNumber; i++)
                            {
                                var deviceSectorItem = Marshal.PtrToStructure<DeviceSector>(externalLoaderStructure.Sectors + (i * deviceSectorSize));
                                deviceSectorList.Add(deviceSectorItem);
                            }

                            output.SetSectors(deviceSectorList);

                            return output;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("SetExternalLoaderOBL: ", ex);
                    throw;
                }

                return null;
            }
        }

        public DeviceExternalStorageInfo? GetExternalLoaders(string path = @".\st\Programmer")
        {
            using (this._logger?.AppendMethodScope(nameof(GetExternalLoaders)))
            {
                if (String.IsNullOrEmpty(path))
                {
                    throw new ArgumentNullException(nameof(path), "Path cannot be null or empty.");
                }

                var pathAdapted = path.Replace('\\', '/');
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

                        output.ExternalLoaderNumber = externalStorageInfoStructure.ExternalLoaderNumber;

                        if (externalStorageInfoStructure.ExternalLoader != IntPtr.Zero)
                        {
                            for (var i = 0; i < externalStorageInfoStructure.ExternalLoaderNumber; i++)
                            {
                                var externalLoaderStructure = Marshal.PtrToStructure<ExternalLoader>(externalStorageInfoStructure.ExternalLoader + (i * externalLoaderSize));

                                var deviceExternalLoader = new DeviceExternalLoader
                                {
                                    FilePath = externalLoaderStructure.FilePath,
                                    DeviceName = externalLoaderStructure.DeviceName,
                                    DeviceType = externalLoaderStructure.DeviceType,
                                    DeviceStartAddress = externalLoaderStructure.DeviceStartAddress,
                                    DeviceSize = externalLoaderStructure.DeviceSize,
                                    PageSize = externalLoaderStructure.PageSize,
                                    SectorsTypeNumber = externalLoaderStructure.SectorsTypeNumber
                                };

                                if (externalLoaderStructure.Sectors != IntPtr.Zero)
                                {
                                    var deviceSectorList = new List<DeviceSector>();
                                    for (var ii = 0; ii < externalLoaderStructure.SectorsTypeNumber; ii++)
                                    {
                                        var deviceSectorItem = Marshal.PtrToStructure<DeviceSector>(externalLoaderStructure.Sectors + (ii * deviceSectorSize));
                                        deviceSectorList.Add(deviceSectorItem);
                                    }

                                    deviceExternalLoader.SetSectors(deviceSectorList);
                                }

                                externalLoaderList.Add(deviceExternalLoader);
                            }

                            output.SetExternalLoader(externalLoaderList);

                            return output;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._logger?.ErrorMessage("GetExternalLoaders: ", ex);
                    throw;
                }

                return null;
            }
        }

        public void RemoveExternalLoader(string path)
        {
            using (this._logger?.AppendMethodScope(nameof(RemoveExternalLoader)))
            {
                if (String.IsNullOrEmpty(path))
                {
                    throw new ArgumentNullException(nameof(path), "Path cannot be null or empty.");
                }

                var pathAdapted = path.Replace('\\', '/');
                this._programmerInstanceApi.RemoveExternalLoader(pathAdapted);
            }
        }

        public void DeleteLoaders()
        {
            using (this._logger?.AppendMethodScope(nameof(DeleteLoaders)))
            {
                this._programmerInstanceApi.DeleteLoaders();
            }
        }

        #endregion

        #region [STM32WB specific]

        /// Specific APIs used exclusively for STM32WB series to manage BLE Stack, and they are available only through USB DFU and UART bootloader interfaces,
        /// except for the "firmwareDelete" and the "firmwareUpgrade", available through USB DFU, UART and SWD interfaces.
        /// Connection under Reset is mandatory.

        public (CubeProgrammerError, byte[]) GetUID64()
        {
            using (this._logger?.AppendMethodScope(nameof(GetUID64)))
            {
                var buffer = new byte[8];
                var bufferPtr = new IntPtr();
                var getUID64Result = this._programmerInstanceApi.GetUID64(ref bufferPtr);

                var result = HexConverterUtil.CheckResult(getUID64Result);
                if (result.Equals(CubeProgrammerError.NoError) && bufferPtr != IntPtr.Zero)
                {
                    Marshal.Copy(bufferPtr, buffer, 0, 8);
                }

                return (result, buffer);
            }
        }

        public bool FirmwareDelete()
        {
            using (this._logger?.AppendMethodScope(nameof(FirmwareDelete)))
            {
                return this._programmerInstanceApi.FirmwareDelete();
            }
        }

        public bool FirmwareUpgrade(string filePath, string address, WbFunctionArguments firstInstall, WbFunctionArguments startStack, WbFunctionArguments verify)
        {
            using (this._logger?.AppendMethodScope(nameof(FirmwareUpgrade)))
            {
                var uintAddress = HexConverterUtil.HexConverterToUint(address);
                var filePathAdapted = String.IsNullOrEmpty(filePath) ? "" : filePath.Replace('\\', '/');

                return this._programmerInstanceApi.FirmwareUpgrade(filePathAdapted, uintAddress, (uint)firstInstall, (uint)startStack, (uint)verify);
            }
        }

        public bool StartWirelessStack()
        {
            using (this._logger?.AppendMethodScope(nameof(StartWirelessStack)))
            {
                return this._programmerInstanceApi.StartWirelessStack();
            }
        }

        public bool UpdateAuthKey(string filePath)
        {
            using (this._logger?.AppendMethodScope(nameof(UpdateAuthKey)))
            {
                return this._programmerInstanceApi.UpdateAuthKey(filePath);
            }
        }

        public CubeProgrammerError AuthKeyLock()
        {
            using (this._logger?.AppendMethodScope(nameof(AuthKeyLock)))
            {
                var authKeyLockResult = this._programmerInstanceApi.AuthKeyLock();
                var result = HexConverterUtil.CheckResult(authKeyLockResult);

                return result;
            }
        }

        public CubeProgrammerError WriteUserKey(string filePath, byte keyType)
        {
            using (this._logger?.AppendMethodScope(nameof(WriteUserKey)))
            {
                var filePathAdapted = String.IsNullOrEmpty(filePath) ? "" : filePath.Replace('\\', '/');
                var writeUserKeyResult = this._programmerInstanceApi.WriteUserKey(filePathAdapted, keyType);
                var result = HexConverterUtil.CheckResult(writeUserKeyResult);

                return result;
            }
        }

        public bool AntiRollBack()
        {
            using (this._logger?.AppendMethodScope(nameof(AntiRollBack)))
            {
                return this._programmerInstanceApi.AntiRollBack();
            }
        }

        public bool StartFus()
        {
            using (this._logger?.AppendMethodScope(nameof(StartFus)))
            {
                return this._programmerInstanceApi.StartFus();
            }
        }

        public CubeProgrammerError UnlockChip()
        {
            using (this._logger?.AppendMethodScope(nameof(UnlockChip)))
            {
                var unlockChipResult = this._programmerInstanceApi.UnlockChip();
                var result = HexConverterUtil.CheckResult(unlockChipResult);

                return result;
            }
        }

        #endregion

        #region [STM32MP specific functions]

        public CubeProgrammerError ProgramSsp(string sspFile, string licenseFile, string tfaFile, int hsmSlotId)
        {
            using (this._logger?.AppendMethodScope(nameof(ProgramSsp)))
            {
                var sspFileAdapted = String.IsNullOrEmpty(sspFile) ? "" : sspFile.Replace('\\', '/');
                var licenseFileAdapted = String.IsNullOrEmpty(licenseFile) ? "" : licenseFile.Replace('\\', '/');
                var tfaFileAdapted = String.IsNullOrEmpty(tfaFile) ? "" : tfaFile.Replace('\\', '/');
                var programSspResult = this._programmerInstanceApi.ProgramSsp(sspFileAdapted, licenseFileAdapted, tfaFileAdapted, hsmSlotId);

                var result = HexConverterUtil.CheckResult(programSspResult);

                return result;
            }
        }

        #endregion

        #region [STM32 HSM specific functions]

        public string GetHsmFirmwareID(int hsmSlotId)
        {
            using (this._logger?.AppendMethodScope(nameof(GetHsmFirmwareID)))
            {
                return this._programmerInstanceApi.GetHsmFirmwareID(hsmSlotId);
            }
        }

        public ulong GetHsmCounter(int hsmSlotId)
        {
            using (this._logger?.AppendMethodScope(nameof(GetHsmCounter)))
            {
                return this._programmerInstanceApi.GetHsmCounter(hsmSlotId);
            }
        }

        public string GetHsmState(int hsmSlotId)
        {
            using (this._logger?.AppendMethodScope(nameof(GetHsmState)))
            {
                return this._programmerInstanceApi.GetHsmState(hsmSlotId);
            }
        }

        public string GetHsmVersion(int hsmSlotId)
        {
            using (this._logger?.AppendMethodScope(nameof(GetHsmVersion)))
            {
                return this._programmerInstanceApi.GetHsmVersion(hsmSlotId);
            }
        }

        public string GetHsmType(int hsmSlotId)
        {
            using (this._logger?.AppendMethodScope(nameof(GetHsmType)))
            {
                return this._programmerInstanceApi.GetHsmType(hsmSlotId);
            }
        }

        public CubeProgrammerError GetHsmLicense(int hsmSlotId, string outLicensePath)
        {
            using (this._logger?.AppendMethodScope(nameof(GetHsmLicense)))
            {
                var outLicensePathAdapted = String.IsNullOrEmpty(outLicensePath) ? "" : outLicensePath.Replace('\\', '/');
                var getHsmLicenseResult = this._programmerInstanceApi.GetHsmLicense(hsmSlotId, outLicensePathAdapted);

                var result = HexConverterUtil.CheckResult(getHsmLicenseResult);

                return result;
            }
        }

        #endregion

        #region [EXTENDED]

        public void Halt()
        {
            using (this._logger?.AppendMethodScope(nameof(Halt)))
            {
                this._programmerInstanceApi.CpuHalt();
            }
        }

        public void Run()
        {
            using (this._logger?.AppendMethodScope(nameof(Run)))
            {
                this._programmerInstanceApi.CpuRun();
            }
        }

        public void Step()
        {
            using (this._logger?.AppendMethodScope(nameof(Step)))
            {
                this._programmerInstanceApi.CpuStep();
            }
        }

        #endregion

        #region [Dispose]

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref this._isDisposed, DisposedFlag, 0) == 0)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                    this._programmerInstanceApi.Dispose();
                }

                // Free any unmanaged objects here.
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
