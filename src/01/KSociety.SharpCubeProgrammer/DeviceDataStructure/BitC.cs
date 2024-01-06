// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct BitC
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1000)]
        public string Description;

        public uint WordOffset;
        public uint BitOffset;
        public uint BitWidth;
        public byte Access;

        public uint ValuesNbr;

        public IntPtr Values;
        public BitCoefficientC Equation;
        public string Reference;
        public uint BitValue;
    }
}
