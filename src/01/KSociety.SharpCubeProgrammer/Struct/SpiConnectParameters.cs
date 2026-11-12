// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\SpiConnectParameters.xml' path='docs/members[@name="spiConnectParameters"]/SpiConnectParameters/*'/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct SpiConnectParameters
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\SpiConnectParameters.xml' path='docs/members[@name="spiConnectParameters"]/Baudrate/*'/>
        public uint Baudrate;

        /// <include file='..\Doc\SpiConnectParameters.xml' path='docs/members[@name="spiConnectParameters"]/CrcPolynomial/*'/>
        public ushort CrcPolynomial;

        /// <include file='..\Doc\SpiConnectParameters.xml' path='docs/members[@name="spiConnectParameters"]/Direction/*'/>
        public int Direction;

        /// <include file='..\Doc\SpiConnectParameters.xml' path='docs/members[@name="spiConnectParameters"]/Cpha/*'/>
        public int Cpha;

        /// <include file='..\Doc\SpiConnectParameters.xml' path='docs/members[@name="spiConnectParameters"]/Cpol/*'/>
        public int Cpol;

        /// <include file='..\Doc\SpiConnectParameters.xml' path='docs/members[@name="spiConnectParameters"]/Crc/*'/>
        public int Crc;

        /// <include file='..\Doc\SpiConnectParameters.xml' path='docs/members[@name="spiConnectParameters"]/FirstBit/*'/>
        public int FirstBit;

        /// <include file='..\Doc\SpiConnectParameters.xml' path='docs/members[@name="spiConnectParameters"]/FrameFormat/*'/>
        public int FrameFormat;

        /// <include file='..\Doc\SpiConnectParameters.xml' path='docs/members[@name="spiConnectParameters"]/DataSize/*'/>
        public int DataSize;

        /// <include file='..\Doc\SpiConnectParameters.xml' path='docs/members[@name="spiConnectParameters"]/Mode/*'/>
        public int Mode;

        /// <include file='..\Doc\SpiConnectParameters.xml' path='docs/members[@name="spiConnectParameters"]/Nss/*'/>
        public int Nss;

        /// <include file='..\Doc\SpiConnectParameters.xml' path='docs/members[@name="spiConnectParameters"]/NssPulse/*'/>
        public int NssPulse;

        /// <include file='..\Doc\SpiConnectParameters.xml' path='docs/members[@name="spiConnectParameters"]/Delay/*'/>
        public int Delay;
    }
}
