// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Collections.Generic;

    /// <include file='..\Doc\DeviceCategoryC.xml' path='docs/members[@name="deviceCategoryC"]/DeviceCategoryC/*'/>
    public struct DeviceCategoryC : IEquatable<DeviceCategoryC>
    {
        /// <include file='..\Doc\DeviceCategoryC.xml' path='docs/members[@name="deviceCategoryC"]/Name/*'/>
        public string Name { get; set; }

        /// <include file='..\Doc\DeviceCategoryC.xml' path='docs/members[@name="deviceCategoryC"]/BitsNumber/*'/>
        public uint BitsNumber { get; set; }

        /// <include file='..\Doc\DeviceCategoryC.xml' path='docs/members[@name="deviceCategoryC"]/Bits/*'/>
        public IList<DeviceBitC> Bits { get; private set; }

        public DeviceCategoryC(string name, uint bitsNumber, IList<DeviceBitC> bits)
        {
            this.Name = name;
            this.BitsNumber = bitsNumber;
            this.Bits = bits;
        }

        public void SetBits(IList<DeviceBitC> bits)
        {
            this.Bits = bits;
        }

        public override bool Equals(object obj)
        {
            return obj is DeviceCategoryC other && this.Equals(other);
        }

        public bool Equals(DeviceCategoryC other)
        {
            return this.Name == other.Name &&
                   this.BitsNumber == other.BitsNumber &&
                   EqualityComparer<IList<DeviceBitC>>.Default.Equals(this.Bits, other.Bits);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Name, this.BitsNumber, this.Bits);
        }

        public static bool operator ==(DeviceCategoryC left, DeviceCategoryC right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DeviceCategoryC left, DeviceCategoryC right)
        {
            return !(left == right);
        }
    }
}
