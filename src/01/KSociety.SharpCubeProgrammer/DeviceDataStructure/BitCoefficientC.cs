namespace KSociety.SharpCubeProgrammer.DeviceDataStructure
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public class BitCoefficientC
    {
        public uint Multiplier;
        public uint Offset;
    }
}
