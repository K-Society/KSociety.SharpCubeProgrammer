// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct I2cConnectParameters
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <summary>
        /// Device address in hex format.
        /// </summary>
        public int Address;

        /// <summary>
        /// Baudrate and speed transmission : 100 or 400 KHz.
        /// </summary>
        public int Baudrate;

        /// <summary>
        /// Speed Mode: STANDARD or FAST.
        /// </summary>
        public int SpeedMode;

        /// <summary>
        /// Address Mode: 7 or 10 bits.
        /// </summary>
        public int AddressMode;

        /// <summary>
        /// Analog filter: DISABLE or ENABLE.
        /// </summary>
        public int AnalogFilter;

        /// <summary>
        /// Digital filter: DISABLE or ENABLE.
        /// </summary>
        public int DigitalFilter;

        /// <summary>
        /// Digital noise filter: 0 to 15.
        /// </summary>
        public byte DigitalNoiseFilter;

        /// <summary>
        /// Rise time: 0-1000 for STANDARD speed mode and  0-300 for FAST.
        /// </summary>
        public int RiseTime;

        /// <summary>
        /// Fall time: 0-300 for STANDARD speed mode and  0-300 for FAST.
        /// </summary>
        public int FallTime;

    }
}
