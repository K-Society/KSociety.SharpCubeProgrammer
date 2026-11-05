// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Get external Loader parameters to launch the process of programming an external flash memory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct ExternalLoader
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <summary>
        /// FlashLoader file path.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string FilePath;

        /// <summary>
        /// Device Name and Description.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string DeviceName;

        /// <summary>
        /// Device Type: ONCHIP, EXT8BIT, EXT16BIT, ...
        /// </summary>
        public int DeviceType;

        /// <summary>
        /// Default Device Start Address.
        /// </summary>
        public uint DeviceStartAddress;

        /// <summary>
        /// Total Size of Device.
        /// </summary>
        public uint DeviceSize;

        /// <summary>
        /// Programming Page Size.
        /// </summary>
        public uint PageSize;

        /// <summary>
        /// Content of Erased Memory.
        /// <summary>
        //  unsigned char  	 EraseValue;

        /// <summary>
        /// Type number.
        /// </summary>
        public uint SectorsTypeNumber;

        /// <summary>
        /// Device sectors.
        /// </summary>
        public IntPtr Sectors;
    }
}
