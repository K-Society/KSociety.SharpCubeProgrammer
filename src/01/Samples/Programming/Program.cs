// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace Programming
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using Autofac;
    using KSociety.SharpCubeProgrammer.Enum;
    using KSociety.SharpCubeProgrammer.Events;
    using KSociety.SharpCubeProgrammer.Interface;
    using KSociety.SharpCubeProgrammer.Struct;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Serilog;

    internal class Program
    {
        private static IConfigurationRoot Configuration;
        private static ILogger<Program> Logger;
        private static ICubeProgrammerApi CubeProgrammerApi;

        private static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            Serilog.Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();

            var container = BuildContainer();

            Logger = container.Resolve<ILogger<Program>>();

            Logger.LogInformation("Programming console application ready.");
            Logger.LogDebug("Resolve ICubeProgrammerApi...");

            CubeProgrammerApi = container.Resolve<ICubeProgrammerApi>();

            CubeProgrammerApi.StLinkAdded += CubeProgrammerApiOnStLinkAdded;
            CubeProgrammerApi.StLinkRemoved += CubeProgrammerApiOnStLinkRemoved;
            CubeProgrammerApi.StLinksFoundStatus += CubeProgrammerApiOnStLinksFoundStatus;
            Console.WriteLine("Press a button to continue.");
            Console.ReadLine();
            //var testStLink = CubeProgrammerApi.TryConnectStLink(0, 0, DebugConnectionMode.UnderResetMode);

            //if (testStLink.Equals(CubeProgrammerError.CubeprogrammerNoError))
            //{
            //    var generalInfo = CubeProgrammerApi.GetDeviceGeneralInf();
            //    if (generalInfo != null)
            //    {
            //        Logger.LogInformation("INFO: \n" +
            //                              "Board: {0} \n" +
            //                              "Bootloader Version: {1} \n" +
            //                              "Cpu: {2} \n" +
            //                              "Description: {3} \n" +
            //                              "DeviceId: {4} \n" +
            //                              "FlashSize: {5} \n" +
            //                              "RevisionId: {6} \n" +
            //                              "Name: {7} \n" +
            //                              "Series: {8} \n" +
            //                              "Type: {9}",
            //            generalInfo.Board,
            //            generalInfo.BootloaderVersion,
            //            generalInfo.Cpu,
            //            generalInfo.Description,
            //            generalInfo.DeviceId,
            //            generalInfo.FlashSize,
            //            generalInfo.RevisionId,
            //            generalInfo.Name,
            //            generalInfo.Series,
            //            generalInfo.Type);
            //    }
            //    CubeProgrammerApi.Disconnect();
            //}
            //else
            //{
            //    Logger.LogWarning(testStLink.ToString());
            //}

            //var path = @".\st\Programmer";
            //var result2 = CubeProgrammerApi.GetExternalLoaders(path);

            //Logger?.LogInformation("GetExternalLoaders: {0}", result2.Count());

            //foreach (var currentItem in result2)
            //{
            //    Logger?.LogTrace("GetExternalLoaders: device name: {0}, file path: {1}, device type: {2}, device size: {3}, start address: {4}, page size: {5}, sectors type: {6}",
            //        currentItem.deviceName, currentItem.filePath, currentItem.deviceType, CubeProgrammerApi.HexConverterToString(currentItem.deviceSize),
            //        CubeProgrammerApi.HexConverterToString(currentItem.deviceStartAddress), CubeProgrammerApi.HexConverterToString(currentItem.pageSize),
            //        currentItem.sectorsTypeNbr);
            //}


            #region [Log Testing]

            var displayCallBacks = new DisplayCallBacks
            {
                InitProgressBar = InitProgressBar,
                LogMessage = ReceiveMessage,
                LoadBar = ProgressBarUpdate
            };

            CubeProgrammerApi.SetDisplayCallbacks(displayCallBacks);

            //CubeProgrammerApi.SetDisplayCallbacks(InitProgressBar, ReceiveMessage, ProgressBarUpdate);

            CubeProgrammerApi.SetVerbosityLevel(CubeProgrammerVerbosityLevel.CubeprogrammerVerLevelDebug);

            #endregion

            var stLinkList = CubeProgrammerApi.GetStLinkEnumerationList();
            if (stLinkList.Any())
            {
                var stLink = (DebugConnectParameters)stLinkList.First().Clone();
                stLink.ConnectionMode = KSociety.SharpCubeProgrammer.Enum.DebugConnectionMode.UnderResetMode;
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

                        var storage = storageStructure.Item2;
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

                    //var uid64 = CubeProgrammerApi.GetUID64();
                    //var startFusREsult = CubeProgrammerApi.StartFus();


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

            CubeProgrammerApi.Dispose();
            Console.WriteLine("Press a button to exit.");
            Console.ReadLine();
            
        }

        private static void CubeProgrammerApiOnStLinksFoundStatus(object? sender, StLinkFoundEventArgs e)
        {
            Logger?.LogInformation("StLinksFound...");
        }

        private static void CubeProgrammerApiOnStLinkRemoved(object? sender, StLinkRemovedEventArgs e)
        {
            Logger?.LogInformation("StLinkRemoved...");
        }

        private static void CubeProgrammerApiOnStLinkAdded(object? sender, StLinkAddedEventArgs e)
        {
            Logger?.LogInformation("StLinkAdded...");
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new Bindings.Log());
            builder.RegisterModule(new KSociety.Wmi.Bindings.Wmi());
            builder.RegisterModule(new KSociety.SharpCubeProgrammer.Bindings.ProgrammerApi());
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
                    Logger.LogError("Message: {0}", message);
                    break;

                case MessageType.Verbosity1:
                case MessageType.Verbosity2:
                case MessageType.Verbosity3:
                    Logger.LogTrace("Message: {0}", message);
                    break;

                case MessageType.GreenInfoNoPopup:
                case MessageType.WarningNoPopup:
                case MessageType.ErrorNoPopup:
                    Logger.LogTrace("Message: {0}", message);
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

                Logger.LogInformation("ProgressBarUpdate: {0}", current);
            }
        }
    }
}
