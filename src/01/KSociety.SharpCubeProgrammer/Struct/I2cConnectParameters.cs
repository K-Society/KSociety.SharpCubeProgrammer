// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\I2cConnectParameters.xml' path='docs/members[@name="i2cConnectParameters"]/I2cConnectParameters/*'/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct I2cConnectParameters
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\I2cConnectParameters.xml' path='docs/members[@name="i2cConnectParameters"]/Address/*'/>
        public int Address;

        /// <include file='..\Doc\I2cConnectParameters.xml' path='docs/members[@name="i2cConnectParameters"]/Baudrate/*'/>
        public int Baudrate;

        /// <include file='..\Doc\I2cConnectParameters.xml' path='docs/members[@name="i2cConnectParameters"]/SpeedMode/*'/>
        public int SpeedMode;

        /// <include file='..\Doc\I2cConnectParameters.xml' path='docs/members[@name="i2cConnectParameters"]/AddressMode/*'/>
        public int AddressMode;

        /// <include file='..\Doc\I2cConnectParameters.xml' path='docs/members[@name="i2cConnectParameters"]/AnalogFilter/*'/>
        public int AnalogFilter;

        /// <include file='..\Doc\I2cConnectParameters.xml' path='docs/members[@name="i2cConnectParameters"]/DigitalFilter/*'/>
        public int DigitalFilter;

        /// <include file='..\Doc\I2cConnectParameters.xml' path='docs/members[@name="i2cConnectParameters"]/DigitalNoiseFilter/*'/>
        public byte DigitalNoiseFilter;

        /// <include file='..\Doc\I2cConnectParameters.xml' path='docs/members[@name="i2cConnectParameters"]/RiseTime/*'/>
        public int RiseTime;

        /// <include file='..\Doc\I2cConnectParameters.xml' path='docs/members[@name="i2cConnectParameters"]/FallTime/*'/>
        public int FallTime;

    }
}
