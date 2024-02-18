// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Dfu.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// This class represents a contiguous memory content starting at an absolute address.
    /// </summary>
    public class Segment : IEquatable<Segment>, IComparable<Segment>
    {
        public ulong StartAddress { get; protected set; }
        public ulong EndAddress { get { return this.StartAddress + (ulong)this.Data.LongLength - 1; } }
        private byte[] data;

        public byte[] Data
        {
            get { return this.data; }
        }

        public Segment(ulong startAddress, byte[] data)
        {
            this.StartAddress = startAddress;
            this.data = data;
        }

        public int Length
        {
            get { return this.Data.Length; }
        }

        public bool Equals(Segment other)
        {
            return this.StartAddress == other.StartAddress && this.Data.SequenceEqual(other.Data);
        }
        public override bool Equals(object obj)
        {
            if (obj is Segment segment)
            {
                return this.Equals(segment);
            }
                
            return base.Equals(obj);
        }
        public static bool operator ==(Segment a, Segment b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Segment a, Segment b)
        {
            return !(a == b);
        }

        public bool Overlaps(Segment other)
        {
            if (this.StartAddress < other.StartAddress)
            {
                return other.StartAddress <= this.EndAddress;
            }
            else
            {
                return this.StartAddress <= other.EndAddress;
            }
        }

        public bool Extends(Segment other)
        {
            return this.StartAddress == (other.EndAddress + 1);
        }

        public bool TryAppend(Segment other)
        {
            if (!other.Extends(this))
            {
                return false;
            }
            else
            {
                int dlen = this.data.Length;
                Array.Resize<byte>(ref this.data, this.data.Length + other.Data.Length);
                Array.Copy(other.Data, 0, this.data, dlen, other.Data.Length);
                return true;
            }
        }

        public bool ContainsKey(ulong key)
        {
            return (key >= this.StartAddress) && (key <= this.EndAddress);
        }

        public bool TryGetValue(ulong key, out byte value)
        {
            if (!this.ContainsKey(key))
            {
                value = 0;
                return false;
            }
            else
            {
                value = this[key];
                return true;
            }
        }

        public ICollection<byte> Values
        {
            get { return this.Data; }
        }

        public byte this[ulong key]
        {
            get
            {
                return this.Data[key - this.StartAddress];
            }
            set
            {
                this.Data[key - this.StartAddress] = value;
            }
        }

        public bool Contains(KeyValuePair<ulong, byte> item)
        {
            return this.TryGetValue(item.Key, out var v) && (v == item.Value);
        }

        public int Count
        {
            get { return this.Values.Count; }
        }

        public int CompareTo(Segment other)
        {
            return this.StartAddress.CompareTo(other.StartAddress);
        }
    }
}
