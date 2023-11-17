// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct BankC
    {
        public uint Size;
        public uint Address;
        public byte Access;
        public uint CategoriesNbr;
        public IntPtr Categories;
    }
}
