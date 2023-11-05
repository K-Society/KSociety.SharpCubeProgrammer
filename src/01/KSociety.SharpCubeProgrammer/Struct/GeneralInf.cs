// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct GeneralInf //: ICloneable
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
