// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System;
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\FileDataC.xml' path='docs/members[@name="fileDataC"]/FileDataC/*'/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct FileDataC
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\FileDataC.xml' path='docs/members[@name="fileDataC"]/Type/*'/>
        public int Type;

        /// <include file='..\Doc\FileDataC.xml' path='docs/members[@name="fileDataC"]/SegmentsNumber/*'/>
        public int SegmentsNumber;

        /// <include file='..\Doc\FileDataC.xml' path='docs/members[@name="fileDataC"]/Segments/*'/>
        public IntPtr Segments;
    }
}
