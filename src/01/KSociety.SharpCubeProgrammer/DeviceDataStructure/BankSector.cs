namespace KSociety.SharpCubeProgrammer.DeviceDataStructure
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public class BankSector
    {
        public uint Index;
        public uint Size;
        public uint Address;
    }
}
