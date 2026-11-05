// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct SpiConnectParameters
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <summary>
        /// Speed transmission 187, 375, 750, 1500, 3000, 6000, 12000 KHz.
        /// </summary>
        public uint Baudrate;

        /// <summary>
        /// Crc polynomial value.
        /// </summary>
        public ushort CrcPolynomial;

        /// <summary>
        /// 2LFullDuplex/2LRxOnly/1LRx/1LTx.
        /// </summary>
        public int Direction;

        /// <summary>
        /// 1Edge or 2Edge.
        /// </summary>
        public int Cpha;

        /// <summary>
        /// LOW or HIGH.
        /// </summary>
        public int Cpol;

        /// <summary>
        /// DISABLE or ENABLE.
        /// </summary>
        public int Crc;

        /// <summary>
        /// First bit: LSB or MSB.
        /// </summary>
        public int FirstBit;

        /// <summary>
        /// Frame format: Motorola or TI.
        /// </summary>
        public int FrameFormat;

        /// <summary>
        /// Size of frame data: 16bit or 8bit.
        /// </summary>
        public int DataSize;

        /// <summary>
        /// Operating mode: Slave or Master.
        /// </summary>
        public int Mode;

        /// <summary>
        /// Selection: Soft or Hard.
        /// </summary>
        public int Nss;

        /// <summary>
        /// NSS pulse: No Pulse or Pulse.
        /// </summary>
        public int NssPulse;

        /// <summary>
        /// Delay of few microseconds, No Delay or Delay, at least 4us delay is inserted.
        /// </summary>
        public int Delay;
    }
}
