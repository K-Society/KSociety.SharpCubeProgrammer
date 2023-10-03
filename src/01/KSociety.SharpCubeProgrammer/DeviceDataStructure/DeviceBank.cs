namespace KSociety.SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public class DeviceBank
    {
        public uint SectorsNumber;
        public IntPtr Sectors;
    }
}
