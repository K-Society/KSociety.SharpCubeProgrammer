// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System.Collections.Generic;

    public struct DeviceStorageStructure
    {
        public uint BanksNumber;
        public List<DeviceDeviceBank> Banks;
    }
}
