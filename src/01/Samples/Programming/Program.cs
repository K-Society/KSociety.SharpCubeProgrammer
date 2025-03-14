// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace Programming
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using Autofac;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Interface;

    internal class Program
    {
        private static IConfigurationRoot? Configuration;
        private static ILogger<Program>? Logger;
        private static ICubeProgrammerApi? CubeProgrammerApi;
        private static ICubeProgrammerApiAsync? CubeProgrammerApiAsync;

        private static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            Serilog.Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();

            var container = BuildContainer();

            Logger = container.Resolve<ILogger<Program>>();

            Logger.LogInformation("Programming console application ready.");
            Logger.LogDebug("Resolve ICubeProgrammerApi...");

            CubeProgrammerApi = container.Resolve<ICubeProgrammerApi>();
            CubeProgrammerApiAsync = container.Resolve<ICubeProgrammerApiAsync>();

            Console.WriteLine("Press a button to continue.");
            Console.ReadLine();

            //#region [Log Testing]

            //var displayCallBacks = new DisplayCallBacks
            //{
            //    InitProgressBar = InitProgressBar,
            //    LogMessage = ReceiveMessage,
            //    LoadBar = ProgressBarUpdate
            //};

            //CubeProgrammerApi.SetDisplayCallbacks(displayCallBacks);

            ////CubeProgrammerApi.SetDisplayCallbacks(InitProgressBar, ReceiveMessage, ProgressBarUpdate);
            //CubeProgrammerApi.SetVerbosityLevel(CubeProgrammerVerbosityLevel.CubeprogrammerVerLevelDebug);

            //#endregion

            //#region [External Loader Testing]

            //var deviceExternalStorageInfo = CubeProgrammerApi.GetExternalLoaders();


            //if (deviceExternalStorageInfo.HasValue)
            //{
            //    var externalStorage = deviceExternalStorageInfo.Value.ExternalLoader.FirstOrDefault();

            //    var externalLoader = CubeProgrammerApi.SetExternalLoaderPath(externalStorage.filePath);

            //    if (externalLoader.HasValue)
            //    {
            //        CubeProgrammerApi.RemoveExternalLoader(externalLoader.Value.filePath);
            //    }
            //}

            //#endregion

            #region [TryConnectStLink]

            //var uartList = CubeProgrammerApi.GetUsartList();

            var tryConnectionResult = CubeProgrammerApi.TryConnectStLink();

            if (tryConnectionResult.Equals(CubeProgrammerError.CubeprogrammerNoError))
            {
                //var bu = CubeProgrammerApi.InitOptionBytesInterface();
                #region [File Open Testing]

                var file = System.IO.File.ReadAllBytes(@"..\..\..\..\..\Test\BIN_32001506_REU_04800004.bin");
                //var filePointer = CubeProgrammerApi.FileOpen(@"..\..\..\..\..\Test\BIN_32001505_CEU_02000005.bin");
                //var buffer = new byte[65537];
                //var buffer = new byte[88996];//2048
                //var buffer = new byte[89000];//2048

                //for (int i = 0; i < 88064; i++)
                //{
                //    buffer[i] = 0xDD;
                //}

                //for (int i = 88064; i < 88996; i++)
                //{
                //    buffer[i] = 0x05;
                //}

                //var buffer = new byte[932];
                //for (int i = 0; i < 932; i++)
                //{
                //    buffer[i] = 0x05;
                //}

                //if (filePointer != IntPtr.Zero)
                //{
                //CubeProgrammerApi.MassErase();
                //var verify = CubeProgrammerApi.WriteMemoryAndVerify("0x08000400", buffer);
                var verify = CubeProgrammerApi.WriteMemoryAndVerify("0x08036000", file);
                //var verify = CubeProgrammerApi.WriteMemoryAndVerify("0x08015800", buffer);

                if (verify.Equals(CubeProgrammerError.CubeprogrammerNoError))
                    {
                    //var saveFileToFileTest = CubeProgrammerApi.SaveFileToFile(filePointer, @"..\..\..\..\..\Test\NUCLEO-L452RE-Test.bin");
                    ;
                        //if (saveFileToFileTest.Equals(CubeProgrammerError.CubeprogrammerNoError))
                        //{

                        //}
                    }

                    //CubeProgrammerApi.FreeFileData(filePointer);
                //}

                #endregion

                #region [Verify Memory Testing memory leak]

                //var openFile1 = CubeProgrammerApi.FileOpen(@"..\..\..\..\..\Test\NUCLEO-L452RE.bin");
                //var openFile = CubeProgrammerApi.FileOpen(@"..\..\..\..\..\Test\NUCLEO-L452RE.bin");
                //var length = firmware.Length;
                //for (var i = 0; i < 200; i++)
                //{
                //var massEraseResult = CubeProgrammerApi.MassErase();

                //if (massEraseResult.Equals(CubeProgrammerError.CubeprogrammerNoError))
                //{
                //var writeMemoryResult = CubeProgrammerApi.WriteMemory("0x08000000", firmware);

                //if (writeMemoryResult.Equals(CubeProgrammerError.CubeprogrammerNoError))
                //{
                for (var i = 0; i < 100; i++)
                {
                    //var tryConnectionResult = CubeProgrammerApi.TryConnectStLink();
                    //if (tryConnectionResult.Equals(CubeProgrammerError.CubeprogrammerNoError))
                    //{
                    //var readMemoryResult = CubeProgrammerApi.Verify(openFile, "0x08000000");
                    
                    

                    //var verifyMemoryResult = CubeProgrammerApi.WriteMemoryAndVerify("0x08000000", firmware);

                    //if (verifyMemoryResult.Equals(CubeProgrammerError.CubeprogrammerNoError))
                    //{

                    //}
                    //Logger.LogInformation("{0}: {1}", i, verifyMemoryResult);
                    //CubeProgrammerApi.Disconnect();
                    //CubeProgrammerApi.DeleteInterfaceList();
                    //}
                }
                //}
                //}
                //}

                //var filePointer = CubeProgrammerApi.FileOpenAsPointer(@"..\..\..\..\..\Test\NUCLEO-L452RE.hex");

                //if (filePointer != IntPtr.Zero)
                //{
                //    var verify = CubeProgrammerApi.Verify(filePointer, "0x08000000");

                //    if (verify.Equals(CubeProgrammerError.CubeprogrammerNoError))
                //    {
                //        var saveFileToFileTest = CubeProgrammerApi.SaveFileToFile(filePointer, @"..\..\..\..\..\Test\NUCLEO-L452RE-Test.bin");

                //        if (saveFileToFileTest.Equals(CubeProgrammerError.CubeprogrammerNoError))
                //        {

                //        }
                //    }

                //    CubeProgrammerApi.FreeFileData(filePointer);
                //}

                #endregion
            }
            

            CubeProgrammerApi.Disconnect();

            #endregion

            var stLinkList = CubeProgrammerApi.GetStLinkEnumerationList();
            if (stLinkList.Any())
            {
                var stLink = stLinkList.First();
                stLink.ConnectionMode = DebugConnectionMode.UnderResetMode;
                stLink.Shared = 0;

                Logger.LogInformation("Speed: {0}", stLink.Speed);
                var connectionResult = CubeProgrammerApi.ConnectStLink(stLink);

                if (connectionResult.Equals(CubeProgrammerError.CubeprogrammerNoError))
                {
                    var generalInfo = CubeProgrammerApi.GetDeviceGeneralInf();
                    if (generalInfo != null)
                    {
                        Logger.LogInformation("INFO: \n" +
                                              "Board: {0} \n" +
                                              "Bootloader Version: {1} \n" +
                                              "Cpu: {2} \n" +
                                              "Description: {3} \n" +
                                              "DeviceId: {4} \n" +
                                              "FlashSize: {5} \n" +
                                              "RevisionId: {6} \n" +
                                              "Name: {7} \n" +
                                              "Series: {8} \n" +
                                              "Type: {9}",
                            generalInfo.Value.Board,
                            generalInfo.Value.BootloaderVersion,
                            generalInfo.Value.Cpu,
                            generalInfo.Value.Description,
                            generalInfo.Value.DeviceId,
                            generalInfo.Value.FlashSize,
                            generalInfo.Value.RevisionId,
                            generalInfo.Value.Name,
                            generalInfo.Value.Series,
                            generalInfo.Value.Type);
                    }

                    var storageStructure = CubeProgrammerApi.GetStorageStructure();

                    if (storageStructure.Item1.Equals(CubeProgrammerError.CubeprogrammerNoError))
                    {
                        //var storage = storageStructure.Item2;
                        Logger.LogInformation("Storage structure: \n" +
                                              "BanksNumber: {0} \n",
                                                storageStructure.Item2.BanksNumber);

                        for (var i = 0; i < storageStructure.Item2.BanksNumber; i++)
                        {
                            var bank = storageStructure.Item2.Banks[i];
                            Logger.LogInformation("Bank [{0}] \n" +
                                "Sector number: {1}", i, bank.SectorsNumber);

                            for (var ii = 0; ii < bank.SectorsNumber; ii++)
                            {
                                var sector = bank.Sectors[ii];
                                Logger.LogInformation("Sector [{0}] \n" +
                                                      "Sector address: {1} \n" +
                                                      "Sector index: {2} \n" +
                                                      "Sector size: {3} \n", ii, CubeProgrammerApi.HexConverterToString(sector.Address), sector.Index, sector.Size);
                            }
                        }
                    }

                    var sendOptionBytesCmd = CubeProgrammerApi.SendOptionBytesCmd("-ob RDP=170");

                    if (sendOptionBytesCmd.Equals(CubeProgrammerError.CubeprogrammerNoError))
                    {

                    }

                    #region [DownloadFile Test]

                    //var downloadFileResult = CubeProgrammerApi.DownloadFile(
                    //    @"..\..\..\..\..\Test\NUCLEO-WBA52CG.bin", "0x08000000");

                    //if (downloadFileResult.Equals(CubeProgrammerError.CubeprogrammerNoError))
                    //{

                    //}

                    #endregion

                    #region [Memory Leak Test]

                    //var firmware = File.ReadAllBytes(@"..\..\..\..\..\Test\NUCLEO-L452RE.bin");
                    //for (var i = 0; i < 20; i++)
                    //{
                    //    var massEraseResult = CubeProgrammerApi.MassErase();

                    //    if (massEraseResult.Equals(CubeProgrammerError.CubeprogrammerNoError))
                    //    {
                    //        var writeMemoryResult = CubeProgrammerApi.WriteMemory("0x08000000", firmware);

                    //        if (writeMemoryResult.Equals(CubeProgrammerError.CubeprogrammerNoError))
                    //        {

                    //        }
                    //    }
                    //}

                    #endregion

                    var peripheral = CubeProgrammerApi.InitOptionBytesInterface();

                    if (peripheral.HasValue)
                    {
                        Logger.LogInformation("PeripheralC: \n" +
                                              "Name: {0} \n" +
                                              "Description: {1} \n" +
                                              "Banks Nbr: {2} \n",
                            peripheral.Value.Name,
                            peripheral.Value.Description,
                            peripheral.Value.BanksNbr);
                    }


                    //if (generalInfo.HasValue)
                    //{
                    //    var peripheral = CubeProgrammerApi.FastRomInitOptionBytesInterface(generalInfo.Value.DeviceId);

                    //    if (peripheral.HasValue)
                    //    {
                    //        Logger.LogInformation("PeripheralC: \n" +
                    //                              "Name: {0} \n" +
                    //                              "Description: {1} \n" +
                    //                              "Banks Nbr: {2} \n" +
                    //                              "Banks: {3} \n",
                    //            peripheral.Value.Name,
                    //            peripheral.Value.Description,
                    //            peripheral.Value.BanksNbr,
                    //            peripheral.Value.Banks);
                    //    }
                    //}

                    var targetInterfaceType = CubeProgrammerApi.GetTargetInterfaceType();

                    Logger.LogInformation("TargetInterfaceType: {0}", targetInterfaceType);

                    CubeProgrammerApi.Disconnect();
                }
                else
                {
                    CubeProgrammerApi.Disconnect();
                    Logger.LogWarning(connectionResult.ToString());
                }
            }
            else
            {
                Logger?.LogWarning("No ST-Link found!");
            }
            CubeProgrammerApi.DeleteInterfaceList();
            CubeProgrammerApi.Dispose();
            Console.WriteLine("Press a button to exit.");
            Console.ReadLine();
            
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new Bindings.Log());
            builder.RegisterModule(new Bindings.ProgrammerApi());
            return builder.Build();
        }

        private static void ReceiveMessage(int messageType, [MarshalAs(UnmanagedType.LPWStr)] string message)
        {
            //Logger?.LogTrace(message);
            message = Regex.Replace(message, "(?<!\r)\n", "");
            if (String.IsNullOrEmpty(message))
            {
                return;
            }

            switch ((MessageType)messageType)
            {
                case MessageType.Normal:
                    Logger?.LogTrace("Message: {0}", message);
                    break;

                case MessageType.Info:
                    Logger?.LogDebug("Message: {0}", message);
                    break;

                case MessageType.GreenInfo:
                    Logger?.LogInformation("Message: {0}", message);
                    break;

                case MessageType.Title:
                    Logger?.LogInformation("Message: {0}", message);
                    break;

                case MessageType.Warning:
                    Logger?.LogWarning("Message: {0}", message);
                    break;

                case MessageType.Error:
                    Logger?.LogError("Message: {0}", message);
                    break;

                case MessageType.Verbosity1:
                case MessageType.Verbosity2:
                case MessageType.Verbosity3:
                    Logger?.LogTrace("Message: {0}", message);
                    break;

                case MessageType.GreenInfoNoPopup:
                case MessageType.WarningNoPopup:
                case MessageType.ErrorNoPopup:
                    Logger?.LogTrace("Message: {0}", message);
                    break;

                default:
                    break;
            }
        }

        private static void InitProgressBar()
        {
            //Logger?.LogTrace("InitProgressBar");
            
        }

        private static void ProgressBarUpdate(int currentProgress, int total)
        {
            if (total > 0)
            {
                //can use current variable to set a progress bar, I have noticed that currentProgress is advancing only by write or download 
                //operation, erase operation does not produce advance
                var current = (currentProgress * 100F) / total;

                Logger?.LogInformation("ProgressBarUpdate: {0}", current);
            }
        }
    }
}
