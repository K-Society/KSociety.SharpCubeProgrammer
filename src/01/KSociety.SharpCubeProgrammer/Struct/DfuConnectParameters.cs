// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\DfuConnectParameters.xml' path='docs/members[@name="dfuConnectParameters"]/DfuConnectParameters/*'/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct DfuConnectParameters
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\DfuConnectParameters.xml' path='docs/members[@name="dfuConnectParameters"]/UsbIndex/*'/>
        public string UsbIndex;

        /// <include file='..\Doc\DfuConnectParameters.xml' path='docs/members[@name="dfuConnectParameters"]/RequestReadUnprotect/*'/>
        public byte RequestReadUnprotect;

        /// <include file='..\Doc\DfuConnectParameters.xml' path='docs/members[@name="dfuConnectParameters"]/Tzenreg/*'/>
        public byte Tzenreg;

        /// <include file='..\Doc\DfuConnectParameters.xml' path='docs/members[@name="dfuConnectParameters"]/UsbTimeout/*'/>
        public int UsbTimeout;
    }
}
