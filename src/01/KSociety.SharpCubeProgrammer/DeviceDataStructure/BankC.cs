namespace KSociety.SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public class BankC
    {
        public uint Size;
        public uint Address;
        public byte Access;
        public uint CategoriesNbr;
        public IntPtr Categories;
    }
}
