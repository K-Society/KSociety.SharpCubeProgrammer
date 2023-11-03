// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    //[StructLayout(LayoutKind.Explicit, Size = 48/*, CharSet = CharSet.Ansi, Size = 312*/)]
    [StructLayout(LayoutKind.Sequential)]
    public class JTagFrequency
    {
        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(0)]
        [MarshalAs(UnmanagedType.U4)]
        public uint JTagFrequency00;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(4)]
        [MarshalAs(UnmanagedType.U4)]
        public uint JTagFrequency01;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(8)]
        [MarshalAs(UnmanagedType.U4)]
        public uint JTagFrequency02;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(12)]
        [MarshalAs(UnmanagedType.U4)]
        public uint JTagFrequency03;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(16)]
        [MarshalAs(UnmanagedType.U4)]
        public uint JTagFrequency04;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(20)]
        [MarshalAs(UnmanagedType.U4)]
        public uint JTagFrequency05;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(24)]
        [MarshalAs(UnmanagedType.U4)]
        public uint JTagFrequency06;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(28)]
        [MarshalAs(UnmanagedType.U4)]
        public uint JTagFrequency07;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(32)]
        [MarshalAs(UnmanagedType.U4)]
        public uint JTagFrequency08;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(36)]
        [MarshalAs(UnmanagedType.U4)]
        public uint JTagFrequency09;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(40)]
        [MarshalAs(UnmanagedType.U4)]
        public uint JTagFrequency10;

        //[MarshalAs(UnmanagedType.SysUInt)]
        //[FieldOffset(44)]
        [MarshalAs(UnmanagedType.U4)]
        public uint JTagFrequency11;
    }
}
