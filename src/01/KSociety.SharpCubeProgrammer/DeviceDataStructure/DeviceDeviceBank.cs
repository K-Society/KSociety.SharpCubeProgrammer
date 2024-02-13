// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System.Collections.Generic;

    public struct DeviceDeviceBank
    {
        public uint SectorsNumber;
        public List<BankSector> Sectors;
    }
}
