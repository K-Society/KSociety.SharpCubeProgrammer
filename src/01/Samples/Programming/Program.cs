// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace Programming
{
    using System;
    using System.Linq;
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
            //CubeProgrammerApi.Start();
            //CubeProgrammerApi.StLinkAdded += CubeProgrammerApiOnStLinkAdded;
            //CubeProgrammerApi.StLinkRemoved += CubeProgrammerApiOnStLinkRemoved;
            //CubeProgrammerApi.StLinksFoundStatus += CubeProgrammerApiOnStLinksFoundStatus;

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

            var stLinkList = CubeProgrammerApi.GetStLinkList();
            if (stLinkList.Any())
            {
                var stLink = (DebugConnectParameters)stLinkList.First().Clone();
                stLink.ConnectionMode = KSociety.SharpCubeProgrammer.Enum.DebugConnectionMode.HotplugMode;
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
                        Logger.LogInformation("Storage structure: \n" +
                                              "Address: {0} \n" +
                                              "BanksNumber: {1} \n" +
                                              "Index: {2} \n" +
                                              "Sectors number: {3} \n" +
                                              "Size: {4} \n",
                            CubeProgrammerApi.HexConverterToString(storageStructure.Item2.Address),
                            storageStructure.Item2.BanksNumber,
                            storageStructure.Item2.Index,
                            storageStructure.Item2.SectorsNumber,
                            storageStructure.Item2.Size);
                    }

                    CubeProgrammerApi.Disconnect();
                }
                else
                {
                    Logger.LogWarning(connectionResult.ToString());
                }
            }
            else
            {
                Logger.LogWarning("No ST-Link found!");
            }

            CubeProgrammerApi.Dispose();

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
    }
}
