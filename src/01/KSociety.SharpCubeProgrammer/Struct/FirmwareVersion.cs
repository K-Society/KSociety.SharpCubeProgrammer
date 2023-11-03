// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    //using System.Runtime.InteropServices;

    //[StructLayout(LayoutKind.Explicit, Size = 20)]
    //public class FirmwareVersion
    //{
    //    [FieldOffset(0)]
    //    public byte FirmwareVersion00;

    //    [FieldOffset(1)]
    //    public byte FirmwareVersion01;

    //    [FieldOffset(2)]
    //    public byte FirmwareVersion02;

    //    [FieldOffset(3)]
    //    public byte FirmwareVersion03;

    //    [FieldOffset(4)]
    //    public byte FirmwareVersion04;

    //    [FieldOffset(5)]
    //    public byte FirmwareVersion05;

    //    [FieldOffset(6)]
    //    public byte FirmwareVersion06;

    //    [FieldOffset(7)]
    //    public byte FirmwareVersion07;

    //    [FieldOffset(8)]
    //    public byte FirmwareVersion08;

    //    [FieldOffset(9)]
    //    public byte FirmwareVersion09;

    //    [FieldOffset(10)]
    //    public byte FirmwareVersion10;

    //    [FieldOffset(11)]
    //    public byte FirmwareVersion11;

    //    [FieldOffset(12)]
    //    public byte FirmwareVersion12;

    //    [FieldOffset(13)]
    //    public byte FirmwareVersion13;

    //    [FieldOffset(14)]
    //    public byte FirmwareVersion14;

    //    [FieldOffset(15)]
    //    public byte FirmwareVersion15;

    //    [FieldOffset(16)]
    //    public byte FirmwareVersion16;

    //    [FieldOffset(17)]
    //    public byte FirmwareVersion17;

    //    [FieldOffset(18)]
    //    public byte FirmwareVersion18;

    //    [FieldOffset(19)]
    //    public byte FirmwareVersion19;
    //}

    //[StructLayout(LayoutKind.Sequential)]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi /*, Size = 312*/)]
    public class FirmwareVersion
    {
        //[FieldOffset(0)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion00;

        //[FieldOffset(1)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion01;

        //[FieldOffset(2)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion02;

        //[FieldOffset(3)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion03;

        //[FieldOffset(4)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion04;

        //[FieldOffset(5)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion05;

        //[FieldOffset(6)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion06;

        //[FieldOffset(7)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion07;

        //[FieldOffset(8)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion08;

        //[FieldOffset(9)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion09;

        //[FieldOffset(10)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion10;

        //[FieldOffset(11)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion11;

        //[FieldOffset(12)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion12;

        //[FieldOffset(13)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion13;

        //[FieldOffset(14)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion14;

        //[FieldOffset(15)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion15;

        //[FieldOffset(16)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion16;

        //[FieldOffset(17)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion17;

        //[FieldOffset(18)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion18;

        //[FieldOffset(19)]
        [MarshalAs(UnmanagedType.U1)]
        public char FirmwareVersion19;
    }
}
