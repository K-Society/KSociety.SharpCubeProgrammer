// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using DeviceDataStructure;
    using Enum;
    using Interface;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Struct;

    public class CubeProgrammerApi : DisposableObject, ICubeProgrammerApi
    {
        /// <summary>
        /// Synchronization object to protect loading the native library and its functions. This field is read-only.
        /// </summary>
        private readonly object _syncRoot = new object();

        private Native.SafeLibraryHandle? _handle;
        private readonly ILogger<CubeProgrammerApi>? _logger;

        #region [Constructor]

        public CubeProgrammerApi(ILogger<CubeProgrammerApi>? logger = default)
        {
            if (logger == null)
            {
                logger = new NullLogger<CubeProgrammerApi>();
            }

            this._logger = logger;
            this.Init();
        }

        #endregion

        private void Init()
        {
            if (this._handle == null)
            {
                lock (this._syncRoot)
                {
                    if (this._handle == null)
                    {
                        this._handle = Native.Utility.LoadNativeLibrary(Environment.Is64BitProcess ? @".\dll\x64\STLinkUSBDriver.dll" : @".\dll\x86\STLinkUSBDriver.dll", IntPtr.Zero, 0);

                        if (this._handle.IsInvalid)
                        {
                            var error = Marshal.GetLastWin32Error();
                            this._handle = null;
                            this._logger?.LogError("Loading {0} - {1} library error: {3} !", "STLinkUSBDriver.dll", Environment.Is64BitProcess ? "x64" : "x86", error);
                        }
                        else
                        {

                            this._logger?.LogInformation("Loading {0} - {1} library.", "STLinkUSBDriver.dll", Environment.Is64BitProcess ? "x64" : "x86");
                        }
                    }
                }
            }
        }

        #region [ST-LINK]

        //ST-LINK module groups debug ports JTAG/SWD functions together.

        /// <inheritdoc />
        public CubeProgrammerError TryConnectStLink(int stLinkProbeIndex = 0, int shared = 0, DebugConnectionMode debugConnectMode = DebugConnectionMode.UnderResetMode)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;

            try
            {
                var connectStLinkResult = Native.ProgrammerApi.TryConnectStLink(stLinkProbeIndex, shared, debugConnectMode);

                output = this.CheckResult(connectStLinkResult);
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
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "GetStLinkList: ");
            }
            //finally
            //{
            //    Marshal.FreeHGlobal(listPtr);
            //}

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
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "GetStLinkEnumerationList: ");
            }
            //finally
            //{
            //    Marshal.FreeHGlobal(listPtr);
            //}

            return parametersList;
        }

        /// <inheritdoc />
        public CubeProgrammerError ConnectStLink(DebugConnectParameters debugConnectParameters)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;

            try
            {
                var connectStLinkResult = Native.ProgrammerApi.ConnectStLink(debugConnectParameters);

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
            var resetResult = Native.ProgrammerApi.Reset(rstMode);
            var output = this.CheckResult(resetResult);
            return output;
        }

        #endregion

        #region [Bootloader]

        //Bootloader module is a way to group Serial interfaces USB/UART/SPI/I2C/CAN functions together.

        /// <inheritdoc />
        public void GetUsartList()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void ConnectUsartBootloader()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void SendByteUart()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public int GetDfuDeviceList(ref List<DfuDeviceInfo> dfuDeviceList)
        {
            var numberOfItems = 0;
            var listPtr = new IntPtr();

            try
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
                var connectDfuBootloaderResult = Native.ProgrammerApi.ConnectDfuBootloader(usbIndex);
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
        public void ConnectDfuBootloader2()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void ConnectSpiBootloader()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void ConnectCanBootloader()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void ConnectI2cBootloader()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region [General purposes]

        // General module groups general purposes functions used by any interface.

        /// <inheritdoc />
        public DisplayCallBacks SetDisplayCallbacks(InitProgressBar initProgressBar, LogMessageReceived messageReceived, ProgressBarUpdateReceived progressBarUpdate)
        {
            var callbacksHandle = new DisplayCallBacks
            {
                InitProgressBar = initProgressBar,
                LogMessage = messageReceived,
                LoadBar = progressBarUpdate
            };

            Native.ProgrammerApi.SetDisplayCallbacks(callbacksHandle);

            return callbacksHandle;
        }

        /// <inheritdoc />
        public void SetDisplayCallbacks(DisplayCallBacks callbacksHandle)
        {
            Native.ProgrammerApi.SetDisplayCallbacks(callbacksHandle);
        }

        /// <inheritdoc />
        public void SetVerbosityLevel(CubeProgrammerVerbosityLevel level)
        {
            Native.ProgrammerApi.SetVerbosityLevel((int)level);
        }

        /// <inheritdoc />
        public bool CheckDeviceConnection()
        {
            var checkDeviceConnectionResult = Native.ProgrammerApi.CheckDeviceConnection();
            return checkDeviceConnectionResult;
        }

        /// <inheritdoc />
        public GeneralInf? GetDeviceGeneralInf()
        {
            GeneralInf? generalInf = null;
            var pointer = Native.ProgrammerApi.GetDeviceGeneralInf();

            try
            {
                generalInf = Marshal.PtrToStructure<GeneralInf>(pointer);
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
                //var gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                var bufferPtr = IntPtr.Zero;
                //var addrOfPinnedObject = gch.AddrOfPinnedObject();
                var readMemoryResult =
                    Native.ProgrammerApi.ReadMemory(uintAddress, ref bufferPtr, Convert.ToUInt32(byteSize));
                result = this.CheckResult(readMemoryResult);
                if (bufferPtr != IntPtr.Zero)
                {
                    Marshal.Copy(bufferPtr, buffer, 0, byteSize);
                }
                //Marshal..FreeHGlobal(bufferPtr);
                //if (addrOfPinnedObject != IntPtr.Zero)
                //{
                //    Marshal.Copy(addrOfPinnedObject, buffer, 0, byteSize);
                //}

                //gch.Free();
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "ReadMemory: ");
            }

            return (result, buffer);
        }

        public CubeProgrammerError WriteMemory(string address, byte[] data)
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;

            if (!String.IsNullOrEmpty(address) && data.Length > 0)
            {
                var uintAddress = this.HexConverterToUint(address);

                try
                {
                    var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                    var writeMemoryResult = Native.ProgrammerApi.WriteMemory(uintAddress, gch.AddrOfPinnedObject(), (uint)data.Length);
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

        public CubeProgrammerError WriteMemoryAndVerify(string address, byte[] data)
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;

            if (!String.IsNullOrEmpty(address) && data.Length > 0)
            {
                var uintAddress = this.HexConverterToUint(address);

                try
                {
                    var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                    var writeMemoryResult = Native.ProgrammerApi.WriteMemoryAndVerify(uintAddress, gch.AddrOfPinnedObject(), (uint)data.Length);
                    gch.Free();
                    result = this.CheckResult(writeMemoryResult);

                    return result;
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

                var writeMemoryResult = Native.ProgrammerApi.EditSector(uintAddress, gch.AddrOfPinnedObject(), (uint)data.Length);
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
                var downloadFileResult = Native.ProgrammerApi.DownloadFile(
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
        public CubeProgrammerError Execute(string address)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;

            try
            {
                var uintAddress = this.HexConverterToUint(address);
                var executeResult = Native.ProgrammerApi.Execute(uintAddress);

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
            var massEraseResult = Native.ProgrammerApi.MassErase(sFlashMemName);
            var output = this.CheckResult(massEraseResult);

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError SectorErase(uint[] sectors, uint sectorNbr, string sFlashMemName = "")
        {
            var sectorEraseResult = Native.ProgrammerApi.SectorErase(sectors, sectorNbr, sFlashMemName);
            var output = this.CheckResult(sectorEraseResult);

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError ReadUnprotect()
        {
            var result = Native.ProgrammerApi.ReadUnprotect();
            var output = this.CheckResult(result);

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError TzenRegression()
        {
            var result = Native.ProgrammerApi.TzenRegression();
            var output = this.CheckResult(result);

            return output;
        }

        /// <inheritdoc />
        public TargetInterfaceType? GetTargetInterfaceType()
        {
            var result = Native.ProgrammerApi.GetTargetInterfaceType();

            if (result == -1)
            {
                return null;
            }

            return (TargetInterfaceType)result;
        }

        /// <inheritdoc />
        public void GetCancelPointer()
        {
            Native.ProgrammerApi.GetCancelPointer();
        }

        /// <inheritdoc />
        public DeviceFileDataC? FileOpen(string filePath)
        {
            var segmentSize = Marshal.SizeOf<SegmentDataC>();
            var deviceSegmentData = new DeviceFileDataC();
            if (!String.IsNullOrEmpty(filePath))
            {
                var filePathAdapted = filePath.Replace(@"\", "/");

                var filePointer = Native.ProgrammerApi.FileOpen(filePathAdapted);
                try
                {
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

                    return Native.ProgrammerApi.FileOpen(filePathAdapted);
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
            Native.ProgrammerApi.FreeFileData(data);
        }

        /// <inheritdoc />
        public CubeProgrammerError Verify(IntPtr fileData, string address)
        {
            var uintAddress = this.HexConverterToUint(address);

            var verifyResult = Native.ProgrammerApi.Verify(fileData, uintAddress);
            var output = this.CheckResult(verifyResult);

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError VerifyMemory(string address, byte[] data)
        {
            var result = CubeProgrammerError.CubeprogrammerErrorOther;

            if (!String.IsNullOrEmpty(address) && data.Length > 0)
            {
                var uintAddress = this.HexConverterToUint(address);

                var gch = GCHandle.Alloc(data, GCHandleType.Pinned);

                var verifyMemoryResult =
                    Native.ProgrammerApi.VerifyMemory(uintAddress, gch.AddrOfPinnedObject(), (uint) data.Length);
                gch.Free();
                result = this.CheckResult(verifyMemoryResult);


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
                var uintAddress = this.HexConverterToUint(address);

                var gch = GCHandle.Alloc(data, GCHandleType.Pinned);

                var verifyMemoryResult =
                    Native.ProgrammerApi.VerifyMemoryBySegment(uintAddress, gch.AddrOfPinnedObject(), (uint)data.Length);
                gch.Free();
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
                var saveFileToFileResult = Native.ProgrammerApi.SaveFileToFile(fileData, sFileNameAdapted);
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
                    Native.ProgrammerApi.SaveMemoryToFile(intAddress, intSize, fileNameAdapted);

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
            var result = Native.ProgrammerApi.Disconnect();

            var output = this.CheckResult(result);

            return output;
        }

        /// <inheritdoc />
        public void DeleteInterfaceList()
        {
            Native.ProgrammerApi.DeleteInterfaceList();
        }

        /// <inheritdoc />
        public void AutomaticMode(string filePath, string address, uint skipErase = 1U, uint verify = 1U, int isMassErase = 0, string obCommand = "", int run = 1)
        {
            if (!String.IsNullOrEmpty(filePath) || !String.IsNullOrEmpty(address))
            {
                var filePathAdapted = filePath.Replace(@"\", "/");
                var uintAddress = this.HexConverterToUint(address);

                Native.ProgrammerApi.AutomaticMode(filePathAdapted, uintAddress, skipErase, verify, isMassErase, obCommand, run);
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
                                    //Marshal.FreeHGlobal(deviceBank.Sectors);
                                }

                                var deviceDeviceBank = new DeviceDeviceBank
                                {
                                    SectorsNumber = deviceBank.SectorsNumber, Sectors = bankSectorList
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
            //finally
            //{
            //    Marshal.FreeHGlobal(storageStructurePtr);
            //}

            return (output, deviceStorageStructure);
        }

        #endregion

        #region [Option Bytes]

        //OB module groups option bytes functions used by any interface.

        /// <inheritdoc />
        public CubeProgrammerError SendOptionBytesCmd(string command)
        {
            var result = Native.ProgrammerApi.SendOptionBytesCmd(command);
            var output = this.CheckResult(result);
            return output;
        }

        /// <inheritdoc />
        public DevicePeripheralC? InitOptionBytesInterface()
        {
            var pointer = Native.ProgrammerApi.InitOptionBytesInterface();

            return pointer != IntPtr.Zero ? this.DevicePeripheralCHandler(pointer) : null;
        }

        /// <inheritdoc />
        public DevicePeripheralC? FastRomInitOptionBytesInterface(ushort deviceId)
        {
            var pointer = Native.ProgrammerApi.FastRomInitOptionBytesInterface(deviceId);

            return pointer != IntPtr.Zero ? this.DevicePeripheralCHandler(pointer) : null;
        }

        private DevicePeripheralC? DevicePeripheralCHandler(IntPtr pointer)
        {
            var pointerSize = Marshal.SizeOf<IntPtr>();

            try
            {
                PeripheralC? peripheralC = Marshal.PtrToStructure<PeripheralC>(pointer);

                if (peripheralC.HasValue)
                {
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
            this._logger?.LogTrace("ObDisplay.");
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            try
            {
                var obDisplayResult = Native.ProgrammerApi.ObDisplay();

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
            Native.ProgrammerApi.SetLoadersPath(pathAdapted);
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
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "SetExternalLoaderPath: ");
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
            Native.ProgrammerApi.RemoveExternalLoader(pathAdapted);
        }

        /// <inheritdoc />
        public void DeleteLoaders()
        {
            Native.ProgrammerApi.DeleteLoaders();
        }

        #endregion

        #region [STM32WB specific]

        /// Specific APIs used exclusively for STM32WB series to manage BLE Stack and they are available only through USB DFU and UART bootloader interfaces,
        /// except for the "firmwareDelete" and the "firmwareUpgrade", available through USB DFU, UART and SWD interfaces.
        /// Connection under Reset is mandatory.

        /// <inheritdoc />
        public (CubeProgrammerError, byte[]) GetUID64()
        {
            var buffer = new byte[8];
            var bufferPtr = new IntPtr();

            var getUID64Result = Native.ProgrammerApi.GetUID64(ref bufferPtr );

            var result = this.CheckResult(getUID64Result);
            if (result.Equals(CubeProgrammerError.CubeprogrammerNoError) && bufferPtr != IntPtr.Zero)
            {
                Marshal.Copy(bufferPtr, buffer, 0, 8);
            }

            return (result, buffer);
        }

        /// <inheritdoc />
        public CubeProgrammerError FirmwareDelete()
        {
            var firmwareDeleteResult = Native.ProgrammerApi.FirmwareDelete();

            var result = this.CheckResult(firmwareDeleteResult);

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError FirmwareUpgrade(string filePath, string address, uint firstInstall, uint startStack, uint verify)
        {
            var uintAddress = this.HexConverterToUint(address);
            var filePathAdapted = String.IsNullOrEmpty(filePath) ? "" : filePath.Replace(@"\", "/");

            var firmwareUpgradeResult =
                Native.ProgrammerApi.FirmwareUpgrade(filePathAdapted, uintAddress, firstInstall, startStack, verify);

            var result = this.CheckResult(firmwareUpgradeResult);

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError StartWirelessStack()
        {
            var startWirelessStackResult = Native.ProgrammerApi.StartWirelessStack();

            var result = this.CheckResult(startWirelessStackResult);

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError UpdateAuthKey(string filePath)
        {
            var updateAuthKeyResult = Native.ProgrammerApi.UpdateAuthKey(filePath);

            var result = this.CheckResult(updateAuthKeyResult);

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError AuthKeyLock()
        {
            var authKeyLockResult = Native.ProgrammerApi.AuthKeyLock();

            var result = this.CheckResult(authKeyLockResult);

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError WriteUserKey(string filePath, byte keyType)
        {
            var filePathAdapted = String.IsNullOrEmpty(filePath) ? "" : filePath.Replace(@"\", "/");

            var writeUserKeyResult = Native.ProgrammerApi.WriteUserKey(filePathAdapted, keyType);

            var result = this.CheckResult(writeUserKeyResult);

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError AntiRollBack()
        {
            var antiRollBackResult = Native.ProgrammerApi.AntiRollBack();

            var result = this.CheckResult(antiRollBackResult);

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError StartFus()
        {
            var startFusResult = Native.ProgrammerApi.StartFus();

            var result = this.CheckResult(startFusResult);

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError UnlockChip()
        {
            var unlockChipResult = Native.ProgrammerApi.UnlockChip();

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
            var programSspResult = Native.ProgrammerApi.ProgramSsp(sspFileAdapted, licenseFileAdapted, tfaFileAdapted, hsmSlotId);

            var result = this.CheckResult(programSspResult);

            return result;
        }

        #endregion

        #region [STM32 HSM specific functions]

        /// <inheritdoc />
        public string GetHsmFirmwareID(int hsmSlotId)
        {
            return Native.ProgrammerApi.GetHsmFirmwareID(hsmSlotId);
        }

        /// <inheritdoc />
        public ulong GetHsmCounter(int hsmSlotId)
        {
            return Native.ProgrammerApi.GetHsmCounter(hsmSlotId);
        }

        /// <inheritdoc />
        public string GetHsmState(int hsmSlotId)
        {
            return Native.ProgrammerApi.GetHsmState(hsmSlotId);
        }

        /// <inheritdoc />
        public string GetHsmVersion(int hsmSlotId)
        {
            return Native.ProgrammerApi.GetHsmVersion(hsmSlotId);
        }

        /// <inheritdoc />
        public string GetHsmType(int hsmSlotId)
        {
            return Native.ProgrammerApi.GetHsmType(hsmSlotId);
        }

        /// <inheritdoc />
        public CubeProgrammerError GetHsmLicense(int hsmSlotId, string outLicensePath)
        {
            var outLicensePathAdapted = String.IsNullOrEmpty(outLicensePath) ? "" : outLicensePath.Replace(@"\", "/");
            var getHsmLicenseResult = Native.ProgrammerApi.GetHsmLicense(hsmSlotId, outLicensePathAdapted);

            var result = this.CheckResult(getHsmLicenseResult);

            return result;
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
                return hex.Split(new char[] {'x', 'X'})[1];
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

        protected override void DisposeUnmanagedResources()
        {
            this._handle?.Dispose();
        }

        #endregion
    } // CubeProgrammerApi.
}
