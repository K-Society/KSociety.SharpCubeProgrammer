// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <include file='..\Doc\DeviceSegmentDataC.xml' path='docs/members[@name="deviceSegmentDataC"]/DeviceSegmentDataC/*'/>
    public struct DeviceSegmentDataC : IEquatable<DeviceSegmentDataC>
    {
        /// <include file='..\Doc\DeviceSegmentDataC.xml' path='docs/members[@name="deviceSegmentDataC"]/Address/*'/>
        public int Address { get; set; }

        /// <include file='..\Doc\DeviceSegmentDataC.xml' path='docs/members[@name="deviceSegmentDataC"]/Size/*'/>
        public int Size { get; set; }

        /// <include file='..\Doc\DeviceSegmentDataC.xml' path='docs/members[@name="deviceSegmentDataC"]/Data/*'/>
        public ReadOnlyCollection<byte> Data { get; private set; }

        public DeviceSegmentDataC(int address, int size, byte[] data)
        {
            this.Address = address;
            this.Size = size;
            this.Data = new ReadOnlyCollection<byte>(data);
        }

        public void SetData(byte[] data)
        {
            this.Data = new ReadOnlyCollection<byte>(data);
        }

        public override bool Equals(object obj)
        {
            return obj is DeviceSegmentDataC other && this.Equals(other);
        }

        public bool Equals(DeviceSegmentDataC other)
        {
            return this.Address == other.Address && this.Size == other.Size && Enumerable.SequenceEqual(this.Data, other.Data);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Address, this.Size, this.Data);
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
