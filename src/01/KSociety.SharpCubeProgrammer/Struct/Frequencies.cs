// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\Frequencies.xml' path='docs/members[@name="frequencies"]/Frequencies/*'/>
    [StructLayout(LayoutKind.Sequential)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct Frequencies
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {

        /// <include file='..\Doc\Frequencies.xml' path='docs/members[@name="frequencies"]/JTagFrequency/*'/>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public uint[] JTagFrequency;

        /// <include file='..\Doc\Frequencies.xml' path='docs/members[@name="frequencies"]/JTagFrequencyNumber/*'/>
        [MarshalAs(UnmanagedType.U4)]
        public uint JTagFrequencyNumber;

        /// <include file='..\Doc\Frequencies.xml' path='docs/members[@name="frequencies"]/SwdFrequency/*'/>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public uint[] SwdFrequency;

        /// <include file='..\Doc\Frequencies.xml' path='docs/members[@name="frequencies"]/SwdFrequencyNumber/*'/>
        [MarshalAs(UnmanagedType.U4)]
        public uint SwdFrequencyNumber;
    }
}
