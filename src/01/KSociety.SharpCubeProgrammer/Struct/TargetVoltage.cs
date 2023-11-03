// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    //using System.Runtime.InteropServices;

    //[StructLayout(LayoutKind.Explicit, Size = 5)]
    //public class TargetVoltage
    //{
    //    [FieldOffset(0)]
    //    public byte TargetVoltage00;

    //    [FieldOffset(1)]
    //    public byte TargetVoltage01;

    //    [FieldOffset(2)]
    //    public byte TargetVoltage02;

    //    [FieldOffset(3)]
    //    public byte TargetVoltage03;

    //    [FieldOffset(4)]
    //    public byte TargetVoltage04;
    //}

    //[StructLayout(LayoutKind.Sequential)]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi/*, CharSet = CharSet.Ansi, Size = 312*/)]
    public class TargetVoltage
    {
        //[FieldOffset(0)]
        [MarshalAs(UnmanagedType.U1)]
        public char TargetVoltage00;

        //[FieldOffset(1)]
        [MarshalAs(UnmanagedType.U1)]
        public char TargetVoltage01;

        //[FieldOffset(2)]
        [MarshalAs(UnmanagedType.U1)]
        public char TargetVoltage02;

        //[FieldOffset(3)]
        [MarshalAs(UnmanagedType.U1)]
        public char TargetVoltage03;

        //[FieldOffset(4)]
        [MarshalAs(UnmanagedType.U1)]
        public char TargetVoltage04;
    }
}
