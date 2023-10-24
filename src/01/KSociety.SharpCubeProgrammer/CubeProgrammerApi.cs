// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using Base.InfraSub.Shared.Class;
    using DeviceDataStructure;
    using Enum;
    using Events;
    using Interface;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Struct;
    using Wmi;

    public class CubeProgrammerApi : DisposableObject, ICubeProgrammerApi
    {
        /// <summary>
        /// Synchronization object to protect loading the native library and its functions. This field is read-only.
        /// </summary>
        private readonly object _syncRoot = new object();

        private Native.SafeLibraryHandle? _handle;

        public event EventHandler<StLinkFoundEventArgs>? StLinksFoundStatus;
        public event EventHandler<StLinkAddedEventArgs>? StLinkAdded;
        public event EventHandler<StLinkRemovedEventArgs>? StLinkRemoved;

        public event EventHandler<Stm32BootLoaderFoundEventArgs>? Stm32BootLoaderFoundStatus;
        public event EventHandler<Stm32BootLoaderAddedEventArgs>? Stm32BootLoaderAdded;
        public event EventHandler<Stm32BootLoaderRemovedEventArgs>? Stm32BootLoaderRemoved;

        private readonly ILogger<CubeProgrammerApi>? _logger;

        protected readonly IWmiManager WmiManager;

        public bool StLinkReady { get; private set; }

        public bool Stm32BootLoaderReady { get; private set; }

        #region [Constructor]

        public CubeProgrammerApi(IWmiManager wmiManager, ILogger<CubeProgrammerApi>? logger = default) 
        {
            this.WmiManager = wmiManager;

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

        public async ValueTask GetStLinkPorts(CancellationToken cancellationToken = default)
        {
            await this.RegisterStLinkEvents(cancellationToken);
            await this.RegisterStm32BootLoaderEvents(cancellationToken);
        }

        private async ValueTask RegisterStLinkEvents(CancellationToken cancellationToken = default)
        {
            this.RegisterStLink();
            await this.WmiManager.SearchAllPortsAsync(SearchPortType.StLinkOnly, cancellationToken).ConfigureAwait(false);
        }

        private async ValueTask RegisterStm32BootLoaderEvents(CancellationToken cancellationToken = default)
        {
            this.RegisterStm32BootLoader();
            await this.WmiManager.SearchAllPortsAsync(SearchPortType.STM32BootLoaderOnly, cancellationToken)
                .ConfigureAwait(false);
        }

        private void RegisterStLink()
        {
            this.WmiManager.StLinkPortChangeStatus += this.WmiManagerOnStLinkPortChangeStatus;
            this.WmiManager.StLinkPortScanned += this.WmiManagerOnStLinkPortScanned;
        }

        private void RegisterStm32BootLoader()
        {
            this.WmiManager.STM32BootLoaderPortChangeStatus += this.WmiManagerOnStm32BootLoaderPortChangeStatus;
            this.WmiManager.STM32BootLoaderPortScanned += this.WmiManagerOnStm32BootLoaderPortScanned;
        }

        private void WmiManagerOnStLinkPortChangeStatus(object sender, Wmi.StLink.StLinkPortChangeStatusEventArgs e)
        {
            this._logger?.LogTrace("CubeProgrammerApi WmiManagerOnStLinkPortChangeStatus: {0} - {1}", e.PortName, e.Status);
            if (e.Status)
            {
                this.StLinkReady = true;
                this.OnStLinkAdded();
            }
            else
            {
                this.StLinkReady = false;
                this.OnStLinkRemoved();
            }
        }

        private void WmiManagerOnStLinkPortScanned(object sender, Wmi.StLink.StLinkPortScannedEventArgs e)
        {
            this._logger?.LogTrace("CubeProgrammerApi WmiManagerOnStLinkPortScanned: {0}", e.PortsList.Count);

            if (e.PortsList.Any())
            {
                this.StLinkReady = true;
                this.OnStLinksFoundStatus();
            }
        }

        private void WmiManagerOnStm32BootLoaderPortChangeStatus(object sender, Wmi.STM32.STM32BootLoaderPortChangeStatusEventArgs e)
        {
            this._logger?.LogTrace("CubeProgrammerApi WmiManagerOnStm32BootLoaderPortChangeStatus: {0} - {1}", e.PortName, e.Status);
            if (e.Status)
            {
                this.Stm32BootLoaderReady = true;
                this.OnStm32BootLoaderAdded();
            }
            else
            {
                this.Stm32BootLoaderReady = false;
                this.OnStm32BootLoaderRemoved();
            }
        }

        private void WmiManagerOnStm32BootLoaderPortScanned(object sender, Wmi.STM32.STM32BootLoaderPortScannedEventArgs e)
        {
            this._logger?.LogTrace("CubeProgrammerApi WmiManagerOnStm32BootLoaderPortScanned: {0}", e.PortsList.Count);

            if (e.PortsList.Any())
            {
                this.Stm32BootLoaderReady = true;
                this.OnStm32BootLoadersFoundStatus();
            }
        }

        #region [STLINK]

        //STLINK module groups debug ports JTAG/SWD functions together.

        /// <inheritdoc />
        public IEnumerable<DebugConnectParameters> GetStLinkList(bool shared = false)
        {
            this._logger?.LogTrace("GetStLinkList shared: {0}", shared);
            var listPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>());
            var parametersList = new List<DebugConnectParameters>();

            try
            {
                var size = Marshal.SizeOf<DebugConnectParameters>();
                this._logger?.LogTrace("GetStLinkList size: {0}", size);
                var numberOfItems = Native.ProgrammerApi.GetStLinkList(listPtr, shared ? 1 : 0);
                this._logger?.LogTrace("GetStLinkList number of items: {0}", numberOfItems);
                var listDereference = Marshal.PtrToStructure<IntPtr>(listPtr);

                for (var i = 0; i < numberOfItems; i++)
                {
                    var currentItem = Marshal.PtrToStructure<DebugConnectParameters>(listDereference + (i * size));
                    this._logger?.LogTrace("GetStLinkList DebugConnectParameters: {0} - {1}", i, currentItem.SerialNumber);
                    parametersList.Add(currentItem);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "GetStLinkList: ");
            }
            finally
            {
                Native.ProgrammerApi.DeleteInterfaceList();
                Marshal.FreeHGlobal(listPtr);
            }

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

                this._logger?.LogTrace("ConnectStLink: {0} result: {1}", debugConnectParameters.SerialNumber, output);
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
            this._logger?.LogTrace("Reset: {0} result: {1}", rstMode, output);
            return output;
        }

        #endregion

        #region [Bootloader]

        //Bootloader module is a way to group Serial interfaces USB/UART/SPI/I2C/CAN functions together.

        /// <inheritdoc />
        public void GetUsartList()
        {
            //Register();

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void ConnectUsartBootloader()
        {
            //Register();

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void SendByteUart()
        {
            //Register();

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public int GetDfuDeviceList(ref List<DfuDeviceInfo> dfuDeviceList)
        {
            var numberOfItems = 0;
            var listPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>());

            try
            {
                var size = Marshal.SizeOf<DfuDeviceInfo>();

                this._logger?.LogTrace("GetDfuDeviceList iPID: {0} iVID: {1}", 0xdf11, 0x0483);
                numberOfItems = Native.ProgrammerApi.GetDfuDeviceList(listPtr, 0xdf11, 0x0483);
                this._logger?.LogTrace("GetDfuDeviceList DFU devices found : {0}", numberOfItems);

                var listDereference = Marshal.PtrToStructure<IntPtr>(listPtr);

                for (var i = 0; i < numberOfItems; i++)
                {
                    var currentItem = Marshal.PtrToStructure<DfuDeviceInfo>(listDereference + (i * size));
                    if (currentItem != null)
                    {
                        this._logger?.LogTrace("GetDfuDeviceList DfuDeviceInfo: {0} - {1}", i, currentItem.SerialNumber);
                        dfuDeviceList.Add(currentItem);
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "GetDfuDeviceList:");
            }
            finally
            {
                Marshal.FreeHGlobal(listPtr);
            }

            return numberOfItems;
        }

        /// <inheritdoc />
        public CubeProgrammerError ConnectDfuBootloader(string usbIndex)
        {
            this._logger?.LogTrace("ConnectDfuBootloader: {0}", usbIndex);
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
        public bool CheckDeviceConnection()
        {
            //Register();

            var checkDeviceConnectionResult = Native.ProgrammerApi.CheckDeviceConnection();
            this._logger?.LogTrace("CheckDeviceConnection. {0} ROW: {1}", checkDeviceConnectionResult ? "OK" : "KO",
                checkDeviceConnectionResult);
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
                this._logger?.LogTrace("GetDeviceGeneralInf: Name: {0} Type: {1} CPU: {2}", generalInf.Name, generalInf.Type,
                    generalInf.Cpu);
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
            var bufferSize = Marshal.SizeOf(buffer[0]) * buffer.Length;

            try
            {
                var bufferPtr = Marshal.AllocHGlobal(bufferSize);
                var readMemoryResult =
                    Native.ProgrammerApi.ReadMemory(uintAddress, out bufferPtr, Convert.ToUInt32(byteSize));
                result = this.CheckResult(readMemoryResult);
                if (bufferPtr != IntPtr.Zero)
                {
                    //var byteArray = Marshal.PtrToStringAnsi(bufferPtr);//Marshal.PtrToStructure<byte[]>(bufferPtr);
                    Marshal.Copy(bufferPtr, buffer, 0, byteSize);

                    //for(int i = 0; i < size; i += 4)
                    //{
                    //    _logger.LogTrace("ReadMemory: {0} {1} {2} {3}", buffer[i].ToString("X"), buffer[i+1].ToString("X"), buffer[i+2].ToString("X"), buffer[i+3].ToString("X"));
                    //}

                    //_logger.LogTrace("ReadMemory: {0}", BitConverter.ToString(buffer));
                }
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

                var gch = GCHandle.Alloc(data, GCHandleType.Pinned);

                var writeMemoryResult =
                    Native.ProgrammerApi.WriteMemory(uintAddress, gch.AddrOfPinnedObject(), (uint)data.Length);
                gch.Free();
                result = this.CheckResult(writeMemoryResult);
                

                return result;
            }

            return result;
        }

        /// <inheritdoc />
        public CubeProgrammerError DownloadFile(string inputFilePath, string address, uint skipErase, uint verify)
        {
            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            var extension = Path.GetExtension(inputFilePath);
            var binPath = @"";


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

            this._logger?.LogTrace("DownloadFile: {0} - {1}", filePathAdapted, binPathAdapted);

            try
            {
                var downloadFileResult = Native.ProgrammerApi.DownloadFile(
                    filePathAdapted,
                    uintAddress,
                    skipErase,
                    verify,
                    binPathAdapted
                );
                //_logger?.LogTrace("DownloadFile result: {0}", downloadFileResult);
                output = this.CheckResult(downloadFileResult);
                this._logger?.LogTrace("DownloadFile filePathAdapted: {0} binPathAdapted: {1} downloadFileResult: {2}",
                    filePathAdapted, binPathAdapted, output);
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
                this._logger?.LogTrace("Execute address: {0} result: {1}", address, output);
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

            this._logger?.LogTrace("MassErase flash mem name: {0} result: {1}", sFlashMemName, output);

            return output;
        }

        /// <inheritdoc />
        public CubeProgrammerError SectorErase(uint[] sectors, uint sectorNbr, string sFlashMemName = "")
        {
            var sectorEraseResult = Native.ProgrammerApi.SectorErase(sectors, sectorNbr, sFlashMemName);
            var output = this.CheckResult(sectorEraseResult);

            this._logger?.LogTrace("SectorErase sectors: {0}, sectors number: {1}, flash mem name: {2}, result: {3}", sectors, sectorNbr, sFlashMemName, output);

            return output;
        }

        /// <inheritdoc />
        public void ReadUnprotect()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void GetTargetInterfaceType()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void GetCancelPointer()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public FileDataC? FileOpen(string filePath)
        {
            FileDataC? fileData = null;
            if (!String.IsNullOrEmpty(filePath))
            {
                var filePathAdapted = filePath.Replace(@"\", "/");
                this._logger?.LogTrace("File Open: {0}", filePathAdapted);

                var filePointer = Native.ProgrammerApi.FileOpen(filePathAdapted);

                if (!filePointer.Equals(IntPtr.Zero))
                {
                    fileData = Marshal.PtrToStructure<FileDataC>(filePointer);
                    var segment = Marshal.PtrToStructure<SegmentDataC>(fileData.segments);
                    var data = new byte[segment.size];
                    Marshal.Copy(segment.data, data, 0, segment.size);
                    Marshal.DestroyStructure<SegmentDataC>(fileData.segments);
                    Marshal.DestroyStructure<FileDataC>(filePointer);
                }
            }

            return fileData;
        }

        public FileDataC GetFileFromByteArray(byte[] data)
        {
            var gch = GCHandle.Alloc(data, GCHandleType.Pinned);

            var segment = new SegmentDataC
            {
                size = data.Length,
                address = 0,
                data = gch.AddrOfPinnedObject()
            };

            var gchSegment = GCHandle.Alloc(segment, GCHandleType.Pinned);

            var fileData = new FileDataC
            {
                Type = 0,
                segmentsNbr = 1,
                segments = gchSegment.AddrOfPinnedObject()
            };

            return fileData;
        }

        /// <inheritdoc />
        public void FreeFileData()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public CubeProgrammerError Verify(byte[] data, string address)
        {
            var uintAddress = this.HexConverterToUint(address);
            this._logger?.LogTrace("Verify address: {0}", uintAddress);

            var gch = GCHandle.Alloc(data, GCHandleType.Pinned);

            var segment = new SegmentDataC
            {
                size = data.Length,
                address = 0,
                data = gch.AddrOfPinnedObject()
            };

            var gchSegment = GCHandle.Alloc(segment, GCHandleType.Pinned);

            var fileData = new FileDataC
            {
                Type = 0,
                segmentsNbr = 1,
                segments = gchSegment.AddrOfPinnedObject()
            };

            var verifyResult = Native.ProgrammerApi.Verify(fileData, uintAddress);
            var output = this.CheckResult(verifyResult);

            gchSegment.Free();
            gch.Free();

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
        public void SaveFileToFile()
        {
            throw new NotImplementedException();
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

                this._logger?.LogTrace("SaveMemoryToFile address: {0} size: {1} file name: {2} result: {3}", intAddress,
                    intSize, fileName, output);
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
            this._logger?.LogTrace("Disconnect. ");
            var result = Native.ProgrammerApi.Disconnect();

            var output = this.CheckResult(result);

            return output;
        }

        /// <inheritdoc />
        public void DeleteInterfaceList()
        {
            this._logger?.LogTrace(" DeleteInterfaceList. ");
            Native.ProgrammerApi.DeleteInterfaceList();
        }

        /// <inheritdoc />
        public void AutomaticMode()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public (CubeProgrammerError, DeviceStorageStructure) GetStorageStructure()
        {
            var deviceStorageStructure = new DeviceStorageStructure();
            var storageStructure = new StorageStructure();
            var deviceBank = new DeviceBank();
            var bankSector = new BankSector();

            var bankSectorPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(bankSector));
            Marshal.StructureToPtr(bankSector, bankSectorPtr, false);

            deviceBank.Sectors = bankSectorPtr;

            var deviceBankPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(deviceBank));
            Marshal.StructureToPtr(deviceBank, deviceBankPtr, false);

            storageStructure.Banks = deviceBankPtr;

            var storageStructurePtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(storageStructure));
            Marshal.StructureToPtr(storageStructure, storageStructurePtr, false);

            var output = CubeProgrammerError.CubeprogrammerErrorOther;
            try
            {
                var result = Native.ProgrammerApi.GetStorageStructure(storageStructurePtr);

                output = this.CheckResult(result);

                if (output.Equals(CubeProgrammerError.CubeprogrammerNoError))
                {
                    var deviceStorageStructureDereference = Marshal.PtrToStructure<IntPtr>(storageStructurePtr);
                    var storageStructureResult = Marshal.PtrToStructure<StorageStructure>(deviceStorageStructureDereference);
                    deviceStorageStructure.BanksNumber = storageStructureResult.BanksNumber;
                    var deviceBankResult = Marshal.PtrToStructure<DeviceBank>(storageStructureResult.Banks);
                    deviceStorageStructure.SectorsNumber = deviceBankResult.SectorsNumber;
                    var bankSectors = Marshal.PtrToStructure<BankSector>(deviceBankResult.Sectors);
                    deviceStorageStructure.Index = bankSectors.Index;
                    deviceStorageStructure.Size = bankSectors.Size;
                    deviceStorageStructure.Address = bankSectors.Address;

                    //_logger?.LogTrace("GetStorageStructure: BanksNumber: {0}, SectorsNumber: {1}, Index: {2}, Size: {3}, Address: {4}", deviceStorageStructure.BanksNumber, deviceStorageStructure.SectorsNumber, deviceStorageStructure.Index, deviceStorageStructure.Size, deviceStorageStructure.Address);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "GetStorageStructure:");
            }
            finally
            {
                Marshal.FreeCoTaskMem(bankSectorPtr);
                Marshal.FreeCoTaskMem(deviceBankPtr);
                Marshal.FreeCoTaskMem(storageStructurePtr);
            }

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
            this._logger?.LogTrace("SendOptionBytesCmd: {0} result: {1}", command, output);
            return output;
        }

        /// <inheritdoc />
        public PeripheralC? InitOptionBytesInterface()
        {
            this._logger?.LogTrace("InitOptionBytesInterface.");
            PeripheralC? generalInf = null;

            var pointer = Native.ProgrammerApi.InitOptionBytesInterface();

            try
            {

                generalInf = Marshal.PtrToStructure<PeripheralC>(pointer);

            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "InitOptionBytesInterface: ");
            }
            finally
            {
                Marshal.DestroyStructure<PeripheralC>(pointer);
            }

            return generalInf;
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
        public void SetExternalLoadersPath(string path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void GetExternalLoaders()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void RemoveExternalLoader()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void DeleteLoaders()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region [STM32WB specific]

        /// Specific APIs used exclusively for STM32WB series to manage BLE Stack and they are available only through USB DFU and UART bootloader interfaces,
        /// except for the “firmwareDelete" and the “firmwareUpgrade", available through USB DFU, UART and SWD interfaces.
        /// Connection under Reset is mandatory.

        /// <inheritdoc />
        public void GetUID64()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void FirmwareDelete()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void FirmwareUpgrade()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void StartWirelessStack()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void UpdateAuthKey()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void AuthKeyLock()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void WriteUserKey()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void AntiRollBack()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void StartFus()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void UnlockChip()
        {
            throw new NotImplementedException();
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

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //private void ReceiveMessage(int messageType, [MarshalAs(UnmanagedType.LPWStr)] string message)
        //{

        //    message = Regex.Replace(message, "(?<!\r)\n", "");
        //    if (String.IsNullOrEmpty(message))
        //    {
        //        return;
        //    }

        //    switch ((MessageType) messageType)
        //    {
        //        case MessageType.Normal:
        //            this._logger?.LogTrace(@"{0}", message);
        //            break;

        //        case MessageType.Info:
        //            this._logger?.LogDebug(@"{0}", message);
        //            break;

        //        case MessageType.GreenInfo:
        //            this._logger?.LogInformation(@"{0}", message);
        //            break;

        //        case MessageType.Title:
        //            this._logger?.LogInformation(@"{0}", message);
        //            break;

        //        case MessageType.Warning:
        //            this._logger?.LogWarning(@"{0}", message);
        //            break;

        //        case MessageType.Error:
        //            this._logger?.LogError(@"{0}", message);
        //            break;

        //        case MessageType.Verbosity1:
        //        case MessageType.Verbosity2:
        //        case MessageType.Verbosity3:
        //            //_logger.LogTrace(@"{0}", message);
        //            break;

        //        case MessageType.GreenInfoNoPopup:
        //        case MessageType.WarningNoPopup:
        //        case MessageType.ErrorNoPopup:
        //            break;

        //        default:
        //            break;
        //    }
        //}

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //private void InitProgressBar()
        //{
        //    //System.Console.WriteLine("InitProgressBar: ");
        //    //_logger.LogDebug("Application InitProgressBar.");

        //    //lock (_logger)
        //    //{
        //    //    //System.Console.WriteLine("Application InitProgressBar.");
        //    //    _logger.LogDebug("Application InitProgressBar.");
        //    //}
        //}

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //private void ProgressBarUpdate(int currentProgress, int total)
        //{
        //    //System.Console.WriteLine("ProgressBarUpdate: " + currentProgress + " " + total);
        //    //_logger.LogDebug("Application ProgressBarUpdate: {0} on {1}", currentProgress, total);

        //    //lock (_logger)
        //    //{
        //    //    _logger.LogDebug("Application ProgressBarUpdate: {0} on {1}", currentProgress, total);
        //    //    //System.Console.WriteLine("Application ProgressBarUpdate: {0} on {1}", currentProgress, total);
        //    //}
        //}

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

        #region [EventWrapper]

        protected void OnStLinksFoundStatus()
        {
            this._logger?.LogTrace("OnStLinkFoundStatus");
            try
            {
                this.StLinksFoundStatus?.Invoke(this, new StLinkFoundEventArgs());
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "OnStLinkFoundStatus: ");
            }
        }

        protected void OnStLinkAdded()
        {
            this._logger?.LogTrace("OnStLinkAdded");
            try
            {
                this.StLinkAdded?.Invoke(this, new StLinkAddedEventArgs());
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "OnStLinkAdded: ");
            }
        }

        protected void OnStLinkRemoved()
        {
            this._logger?.LogTrace("OnStLinkRemoved");
            try
            {
                this.StLinkRemoved?.Invoke(this, new StLinkRemovedEventArgs());
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "OnStLinkRemoved: ");
            }
        }

        protected void OnStm32BootLoadersFoundStatus()
        {
            this._logger?.LogTrace("OnStm32BootLoadersFoundStatus");
            try
            {
                this.Stm32BootLoaderFoundStatus?.Invoke(this, new Stm32BootLoaderFoundEventArgs());
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "OnStm32BootLoadersFoundStatus: ");
            }
        }

        protected void OnStm32BootLoaderAdded()
        {
            this._logger?.LogTrace("OnStm32BootLoaderAdded");
            try
            {
                this.Stm32BootLoaderAdded?.Invoke(this, new Stm32BootLoaderAddedEventArgs());
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "OnStm32BootLoaderAdded: ");
            }
        }

        protected void OnStm32BootLoaderRemoved()
        {
            this._logger?.LogTrace("OnStm32BootLoaderRemoved");
            try
            {
                this.Stm32BootLoaderRemoved?.Invoke(this, new Stm32BootLoaderRemovedEventArgs());
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, "OnStm32BootLoaderRemoved: ");
            }
        }

        #endregion

        protected override void DisposeManagedResources()
        {
            this.WmiManager.Dispose();
        }

        protected override void DisposeUnmanagedResources()
        {
            this._handle?.Dispose();
        }
    } // CubeProgrammerApi.
}
