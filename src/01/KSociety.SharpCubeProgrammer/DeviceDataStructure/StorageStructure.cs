namespace KSociety.SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public class StorageStructure
    {
        public uint BanksNumber;
        public IntPtr Banks;
    }
}
