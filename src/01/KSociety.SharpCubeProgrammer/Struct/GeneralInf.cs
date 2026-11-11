// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\GeneralInf.xml' path='docs/members[@name="generalInf"]/GeneralInf/*'/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct GeneralInf
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\GeneralInf.xml' path='docs/members[@name="generalInf"]/DeviceId/*'/>
        public ushort DeviceId;

        /// <include file='..\Doc\GeneralInf.xml' path='docs/members[@name="generalInf"]/FlashSize/*'/>
        public int FlashSize;

        /// <include file='..\Doc\GeneralInf.xml' path='docs/members[@name="generalInf"]/BootloaderVersion/*'/>
        public int BootloaderVersion;

        /// <include file='..\Doc\GeneralInf.xml' path='docs/members[@name="generalInf"]/Type/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Type;

        /// <include file='..\Doc\GeneralInf.xml' path='docs/members[@name="generalInf"]/Cpu/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string Cpu;

        /// <include file='..\Doc\GeneralInf.xml' path='docs/members[@name="generalInf"]/Name/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Name;

        /// <include file='..\Doc\GeneralInf.xml' path='docs/members[@name="generalInf"]/Series/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Series;

        /// <include file='..\Doc\GeneralInf.xml' path='docs/members[@name="generalInf"]/Description/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 150)]
        public string Description;

        /// <include file='..\Doc\GeneralInf.xml' path='docs/members[@name="generalInf"]/RevisionId/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string RevisionId;

        /// <include file='..\Doc\GeneralInf.xml' path='docs/members[@name="generalInf"]/Board/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Board;
    }
}
