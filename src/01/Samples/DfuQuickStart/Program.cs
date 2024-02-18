// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace DfuQuickStart
{
    using System;

    internal class Program
    {
        static void Main(string[] args)
        {
            //var cubeProgrammerApi = new SharpCubeProgrammer.CubeProgrammerApi();

            Console.WriteLine("Press a button to start.");
            Console.ReadLine();

            var dfuFile = SharpCubeProgrammer.Dfu.FileFormat.Dfu.ParseFile("");
            ;
        }
    }
}
