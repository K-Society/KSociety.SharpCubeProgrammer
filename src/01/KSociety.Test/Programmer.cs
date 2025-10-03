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
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Interface;
    using SharpCubeProgrammer.Struct;
    using Xunit;
    using Xunit.Abstractions;

    public class Programmer
    {
        private readonly Serilog.ILogger _output;
        private readonly ICubeProgrammerApi _cubeProgrammerApi;
        private readonly bool _isConnected;
        private readonly ILogger<Programmer> _logger;
        private readonly ILogger<CubeProgrammerApi> _loggerCubeProgrammerApi;

        public Programmer(ITestOutputHelper output)
        {
            this._output = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(output, Serilog.Events.LogEventLevel.Verbose)
                .CreateLogger()
                .ForContext<Programmer>()
                .ForContext<CubeProgrammerApi>();

            this._logger = new SerilogLoggerFactory(this._output)
                .CreateLogger<Programmer>();

            this._loggerCubeProgrammerApi = new SerilogLoggerFactory(this._output)
                .CreateLogger<CubeProgrammerApi>();

            this._cubeProgrammerApi = new CubeProgrammerApi(this._loggerCubeProgrammerApi);

            this._isConnected = this.Connect();
        }

        public bool Connect()
        {
            try
            {
                var test = this._cubeProgrammerApi.WindowsVersion();

                var stLinkList = this._cubeProgrammerApi.GetStLinkList();

                if (!stLinkList.Any())
                {
                    return false;
                }

                var stLink = (DebugConnectParameters)stLinkList.First();
                stLink.ConnectionMode = DebugConnectionMode.UnderResetMode;

                var connectionResult = this._cubeProgrammerApi.ConnectStLink(stLink);

                if (!connectionResult.Equals(CubeProgrammerError.CubeprogrammerNoError))
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
                Assert.Equal(CubeProgrammerError.CubeprogrammerNoError, eraseResult.Item1);

                //var result = eraseResult.Item2;


                this._cubeProgrammerApi.Disconnect(); //Do not disconnect after execute, with udf.


            }

            watch.Stop();
            //var elapsed = watch.Elapsed.Seconds;
        }
    }
}
