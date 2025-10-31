// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct CanConnectParameters
    {
        /// <summary>
        /// Baudrate and speed transmission 125KHz, 250KHz, 500KHz...
        /// </summary>
        public int br;

        /// <summary>
        /// CAN mode: NORMAL, LOOPBACK...
        /// </summary>
        public int mode;

        /// <summary>
        /// CAN type: STANDARD or EXTENDED.
        /// </summary>
        public int ide;

        /// <summary>
        /// Frame format: DATA or REMOTE.
        /// </summary>
        public int rtr;

        /// <summary>
        /// Memory of received messages: FIFO0 or FIFO1.
        /// </summary>
        public int fifo;

        /// <summary>
        /// Filter mode: MASK or LIST.
        /// </summary>
        public int fm;

        /// <summary>
        /// Filter scale: 16 or 32.
        /// </summary>
        public int fs;

        /// <summary>
        /// Filter activation: DISABLE or ENABLE.
        /// </summary>
        public int fe;

        /// <summary>
        /// Filter bank number: 0 to 13.
        /// </summary>
        public byte fbn;

    }
}
