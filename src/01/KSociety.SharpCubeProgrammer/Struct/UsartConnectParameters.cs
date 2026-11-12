// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;
    using Enumeration;

    /// <include file='..\Doc\UsartConnectParameters.xml' path='docs/members[@name="usartConnectParameters"]/UsartConnectParameters/*'/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types 
    public struct UsartConnectParameters
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\UsartConnectParameters.xml' path='docs/members[@name="usartConnectParameters"]/PortName/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string PortName;

        /// <include file='..\Doc\UsartConnectParameters.xml' path='docs/members[@name="usartConnectParameters"]/Baudrate/*'/>
        public uint Baudrate;

        /// <include file='..\Doc\UsartConnectParameters.xml' path='docs/members[@name="usartConnectParameters"]/Parity/*'/>
        public UsartParity Parity;

        /// <include file='..\Doc\UsartConnectParameters.xml' path='docs/members[@name="usartConnectParameters"]/DataBits/*'/>
        public byte DataBits;

        /// <include file='..\Doc\UsartConnectParameters.xml' path='docs/members[@name="usartConnectParameters"]/StopBits/*'/>
        public float StopBits;

        /// <include file='..\Doc\UsartConnectParameters.xml' path='docs/members[@name="usartConnectParameters"]/FlowControl/*'/>
        public UsartFlowControl FlowControl;

        /// <include file='..\Doc\UsartConnectParameters.xml' path='docs/members[@name="usartConnectParameters"]/StatusRTS/*'/>
        public int StatusRTS;

        /// <include file='..\Doc\UsartConnectParameters.xml' path='docs/members[@name="usartConnectParameters"]/StatusDTR/*'/>
        public int StatusDTR;

        /// <include file='..\Doc\UsartConnectParameters.xml' path='docs/members[@name="usartConnectParameters"]/DelayRtsDtr/*'/>
        public int DelayRtsDtr;

        /// <include file='..\Doc\UsartConnectParameters.xml' path='docs/members[@name="usartConnectParameters"]/NoInitBits/*'/>
        public byte NoInitBits;

        /// <include file='..\Doc\UsartConnectParameters.xml' path='docs/members[@name="usartConnectParameters"]/RequestReadUnprotect/*'/>
        public byte RequestReadUnprotect;

        /// <include file='..\Doc\UsartConnectParameters.xml' path='docs/members[@name="usartConnectParameters"]/Tzenreg/*'/>
        public byte Tzenreg;
    }
}
