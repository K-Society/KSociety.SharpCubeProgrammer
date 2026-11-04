// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct CategoryC
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Name;

        public uint BitsNbr;

        public IntPtr Bits;
    }
}
