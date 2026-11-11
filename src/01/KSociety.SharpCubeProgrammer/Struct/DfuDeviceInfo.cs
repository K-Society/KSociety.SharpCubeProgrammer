// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\DfuDeviceInfo.xml' path='docs/members[@name="dfuDeviceInfo"]/DfuDeviceInfo/*'/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct DfuDeviceInfo
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\DfuDeviceInfo.xml' path='docs/members[@name="dfuDeviceInfo"]/UsbIndex/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string UsbIndex;

        /// <include file='..\Doc\DfuDeviceInfo.xml' path='docs/members[@name="dfuDeviceInfo"]/BusNumber/*'/>
        public int BusNumber;

        /// <include file='..\Doc\DfuDeviceInfo.xml' path='docs/members[@name="dfuDeviceInfo"]/AddressNumber/*'/>
        public int AddressNumber;

        /// <include file='..\Doc\DfuDeviceInfo.xml' path='docs/members[@name="dfuDeviceInfo"]/ProductId/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string ProductId;

        /// <include file='..\Doc\DfuDeviceInfo.xml' path='docs/members[@name="dfuDeviceInfo"]/SerialNumber/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string SerialNumber;

        /// <include file='..\Doc\DfuDeviceInfo.xml' path='docs/members[@name="dfuDeviceInfo"]/DfuVersion/*'/>
        public uint DfuVersion;
    }
}
