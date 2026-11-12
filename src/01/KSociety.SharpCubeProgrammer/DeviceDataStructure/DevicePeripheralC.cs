// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Collections.Generic;

    /// <include file='..\Doc\DevicePeripheralC.xml' path='docs/members[@name="devicePeripheralC"]/DevicePeripheralC/*'/>
    public struct DevicePeripheralC : IEquatable<DevicePeripheralC>
    {
        /// <include file='..\Doc\DevicePeripheralC.xml' path='docs/members[@name="devicePeripheralC"]/Name/*'/>
        public string Name { get; set; }

        /// <include file='..\Doc\DevicePeripheralC.xml' path='docs/members[@name="devicePeripheralC"]/Description/*'/>
        public string Description { get; set; }

        /// <include file='..\Doc\DevicePeripheralC.xml' path='docs/members[@name="devicePeripheralC"]/BanksNumber/*'/>
        public uint BanksNumber { get; set; }

        /// <include file='..\Doc\DevicePeripheralC.xml' path='docs/members[@name="devicePeripheralC"]/Banks/*'/>
        public IList<DeviceBankC> Banks { get; private set; }

        public DevicePeripheralC(string name, string description, uint banksNumber, IList<DeviceBankC> banks)
        {
            this.Name = name;
            this.Description = description;
            this.BanksNumber = banksNumber;
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
                   this.BanksNumber == other.BanksNumber &&
                   EqualityComparer<IList<DeviceBankC>>.Default.Equals(this.Banks, other.Banks);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Name, this.Description, this.BanksNumber, this.Banks);    
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
