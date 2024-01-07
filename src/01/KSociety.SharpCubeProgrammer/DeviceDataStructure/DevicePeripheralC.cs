// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.DeviceDataStructure
{
    using System.Collections.Generic;

    public struct DevicePeripheralC
    {
        public string Name;

        public string Description;

        public uint BanksNbr;

        public List<DeviceBankC> Banks;
    }
}
