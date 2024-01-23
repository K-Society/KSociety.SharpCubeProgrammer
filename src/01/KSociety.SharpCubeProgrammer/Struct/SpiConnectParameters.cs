// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SpiConnectParameters
    {
        /// <summary>
        /// Speed transmission 187, 375, 750, 1500, 3000, 6000, 12000 KHz.
        /// </summary>
        public uint baudrate;

        /// <summary>
        /// Crc polynomial value.
        /// </summary>
        public ushort crcPol;

        /// <summary>
        /// 2LFullDuplex/2LRxOnly/1LRx/1LTx.
        /// </summary>
        public int direction;

        /// <summary>
        /// 1Edge or 2Edge.
        /// </summary>
        public int cpha;

        /// <summary>
        /// LOW or HIGH.
        /// </summary>
        public int cpol;

        /// <summary>
        /// DISABLE or ENABLE.
        /// </summary>
        public int crc;

        /// <summary>
        /// First bit: LSB or MSB.
        /// </summary>
        public int firstBit;

        /// <summary>
        /// Frame format: Motorola or TI.
        /// </summary>
        public int frameFormat;

        /// <summary>
        /// Size of frame data: 16bit or 8bit.
        /// </summary>
        public int dataSize;

        /// <summary>
        /// Operating mode: Slave or Master.
        /// </summary>
        public int mode;

        /// <summary>
        /// Selection: Soft or Hard.
        /// </summary>
        public int nss;

        /// <summary>
        /// NSS pulse: No Pulse or Pulse.
        /// </summary>
        public int nssPulse;

        /// <summary>
        /// Delay of few microseconds, No Delay or Delay, at least 4us delay is inserted.
        /// </summary>
        public int delay;
    }
}
