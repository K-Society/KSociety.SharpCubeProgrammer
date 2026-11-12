// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\CategoryC.xml' path='docs/members[@name="categoryC"]/CategoryC/*'/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct CategoryC
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\CategoryC.xml' path='docs/members[@name="categoryC"]/Name/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Name;

        /// <include file='..\Doc\CategoryC.xml' path='docs/members[@name="categoryC"]/BitsNumber/*'/>
        public uint BitsNumber;

        /// <include file='..\Doc\CategoryC.xml' path='docs/members[@name="categoryC"]/Bits/*'/>
        public IntPtr Bits;
    }
}
