// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceSector
    {
        /// <summary>
        /// Number of Sectors.
        /// </summary>
        public uint sectorNum;

        /// <summary>
        /// Sector Size in BYTEs.
        /// </summary>
        public uint sectorSize;
    }
}
