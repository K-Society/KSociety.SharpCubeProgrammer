namespace KSociety.SharpCubeProgrammer.Struct
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class SegmentDataC
    {
        public int address;
        public int size;
        public IntPtr data;
    }
}
