// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    //[StructLayout(LayoutKind.Explicit, Size = 48/*, CharSet = CharSet.Ansi, Size = 312*/)]
    [StructLayout(LayoutKind.Sequential)]
    public class SwdFrequency
    {
        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(0)]
        [MarshalAs(UnmanagedType.U4)]
        public uint SwdFrequency00;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(4)]
        [MarshalAs(UnmanagedType.U4)]
        public uint SwdFrequency01;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(8)]
        [MarshalAs(UnmanagedType.U4)]
        public uint SwdFrequency02;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(12)]
        [MarshalAs(UnmanagedType.U4)]
        public uint SwdFrequency03;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(16)]
        [MarshalAs(UnmanagedType.U4)]
        public uint SwdFrequency04;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(20)]
        [MarshalAs(UnmanagedType.U4)]
        public uint SwdFrequency05;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(24)]
        [MarshalAs(UnmanagedType.U4)]
        public uint SwdFrequency06;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(28)]
        [MarshalAs(UnmanagedType.U4)]
        public uint SwdFrequency07;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(32)]
        [MarshalAs(UnmanagedType.U4)]
        public uint SwdFrequency08;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(36)]
        [MarshalAs(UnmanagedType.U4)]
        public uint SwdFrequency09;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(40)]
        [MarshalAs(UnmanagedType.U4)]
        public uint SwdFrequency10;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(44)]
        [MarshalAs(UnmanagedType.U4)]
        public uint SwdFrequency11;
    }
}
