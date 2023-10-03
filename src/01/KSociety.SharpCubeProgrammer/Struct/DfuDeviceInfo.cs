// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace KSociety.SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class DfuDeviceInfo
    {
        /// <summary>
        /// USB index.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string UsbIndex;

        /// <summary>
        /// Bus number.
        /// </summary>
        public int BusNumber;

        /// <summary>
        /// Address number.
        /// </summary>
        public int AddressNumber;

        /// <summary>
        /// Product number.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string ProductId;

        /// <summary>
        /// Serial number.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string SerialNumber;

        /// <summary>
        /// DFU version.
        /// </summary>
        public uint DfuVersion;
    }
}
