// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct DfuConnectParameters
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <summary>
        /// Indicates the index of DFU ports already connected.
        /// </summary>
        public string UsbIndex;

        /// <summary>
        /// Request a read unprotect: value in {0,1}.
        /// </summary>
        public byte RequestReadUnprotect;

        /// <summary>
        /// Request a TZEN regression: value in {0,1}.
        /// </summary>
        public byte Tzenreg;

        /// <summary>
        /// USB Timeout control.
        /// </summary>
        public int UsbTimeout;
    }
}
