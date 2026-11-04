// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Collections.Generic;

    public struct DeviceDeviceBank : IEquatable<DeviceDeviceBank>
    {
        public uint SectorsNumber { get; set; }
        public IList<BankSector> Sectors { get; private set; }

        public DeviceDeviceBank(uint sectorsNumber, IList<BankSector> sectors)
        {
            this.SectorsNumber = sectorsNumber;
            this.Sectors = sectors;
        }

        public void SetSectors(IList<BankSector> sectors)
        {
            this.Sectors = sectors;
        }

        public override bool Equals(object obj)
        {
            return obj is DeviceDeviceBank other && this.Equals(other);
        }

        public bool Equals(DeviceDeviceBank other)
        {
            return this.SectorsNumber == other.SectorsNumber &&
                   EqualityComparer<IList<BankSector>>.Default.Equals(this.Sectors, other.Sectors);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.SectorsNumber, this.Sectors);
        }

        public static bool operator ==(DeviceDeviceBank left, DeviceDeviceBank right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DeviceDeviceBank left, DeviceDeviceBank right)
        {
            return !(left == right);
        }
    }
}
