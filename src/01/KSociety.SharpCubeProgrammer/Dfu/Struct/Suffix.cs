// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Dfu.Struct
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// The purpose of the DFU suffix is to allow the operating system in general,
    /// and the DFU operator interface application in particular, to have a-priori knowledge of
    /// whether a firmware download is likely to complete correctly. In other words, these bytes
    /// allow the host software to detect and prevent attempts to download incompatible firmware.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct Suffix
    {
        /// <summary>
        /// Total size of this structure in bytes.
        /// </summary>
        public static readonly int Size = Marshal.SizeOf(typeof(Suffix));

        /// <summary>
        /// The valid suffix signature.
        /// </summary>
        public static readonly string Signature = "UFD";

        /// <summary>
        /// The release number of the device associated with this file.
        /// Either FFFFh or a BCD firmware release or version number.
        /// </summary>
        public readonly ushort bcdDevice;

        /// <summary>
        /// The product ID associated with this file. Either FFFFh or must match device’s product ID.
        /// </summary>
        public readonly ushort idProduct;

        /// <summary>
        /// The vendor ID associated with this file. Either FFFFh or must match device’s vendor ID.
        /// </summary>
        public readonly ushort idVendor;

        /// <summary>
        /// DFU specification number.
        /// </summary>
        public readonly ushort bcdDFU;

        /// <summary>
        /// The unique DFU signature field.
        /// </summary>
        public string sDfuSignature { get { return new string(this.ucDfuSignature); } }
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public readonly char[] ucDfuSignature;

        /// <summary>
        /// The length of this DFU suffix including dwCRC.
        /// </summary>
        public readonly byte bLength;

        /// <summary>
        /// The CRC of the entire file, excluding dwCRC.
        /// </summary>
        public readonly uint dwCRC;
    }
}
