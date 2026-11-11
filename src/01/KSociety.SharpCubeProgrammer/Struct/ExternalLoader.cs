// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System;
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\ExternalLoader.xml' path='docs/members[@name="externalLoader"]/ExternalLoader/*'/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct ExternalLoader
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\ExternalLoader.xml' path='docs/members[@name="externalLoader"]/FilePath/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string FilePath;

        /// <include file='..\Doc\ExternalLoader.xml' path='docs/members[@name="externalLoader"]/DeviceName/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string DeviceName;

        /// <include file='..\Doc\ExternalLoader.xml' path='docs/members[@name="externalLoader"]/DeviceType/*'/>
        public int DeviceType;

        /// <include file='..\Doc\ExternalLoader.xml' path='docs/members[@name="externalLoader"]/DeviceStartAddress/*'/>
        public uint DeviceStartAddress;

        /// <include file='..\Doc\ExternalLoader.xml' path='docs/members[@name="externalLoader"]/DeviceSize/*'/>
        public uint DeviceSize;

        /// <include file='..\Doc\ExternalLoader.xml' path='docs/members[@name="externalLoader"]/PageSize/*'/>
        public uint PageSize;

        /// <summary>
        /// Content of Erased Memory.
        /// <summary>
        //  unsigned char  	 EraseValue;

        /// <include file='..\Doc\ExternalLoader.xml' path='docs/members[@name="externalLoader"]/SectorsTypeNumber/*'/>
        public uint SectorsTypeNumber;

        /// <include file='..\Doc\ExternalLoader.xml' path='docs/members[@name="externalLoader"]/Sectors/*'/>
        public IntPtr Sectors;
    }
}
