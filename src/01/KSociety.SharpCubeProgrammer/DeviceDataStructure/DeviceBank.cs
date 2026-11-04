// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct DeviceBank
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public uint SectorsNumber;
        public IntPtr Sectors;
    }
}
