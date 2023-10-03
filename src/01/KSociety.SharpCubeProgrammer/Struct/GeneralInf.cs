#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace KSociety.SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class GeneralInf //: ICloneable
    {
        public ushort DeviceId;
        public int FlashSize;
        public int BootloaderVersion;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Type;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string Cpu;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Series;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 150)]
        public string Description;

        /// <summary>
        /// Revision ID.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string RevisionId;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Board;

        //public object Clone()
        //{
        //    return MemberwiseClone();
        //}
    }
}
