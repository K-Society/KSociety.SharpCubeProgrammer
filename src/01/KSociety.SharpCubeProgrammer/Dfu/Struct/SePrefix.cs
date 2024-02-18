// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Dfu.Struct
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SePrefix
    {
        /// <summary>
        /// Total size of this structure in bytes.
        /// </summary>
        public static readonly int Size = Marshal.SizeOf(typeof(SePrefix));

        /// <summary>
        /// The valid prefix signature.
        /// </summary>
        public static readonly string Signature = "DfuSe";

        /// <summary>
        /// The unique signature field.
        /// </summary>
        public string sSignature { get { return new string(this.ucSignature); } }
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public readonly char[] ucSignature;

        /// <summary>
        /// DfuSe file type version (== 1).
        /// </summary>
        public readonly byte bVersion;

        /// <summary>
        /// Size of the firmware image
        /// </summary>
        public readonly uint dwImageSize;

        /// <summary>
        /// The number of targets defined in the file
        /// </summary>
        public readonly byte bTargets;
    }
}
