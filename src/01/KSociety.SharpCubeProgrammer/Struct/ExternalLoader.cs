// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Get external Loader parameters to launch the process of programming an external flash memory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ExternalLoader
    {
        /// <summary>
        /// FlashLoader file path.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string filePath;

        /// <summary>
        /// Device Name and Description.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string deviceName;

        /// <summary>
        /// Device Type: ONCHIP, EXT8BIT, EXT16BIT, ...
        /// </summary>
        public int deviceType;

        /// <summary>
        /// Default Device Start Address.
        /// </summary>
        public uint deviceStartAddress;

        /// <summary>
        /// Total Size of Device.
        /// </summary>
        public uint deviceSize;

        /// <summary>
        /// Programming Page Size.
        /// </summary>
        public uint pageSize;

        /// <summary>
        /// Content of Erased Memory.
        /// <summary>
        //  unsigned char  	 EraseValue;

        /// <summary>
        /// Type number.
        /// </summary>
        public uint sectorsTypeNbr;

        /// <summary>
        /// Device sectors.
        /// </summary>
        public IntPtr sectors;
    }
}
