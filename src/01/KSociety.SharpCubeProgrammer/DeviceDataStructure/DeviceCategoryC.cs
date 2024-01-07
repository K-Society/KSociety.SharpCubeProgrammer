// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.DeviceDataStructure
{
    using System.Collections.Generic;

    public struct DeviceCategoryC
    {
        public string Name;
        public uint BitsNbr;
        public List<DeviceBitC> Bits;
    }
}
