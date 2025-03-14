// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;
    using Enum;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct UsartConnectParameters
    {
        /// <summary>
        /// Interface identifier: COM1, COM2, /dev/ttyS0...
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string portName;

        /// <summary>
        /// Speed transmission: 115200, 9600...
        /// </summary>
        public uint baudrate;

        /// <summary>
        /// Parity bit: value in usartParity.
        /// </summary>
        public UsartParity parity;

        /// <summary>
        /// Data bit: value in {6, 7, 8}.
        /// </summary>
        public byte dataBits;

        /// <summary>
        /// Stop bit: value in {1, 1.5, 2}.
        /// </summary>
        public float stopBits;

        /// <summary>
        /// Flow control: value in usartFlowControl.
        /// </summary>
        public UsartFlowControl flowControl;

        /// <summary>
        /// RTS: Value in {0,1}.
        /// </summary>
        public int statusRTS;

        /// <summary>
        /// DTR: Value in {0,1}.
        /// </summary>
        public int statusDTR;

        /// <summary>
        /// Manage the delay timing of the Pulse.
        /// </summary>
        public int DelayRtsDtr;

        /// <summary>
        /// Set No Init bits: value in {0,1}.
        /// </summary>
        public byte noinitBits;

        /// <summary>
        /// Request a read unprotect: value in {0,1}.
        /// </summary>
        public byte rdu;

        /// <summary>
        /// Request a TZEN regression: value in {0,1}.
        /// </summary>
        public byte tzenreg;
    }
}
