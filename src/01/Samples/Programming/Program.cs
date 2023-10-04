// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace Programming
{
    using System;
    using System.Linq;
    using Autofac;
    using KSociety.SharpCubeProgrammer.Events;
    using KSociety.SharpCubeProgrammer.Interface;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Serilog;

    internal class Program
    {
        private static IConfigurationRoot? Configuration;
        private static ILogger<Program>? Logger;
        private static ICubeProgrammerApi? CubeProgrammerApi;

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

            var stLinkList = CubeProgrammerApi.GetStLinkList();
            if (stLinkList.Any())
            {
                var stLink = (KSociety.SharpCubeProgrammer.Struct.DebugConnectParameters)stLinkList.First().Clone();
                var connectionResult = CubeProgrammerApi.ConnectStLink(stLink);
            }
            else
            {
                Logger.LogWarning("No ST-Link found!");
            }
            


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
