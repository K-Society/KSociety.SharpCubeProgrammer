// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System;
    using System.Collections.Generic;

    public struct DeviceFileDataC : IEquatable<DeviceFileDataC>
    {
        public int Type { get; set; }
        public int segmentsNbr { get; set; }
        public IList<DeviceSegmentDataC> segments { get; private set; }

        public DeviceFileDataC(int type, int segmentsNbr, IList<DeviceSegmentDataC> segments)
        {
            this.Type = type;
            this.segmentsNbr = segmentsNbr;
            this.segments = segments;
        }

        public void SetSegments(IList<DeviceSegmentDataC> segments)
        {
            this.segments = segments;
        }

        public override bool Equals(object obj)
        {
            return obj is DeviceFileDataC other && this.Equals(other);
        }

        public bool Equals(DeviceFileDataC other)
        {
            return this.Type == other.Type && this.segmentsNbr == other.segmentsNbr && EqualityComparer<IList<DeviceSegmentDataC>>.Default.Equals(this.segments, other.segments);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Type, this.segmentsNbr, this.segments);
        }

        public static bool operator ==(DeviceFileDataC left, DeviceFileDataC right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DeviceFileDataC left, DeviceFileDataC right)
        {
            return !(left == right);
        }
    }
}
