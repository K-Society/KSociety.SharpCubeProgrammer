// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct StorageStructure
    {
        public uint BanksNumber;
        public IntPtr Banks;
    }
}
