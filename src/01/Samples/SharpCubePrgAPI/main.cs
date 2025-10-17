// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubePrgAPI
{
    using System;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Struct;

    internal class main
    {
        static int Main()
        {
            Console.WriteLine("==== API Progamming Examples ===== ");
            var cubeProgrammerApi = new SharpCubeProgrammer.CubeProgrammerApi();

            #region [Logging]

            /* Set the progress bar and message display functions callbacks */
            var displayCallBacks = new DisplayCallBacks
            {
                InitProgressBar = DisplayManager.InitProgressBar,
                LogMessage = DisplayManager.DisplayMessage,
                LoadBar = DisplayManager.ProgressBarUpdate
            };

            cubeProgrammerApi.SetDisplayCallbacks(displayCallBacks);

            /* Set DLL verbosity level */
            DisplayManager.VerbosityLevel = VerbosityLevel.VerbosityLevel1;
            cubeProgrammerApi.SetVerbosityLevel(DisplayManager.VerbosityLevel);

            #endregion
            var result = 0;
            result = Example1.Example(cubeProgrammerApi);
            //result = Example2.Example(cubeProgrammerApi);
            //result = Example3.Example(cubeProgrammerApi);
            //result = ExampleWB.Example(cubeProgrammerApi);

            cubeProgrammerApi.Dispose();

            Console.WriteLine("\nPress enter to continue...");           
            Console.ReadLine();

            return result;
        }
    }
}
