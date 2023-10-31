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

            var stLinkList = CubeProgrammerApi.GetStLinkList(true);
            if (stLinkList.Any())
            {
                var stLink = (DebugConnectParameters)stLinkList.First().Clone();
                stLink.ConnectionMode = KSociety.SharpCubeProgrammer.Enum.DebugConnectionMode.UnderResetMode;
                
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
                            generalInfo.Board,
                            generalInfo.BootloaderVersion,
                            generalInfo.Cpu,
                            generalInfo.Description,
                            generalInfo.DeviceId,
                            generalInfo.FlashSize,
                            generalInfo.RevisionId,
                            generalInfo.Name,
                            generalInfo.Series,
                            generalInfo.Type);
                    }
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

            CubeProgrammerApi.Disconnect();
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
