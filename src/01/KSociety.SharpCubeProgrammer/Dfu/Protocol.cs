// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Dfu
{
    using System;

    public static class Protocol
    {
        /// <summary>
        /// The version of the official DFU specification
        /// </summary>
        public static readonly Version LatestVersion = new Version(1, 1);

        /// <summary>
        /// ST Microelectronics-specific Extension (DFUSE) version number
        /// </summary>
        public static readonly Version SeVersion = new Version(0x1, 0x1a);
    }
}
