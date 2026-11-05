// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System;
    using System.Collections.Generic;

    public struct DeviceFileDataC : IEquatable<DeviceFileDataC>
    {
        public int Type { get; set; }
        public int SegmentsNumber { get; set; }
        public IList<DeviceSegmentDataC> Segments { get; private set; }

        public DeviceFileDataC(int type, int segmentsNumber, IList<DeviceSegmentDataC> segments)
        {
            this.Type = type;
            this.SegmentsNumber = segmentsNumber;
            this.Segments = segments;
        }

        public void SetSegments(IList<DeviceSegmentDataC> segments)
        {
            this.Segments = segments;
        }

        public override bool Equals(object obj)
        {
            return obj is DeviceFileDataC other && this.Equals(other);
        }

        public bool Equals(DeviceFileDataC other)
        {
            return this.Type == other.Type && this.SegmentsNumber == other.SegmentsNumber && EqualityComparer<IList<DeviceSegmentDataC>>.Default.Equals(this.Segments, other.Segments);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Type, this.SegmentsNumber, this.Segments);
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
