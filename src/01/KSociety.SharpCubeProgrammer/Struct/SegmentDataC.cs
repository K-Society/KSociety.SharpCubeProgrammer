// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System;
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\SegmentDataC.xml' path='docs/members[@name="segmentDataC"]/SegmentDataC/*'/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct SegmentDataC
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\SegmentDataC.xml' path='docs/members[@name="segmentDataC"]/Address/*'/>
        public int Address;

        /// <include file='..\Doc\SegmentDataC.xml' path='docs/members[@name="segmentDataC"]/Size/*'/>
        public int Size;

        /// <include file='..\Doc\SegmentDataC.xml' path='docs/members[@name="segmentDataC"]/Data/*'/>
        public IntPtr Data;
    }
}
