// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct CanConnectParameters
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <summary>
        /// Baudrate and speed transmission 125KHz, 250KHz, 500KHz...
        /// </summary>
        public int Baudrate;

        /// <summary>
        /// CAN mode: NORMAL, LOOPBACK...
        /// </summary>
        public int CanMode;

        /// <summary>
        /// CAN type: STANDARD or EXTENDED.
        /// </summary>
        public int CanType;

        /// <summary>
        /// Frame format: DATA or REMOTE.
        /// </summary>
        public int FrameFormat;

        /// <summary>
        /// Memory of received messages: FIFO0 or FIFO1.
        /// </summary>
        public int Fifo;

        /// <summary>
        /// Filter mode: MASK or LIST.
        /// </summary>
        public int FilterMode;

        /// <summary>
        /// Filter scale: 16 or 32.
        /// </summary>
        public int FilterScale;

        /// <summary>
        /// Filter activation: DISABLE or ENABLE.
        /// </summary>
        public int FilterActivation;

        /// <summary>
        /// Filter bank number: 0 to 13.
        /// </summary>
        public byte FilterBankNumber;

    }
}
