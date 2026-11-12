// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\PeripheralC.xml' path='docs/members[@name="peripheralC"]/PeripheralC/*'/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct PeripheralC
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\PeripheralC.xml' path='docs/members[@name="peripheralC"]/Name/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Name;

        /// <include file='..\Doc\PeripheralC.xml' path='docs/members[@name="peripheralC"]/Description/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1000)]
        public string Description;

        /// <include file='..\Doc\PeripheralC.xml' path='docs/members[@name="peripheralC"]/BanksNumber/*'/>
        public uint BanksNumber;

        /// <include file='..\Doc\PeripheralC.xml' path='docs/members[@name="peripheralC"]/Banks/*'/>
        public IntPtr Banks;
    }
}
