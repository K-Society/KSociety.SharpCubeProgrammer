// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Dfu.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// This class represents a raw memory image with data segments at absolute addresses.
    /// </summary>
    public class RawMemory : IEquatable<RawMemory>
    {
        public List<Segment> Segments { get; protected set; }

        public RawMemory()
        {
            this.Segments = new List<Segment>();
        }

        public bool TryAddSegment(Segment newSegment)
        {
            if (this.Segments.Any((s) => s.Overlaps(newSegment)))
            {
                return false;
            }
            else if (this.Segments.Any((s) => s.TryAppend(newSegment) || newSegment.TryAppend(s)))
            {
                return true;
            }
            else
            {
                this.Segments.Add(newSegment);
                this.Segments.Sort();
                return true;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is RawMemory memory)
            {
                return this.Equals(memory);
            }
            
            return base.Equals(obj);
        }
        public bool Equals(RawMemory other)
        {
            return ReferenceEquals(this, other) || this.Segments.SequenceEqual(other.Segments);
        }
        public static bool operator ==(RawMemory a, RawMemory b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(RawMemory a, RawMemory b)
        {
            return !(a == b);
        }
    }
}
