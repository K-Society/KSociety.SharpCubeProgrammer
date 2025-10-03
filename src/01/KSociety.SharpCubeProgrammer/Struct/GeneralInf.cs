// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Get device general informations.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct GeneralInf
    {
        /// <summary>
        /// Device ID.
        /// </summary>
        public ushort DeviceId;

        /// <summary>
        /// Flash memory size.
        /// </summary>
        public int FlashSize;

        /// <summary>
        /// Bootloader version.
        /// </summary>
        public int BootloaderVersion;

        /// <summary>
        /// Device MCU or MPU.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Type;

        /// <summary>
        /// Cortex CPU.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string Cpu;

        /// <summary>
        /// Device name.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Name;

        /// <summary>
        /// Device serie.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Series;

        /// <summary>
        /// Take notice.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 150)]
        public string Description;

        /// <summary>
        /// Revision ID.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string RevisionId;

        /// <summary>
        /// Board Rpn.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Board;
    }
}
