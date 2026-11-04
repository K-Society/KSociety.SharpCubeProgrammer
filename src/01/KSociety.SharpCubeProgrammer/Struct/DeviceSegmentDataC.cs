// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    public struct DeviceSegmentDataC : IEquatable<DeviceSegmentDataC>
    {
        public int address { get; set; }
        public int size { get; set; }
        public ReadOnlyCollection<byte> data { get; private set; }

        public DeviceSegmentDataC(int address, int size, byte[] data)
        {
            this.address = address;
            this.size = size;
            this.data = new ReadOnlyCollection<byte>(data);
        }

        public void SetData(byte[] data)
        {
            this.data = new ReadOnlyCollection<byte>(data);
        }

        public override bool Equals(object obj)
        {
            return obj is DeviceSegmentDataC other && this.Equals(other);
        }

        public bool Equals(DeviceSegmentDataC other)
        {
            return this.address == other.address && this.size == other.size && Enumerable.SequenceEqual(this.data, other.data);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.address, this.size, this.data);
        }

        public static bool operator ==(DeviceSegmentDataC left, DeviceSegmentDataC right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DeviceSegmentDataC left, DeviceSegmentDataC right)
        {
            return !(left == right);
        }
    }
}
