// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\DeviceSector.xml' path='docs/members[@name="deviceSector"]/DeviceSector/*'/>
    [StructLayout(LayoutKind.Sequential)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct DeviceSector
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\DeviceSector.xml' path='docs/members[@name="deviceSector"]/SectorNumber/*'/>
        public uint SectorNumber;

        /// <include file='..\Doc\DeviceSector.xml' path='docs/members[@name="deviceSector"]/SectorSize/*'/>
        public uint SectorSize;
    }
}
