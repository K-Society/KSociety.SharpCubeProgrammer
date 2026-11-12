// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\BankC.xml' path='docs/members[@name="bankC"]/BankC/*'/>
    [StructLayout(LayoutKind.Sequential)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct BankC
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\BankC.xml' path='docs/members[@name="bankC"]/Size/*'/>
        public uint Size;

        /// <include file='..\Doc\BankC.xml' path='docs/members[@name="bankC"]/Address/*'/>
        public uint Address;

        /// <include file='..\Doc\BankC.xml' path='docs/members[@name="bankC"]/Access/*'/>
        public byte Access;

        /// <include file='..\Doc\BankC.xml' path='docs/members[@name="bankC"]/CategoriesNumber/*'/>
        public uint CategoriesNumber;

        /// <include file='..\Doc\BankC.xml' path='docs/members[@name="bankC"]/Categories/*'/>
        public IntPtr Categories;
    }
}
