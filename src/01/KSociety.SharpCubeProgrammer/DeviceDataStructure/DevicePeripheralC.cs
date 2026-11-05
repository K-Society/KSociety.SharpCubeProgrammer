// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Collections.Generic;

    public struct DevicePeripheralC : IEquatable<DevicePeripheralC>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public uint BanksNbr { get; set; }

        public IList<DeviceBankC> Banks { get; private set; }

        public DevicePeripheralC(string name, string description, uint banksNbr, IList<DeviceBankC> banks)
        {
            this.Name = name;
            this.Description = description;
            this.BanksNbr = banksNbr;
            this.Banks = banks;
        }

        public void SetBanks(IList<DeviceBankC> banks)
        {
            this.Banks = banks;
        }

        public override bool Equals(object obj)
        {
            return obj is DevicePeripheralC other && this.Equals(other);
        }

        public bool Equals(DevicePeripheralC other)
        {
            return this.Name == other.Name &&
                   this.Description == other.Description &&
                   this.BanksNbr == other.BanksNbr &&
                   EqualityComparer<IList<DeviceBankC>>.Default.Equals(this.Banks, other.Banks);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Name, this.Description, this.BanksNbr, this.Banks);    
        }

        public static bool operator ==(DevicePeripheralC left, DevicePeripheralC right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DevicePeripheralC left, DevicePeripheralC right)
        {
            return !(left == right);
        }
    }
}
