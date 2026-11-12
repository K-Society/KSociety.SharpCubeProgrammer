// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\BitCoefficientC.xml' path='docs/members[@name="bitCoefficientC"]/BitCoefficientC/*'/>
    [StructLayout(LayoutKind.Sequential)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct BitCoefficientC
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\BitCoefficientC.xml' path='docs/members[@name="bitCoefficientC"]/Multiplier/*'/>
        public uint Multiplier;

        /// <include file='..\Doc\BitCoefficientC.xml' path='docs/members[@name="bitCoefficientC"]/Offset/*'/>
        public uint Offset;
    }
}
