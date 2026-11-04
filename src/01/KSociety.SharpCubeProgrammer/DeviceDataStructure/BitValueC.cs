// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct BitValueC
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public uint Value;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1000)]
        public string Description;
    }
}
