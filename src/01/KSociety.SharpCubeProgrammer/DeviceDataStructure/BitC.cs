// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\BitC.xml' path='docs/members[@name="bitC"]/BitC/*'/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct BitC
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\BitC.xml' path='docs/members[@name="bitC"]/Name/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Name;

        /// <include file='..\Doc\BitC.xml' path='docs/members[@name="bitC"]/Description/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1000)]
        public string Description;

        /// <include file='..\Doc\BitC.xml' path='docs/members[@name="bitC"]/WordOffset/*'/>
        public uint WordOffset;

        /// <include file='..\Doc\BitC.xml' path='docs/members[@name="bitC"]/BitOffset/*'/>
        public uint BitOffset;

        /// <include file='..\Doc\BitC.xml' path='docs/members[@name="bitC"]/BitWidth/*'/>
        public uint BitWidth;

        /// <include file='..\Doc\BitC.xml' path='docs/members[@name="bitC"]/Access/*'/>
        public byte Access;

        /// <include file='..\Doc\BitC.xml' path='docs/members[@name="bitC"]/ValuesNumber/*'/>
        public uint ValuesNumber;

        /// <include file='..\Doc\BitC.xml' path='docs/members[@name="bitC"]/Values/*'/>
        public IntPtr Values;

        /// <include file='..\Doc\BitC.xml' path='docs/members[@name="bitC"]/Equation/*'/>
        public BitCoefficientC Equation;

        /// <include file='..\Doc\BitC.xml' path='docs/members[@name="bitC"]/Reference/*'/>
        public IntPtr Reference;

        /// <include file='..\Doc\BitC.xml' path='docs/members[@name="bitC"]/BitValue/*'/>
        public uint BitValue;

        /// <include file='..\Doc\BitC.xml' path='docs/members[@name="bitC"]/ValLine/*'/>
        public uint ValLine;
    }
}
