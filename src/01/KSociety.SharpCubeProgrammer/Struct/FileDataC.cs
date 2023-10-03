namespace KSociety.SharpCubeProgrammer.Struct
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class FileDataC
    {
        public int Type;
        public int segmentsNbr;
        public IntPtr segments;
    }
}
