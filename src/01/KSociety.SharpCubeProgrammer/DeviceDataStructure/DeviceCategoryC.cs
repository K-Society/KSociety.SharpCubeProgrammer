// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Collections.Generic;

    public struct DeviceCategoryC : IEquatable<DeviceCategoryC>
    {
        public string Name { get; set; }
        public uint BitsNbr { get; set; }
        public IList<DeviceBitC> Bits { get; private set; }

        public DeviceCategoryC(string name, uint bitsNbr, IList<DeviceBitC> bits)
        {
            this.Name = name;
            this.BitsNbr = bitsNbr;
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
                   this.BitsNbr == other.BitsNbr &&
                   EqualityComparer<IList<DeviceBitC>>.Default.Equals(this.Bits, other.Bits);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Name, this.BitsNbr, this.Bits);
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
