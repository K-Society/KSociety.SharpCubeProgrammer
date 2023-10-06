// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Test
{
    using System;
    using System.Linq;
    using Log.Serilog.Sinks.XUnit;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Extensions.Logging;
    using SharpCubeProgrammer;
    using SharpCubeProgrammer.Events;
    using SharpCubeProgrammer.Interface;
    using Wmi;
    using Xunit;
    using Xunit.Abstractions;

    public class Programmer
    {
        private readonly Serilog.ILogger _output;
        private readonly ICubeProgrammerApi _cubeProgrammerApi;
        private readonly bool _isConnected;
        private readonly ILogger<Programmer> _logger;
        private readonly ILogger<WmiManager> _loggerWmiManager;
        private readonly ILogger<CubeProgrammerApi> _loggerCubeProgrammerApi;

        public Programmer(ITestOutputHelper output)
        {
            this._output = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(output, Serilog.Events.LogEventLevel.Verbose)
                .CreateLogger()
                .ForContext<Programmer>()
                .ForContext<WmiManager>()
                .ForContext<CubeProgrammerApi>();

            this._logger = new SerilogLoggerFactory(this._output)
                .CreateLogger<Programmer>();

            this._loggerWmiManager = new SerilogLoggerFactory(this._output)
                .CreateLogger<WmiManager>();

            this._loggerCubeProgrammerApi = new SerilogLoggerFactory(this._output)
                .CreateLogger<CubeProgrammerApi>();

            IWmiManager wmiManager = new WmiManager(this._loggerWmiManager);

            this._cubeProgrammerApi = new CubeProgrammerApi(wmiManager, this._loggerCubeProgrammerApi);

            this._cubeProgrammerApi.StLinkAdded += this.CubeProgrammerApiOnStLinkAdded;
            this._cubeProgrammerApi.StLinkRemoved += this.CubeProgrammerApiOnStLinkRemoved;
            this._cubeProgrammerApi.StLinksFoundStatus += this.CubeProgrammerApiOnStLinksFoundStatus;

            //_isConnected = DfuConnect();
            this._isConnected = this.Connect();
            //ProgrammingInitOptionBytesInterface();
        }

        private void CubeProgrammerApiOnStLinksFoundStatus(object? sender, StLinkFoundEventArgs e)
        {
            this._logger?.LogTrace("CubeProgrammerApiOnStLinksFoundStatus: {0}", "OK");
        }

        private void CubeProgrammerApiOnStLinkRemoved(object? sender, StLinkRemovedEventArgs e)
        {
            this._logger?.LogTrace("CubeProgrammerApiOnStLinkRemoved: {0}", false);
        }

        private void CubeProgrammerApiOnStLinkAdded(object? sender, StLinkAddedEventArgs e)
        {
            this._logger?.LogTrace("CubeProgrammerApiOnStLinkAdded: {0}", true);
        }

        public bool Connect()
        {
            try
            {
                var stLinkList = this._cubeProgrammerApi.GetStLinkList();

                if (!stLinkList.Any())
                {
                    return false;
                }

                var stLink = (KSociety.SharpCubeProgrammer.Struct.DebugConnectParameters)stLinkList.First().Clone();
                stLink.ConnectionMode = KSociety.SharpCubeProgrammer.Enum.DebugConnectionMode.UnderResetMode;

                var connectionResult = this._cubeProgrammerApi.ConnectStLink(stLink);

                if (!connectionResult.Equals(KSociety.SharpCubeProgrammer.Enum.CubeProgrammerError.CubeprogrammerNoError))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                ;
            }

            return false;
        }

        private bool ProgrammingInitOptionBytesInterface()
        {
            var output = false;
            var optionBytesInterface = this._cubeProgrammerApi.InitOptionBytesInterface();
            if (optionBytesInterface != null)
            {
                output = true;
            }

            return output;
        }

        [Fact]
        public void GetStorageStructure()
        {
            Assert.True(this._isConnected);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            if (this._isConnected)
            {
                var eraseResult = this._cubeProgrammerApi.GetStorageStructure();
                Assert.Equal(KSociety.SharpCubeProgrammer.Enum.CubeProgrammerError.CubeprogrammerNoError, eraseResult.Item1);

                //var result = eraseResult.Item2;


                this._cubeProgrammerApi.Disconnect(); //Do not disconnect after execute, with udf.
                this._cubeProgrammerApi.DeleteInterfaceList();


            }

            watch.Stop();
            //var elapsed = watch.Elapsed.Seconds;
        }
    }
}
