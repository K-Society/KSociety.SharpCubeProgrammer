// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\CanConnectParameters.xml' path='docs/members[@name="canConnectParameters"]/CanConnectParameters/*'/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct CanConnectParameters
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\CanConnectParameters.xml' path='docs/members[@name="canConnectParameters"]/Baudrate/*'/>
        public int Baudrate;

        /// <include file='..\Doc\CanConnectParameters.xml' path='docs/members[@name="canConnectParameters"]/CanMode/*'/>
        public int CanMode;

        /// <include file='..\Doc\CanConnectParameters.xml' path='docs/members[@name="canConnectParameters"]/CanType/*'/>
        public int CanType;

        /// <include file='..\Doc\CanConnectParameters.xml' path='docs/members[@name="canConnectParameters"]/FrameFormat/*'/>
        public int FrameFormat;

        /// <include file='..\Doc\CanConnectParameters.xml' path='docs/members[@name="canConnectParameters"]/Fifo/*'/>
        public int Fifo;

        /// <include file='..\Doc\CanConnectParameters.xml' path='docs/members[@name="canConnectParameters"]/FilterMode/*'/>
        public int FilterMode;

        /// <include file='..\Doc\CanConnectParameters.xml' path='docs/members[@name="canConnectParameters"]/FilterScale/*'/>
        public int FilterScale;

        /// <include file='..\Doc\CanConnectParameters.xml' path='docs/members[@name="canConnectParameters"]/FilterActivation/*'/>
        public int FilterActivation;

        /// <include file='..\Doc\CanConnectParameters.xml' path='docs/members[@name="canConnectParameters"]/FilterBankNumber/*'/>
        public byte FilterBankNumber;

    }
}
