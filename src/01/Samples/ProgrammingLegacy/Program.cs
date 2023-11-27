namespace ProgrammingLegacy
{
    using System.Linq;
    using System;
    using Autofac;
    using KSociety.SharpCubeProgrammer.Enum;
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

        static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            Serilog.Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();

            var container = BuildContainer();

            Logger = container.Resolve<ILogger<Program>>();

            Logger.LogInformation("Programming console application ready.");
            Logger.LogDebug("Resolve ICubeProgrammerApi...");

            CubeProgrammerApi = container.Resolve<ICubeProgrammerApi>();

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

                    var peripheral = CubeProgrammerApi.InitOptionBytesInterface();

                    if (peripheral.HasValue)
                    {
                        Logger.LogInformation("PeripheralC: \n" +
                                              "Name: {0} \n" +
                                              "Description: {1} \n" +
                                              "Banks Nbr: {2} \n" +
                                              "Banks: {3} \n",
                            peripheral.Value.Name,
                            peripheral.Value.Description,
                            peripheral.Value.BanksNbr,
                            peripheral.Value.Banks);
                    }

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

            Console.ReadLine();
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
