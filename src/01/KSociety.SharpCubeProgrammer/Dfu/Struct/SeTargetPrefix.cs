// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Dfu.Struct
{
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SeTargetPrefix
    {
        /// <summary>
        /// Total size of this structure in bytes.
        /// </summary>
        public static readonly int Size = Marshal.SizeOf(typeof(SeTargetPrefix));

        /// <summary>
        /// The valid prefix signature.
        /// </summary>
        public static readonly string Signature = "Target";

        /// <summary>
        /// The unique signature field.
        /// </summary>
        public string sSignature { get { return new string(this.ucSignature); } }
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public readonly char[] ucSignature;

        /// <summary>
        /// Specifies the alternate setting of the DFU interface to select to flash the current target
        /// </summary>
        public readonly byte bAlternateSetting;

        /// <summary>
        /// Flag to indicate if the target is named.
        /// </summary>
        public readonly bool bTargetNamed;

        /// <summary>
        /// Descriptive name of the target.
        /// </summary>
        public string sTargetName
        {
            get
            {
                // need to get rid of random memory after \0
                var encoding = Encoding.ASCII;
                return encoding.GetString(encoding.GetBytes(this.ucTargetName.TakeWhile(c => !c.Equals('\0')).ToArray()));
            }
        }
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
        public readonly char[] ucTargetName;

        /// <summary>
        /// Total size of the target data.
        /// </summary>
        public readonly uint dwTargetSize;

        /// <summary>
        /// Number of memory elements (segments).
        /// </summary>
        public readonly uint dwNbElements;
    }
}
