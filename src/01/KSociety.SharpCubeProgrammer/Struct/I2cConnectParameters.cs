// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct I2CConnectParameters
    {
        /// <summary>
        /// Device address in hex format.
        /// </summary>
        public int add;

        /// <summary>
        /// Baudrate and speed transmission : 100 or 400 KHz.
        /// </summary>
        public int br;

        /// <summary>
        /// Speed Mode: STANDARD or FAST.
        /// </summary>
        public int sm;

        /// <summary>
        /// Address Mode: 7 or 10 bits.
        /// </summary>
        public int am;

        /// <summary>
        /// Analog filter: DISABLE or ENABLE.
        /// </summary>
        public int af;

        /// <summary>
        /// Digital filter: DISABLE or ENABLE.
        /// </summary>
        public int df;

        /// <summary>
        /// Digital noise filter: 0 to 15.
        /// </summary>
        public string dnf;

        /// <summary>
        /// Rise time: 0-1000 for STANDARD speed mode and  0-300 for FAST.
        /// </summary>
        public int rt;

        /// <summary>
        /// Fall time: 0-300 for STANDARD speed mode and  0-300 for FAST.
        /// </summary>
        public int ft;

    }
}
