// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Collections.Generic;

    /// <include file='..\Doc\DeviceStorageStructure.xml' path='docs/members[@name="devieStorageStructure"]/DeviceStorageStructure/*'/>
    public struct DeviceStorageStructure : IEquatable<DeviceStorageStructure>
    {
        /// <include file='..\Doc\DeviceStorageStructure.xml' path='docs/members[@name="devieStorageStructure"]/BanksNumber/*'/>
        public uint BanksNumber { get; set; }

        /// <include file='..\Doc\DeviceStorageStructure.xml' path='docs/members[@name="devieStorageStructure"]/Banks/*'/>
        public IList<DeviceDeviceBank> Banks { get; private set; }

        public DeviceStorageStructure(uint banksNumber, IList<DeviceDeviceBank> banks)
        {
            this.BanksNumber = banksNumber;
            this.Banks = banks;
        }

        public void SetBanks(IList<DeviceDeviceBank> banks)
        {
            this.Banks = banks;
        }

        public override bool Equals(object obj)
        {
            return obj is DeviceStorageStructure other && this.Equals(other);
        }

        public bool Equals(DeviceStorageStructure other)
        {
            return this.BanksNumber == other.BanksNumber &&
                   EqualityComparer<IList<DeviceDeviceBank>>.Default.Equals(this.Banks, other.Banks);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.BanksNumber, this.Banks);
        }

        public static bool operator ==(DeviceStorageStructure left, DeviceStorageStructure right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DeviceStorageStructure left, DeviceStorageStructure right)
        {
            return !(left == right);
        }
    }
}
