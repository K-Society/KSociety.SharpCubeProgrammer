namespace KSociety.SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class DeviceSector
    {
        /// <summary>
        /// Number of Sectors.
        /// </summary>
        public uint sectorNum;

        /// <summary>
        /// Sector Size in BYTEs.
        /// </summary>
        public uint sectorSize;
    }
}
