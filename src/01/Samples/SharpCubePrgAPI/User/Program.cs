// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubePrgAPI.User
{
    using System;
    using System.Runtime.InteropServices;
    using SharpCubePrgAPI.Bootoader;
    using SharpCubePrgAPI.HSM;
    using SharpCubePrgAPI.StLink;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Struct;

    internal class Program
    {
        static int Main()
        {
            Console.WriteLine("==== API Progamming Examples ===== ");
            var cubeProgrammerApi = new SharpCubeProgrammer.CubeProgrammerApi();

            #region [Logging]

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                /* Set the progress bar and message display functions callbacks */
                var displayCallBacks = new DisplayCallBacks
                {
                    InitProgressBar = DisplayManager.InitProgressBar,
                    LogMessage = DisplayManager.DisplayMessage,
                    LoadBar = DisplayManager.ProgressBarUpdate
                };

                cubeProgrammerApi.SetDisplayCallbacks(displayCallBacks);
            }else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                /* Set the progress bar and message display functions callbacks */
                var displayCallBacks = new DisplayCallBacksLinux
                {
                    InitProgressBar = DisplayManager.InitProgressBar,
                    LogMessage = DisplayManager.DisplayMessageLinux,
                    LoadBar = DisplayManager.ProgressBarUpdate
                };

                cubeProgrammerApi.SetDisplayCallbacks(displayCallBacks);
            }



                /* Set DLL verbosity level */
                DisplayManager.VerbosityLevel = VerbosityLevel.VerbosityLevel1;
            cubeProgrammerApi.SetVerbosityLevel(DisplayManager.VerbosityLevel);

            #endregion
            var result = 0;
            result = Example1.Example(cubeProgrammerApi); //Tested
            //result = Example2.Example(cubeProgrammerApi); //Tested
            //result = Example3.Example(cubeProgrammerApi); //Tested
            //result = ExampleWB.Example(cubeProgrammerApi); //Tested
            //result = UartExample.Example(cubeProgrammerApi); //Not Tested
            //result = CanExample.Example(cubeProgrammerApi); //Not Tested
            //result = I2cExample.Example(cubeProgrammerApi); //Not Tested
            //result = SpiExample.Example(cubeProgrammerApi); //Not Tested
            //result = UsbExample.Example(cubeProgrammerApi); //Tested
            //result = TsvFlashing.Example(cubeProgrammerApi); //Not Tested
            //result = MpuSsp.Example(cubeProgrammerApi); //Not Tested
            //result = HSMExample.Example(cubeProgrammerApi); //Not Tested

            cubeProgrammerApi.Dispose();

            Console.WriteLine("\nPress enter to continue...");           
            Console.ReadLine();

            return result;
        }
    }
}
