// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Collections.Generic;

    /// <include file='..\Doc\DeviceBitC.xml' path='docs/members[@name="deviceBitC"]/DeviceBitC/*'/>
    public struct DeviceBitC : IEquatable<DeviceBitC>
    {
        /// <include file='..\Doc\DeviceBitC.xml' path='docs/members[@name="deviceBitC"]/Name/*'/>
        public string Name { get; set; }

        /// <include file='..\Doc\DeviceBitC.xml' path='docs/members[@name="deviceBitC"]/Description/*'/>
        public string Description { get; set; }

        /// <include file='..\Doc\DeviceBitC.xml' path='docs/members[@name="deviceBitC"]/WordOffset/*'/>
        public uint WordOffset { get; set; }

        /// <include file='..\Doc\DeviceBitC.xml' path='docs/members[@name="deviceBitC"]/BitOffset/*'/>
        public uint BitOffset { get; set; }

        /// <include file='..\Doc\DeviceBitC.xml' path='docs/members[@name="deviceBitC"]/BitWidth/*'/>
        public uint BitWidth { get; set; }

        /// <include file='..\Doc\DeviceBitC.xml' path='docs/members[@name="deviceBitC"]/Access/*'/>
        public byte Access { get; set; }

        /// <include file='..\Doc\DeviceBitC.xml' path='docs/members[@name="deviceBitC"]/ValuesNumber/*'/>
        public uint ValuesNumber { get; set; }

        /// <include file='..\Doc\DeviceBitC.xml' path='docs/members[@name="deviceBitC"]/Values/*'/>
        public IList<BitValueC> Values { get; private set; }

        /// <include file='..\Doc\DeviceBitC.xml' path='docs/members[@name="deviceBitC"]/Equation/*'/>
        public BitCoefficientC Equation { get; set; }

        /// <include file='..\Doc\DeviceBitC.xml' path='docs/members[@name="deviceBitC"]/Reference/*'/>
        public string Reference { get; set; }

        /// <include file='..\Doc\DeviceBitC.xml' path='docs/members[@name="deviceBitC"]/BitValue/*'/>
        public uint BitValue { get; set; }

        /// <include file='..\Doc\DeviceBitC.xml' path='docs/members[@name="deviceBitC"]/ValLine/*'/>
        public uint ValLine { get; set; }

        public DeviceBitC(string name, string description, uint wordOffset, uint bitOffset, uint bitWidth, byte access, uint valuesNumber, IList<BitValueC> values, BitCoefficientC equation, string reference, uint bitValue, uint valLine)
        {
            this.Name = name;
            this.Description = description;
            this.WordOffset = wordOffset;
            this.BitOffset = bitOffset;
            this.BitWidth = bitWidth;
            this.Access = access;
            this.ValuesNumber = valuesNumber;
            this.Values = values;
            this.Equation = equation;
            this.Reference = reference;
            this.BitValue = bitValue;
            this.ValLine = valLine;
        }

        public void SetValues(IList<BitValueC> values)
        {
            this.Values = values;
        }

        public override bool Equals(object obj)
        {
            return obj is DeviceBitC other && this.Equals(other);
        }

        public bool Equals(DeviceBitC other)
        {
            return this.Name == other.Name &&
                   this.Description == other.Description &&
                   this.WordOffset == other.WordOffset &&
                   this.BitOffset == other.BitOffset &&
                   this.BitWidth == other.BitWidth &&
                   this.Access == other.Access &&
                   this.ValuesNumber == other.ValuesNumber &&
                   EqualityComparer<IList<BitValueC>>.Default.Equals(this.Values, other.Values) &&
                   this.Equation.Equals(other.Equation) &&
                   this.Reference == other.Reference &&
                   this.BitValue == other.BitValue &&
                   this.ValLine == other.ValLine;
        }

        public override int GetHashCode()
        { 
            var hash = new HashCode();
            hash.Add(this.Name);
            hash.Add(this.Description);
            hash.Add(this.WordOffset);
            hash.Add(this.BitOffset);
            hash.Add(this.BitWidth);
            hash.Add(this.Access);
            hash.Add(this.ValuesNumber);
            hash.Add(this.Values);
            hash.Add(this.Equation);
            hash.Add(this.Reference);
            hash.Add(this.BitValue);
            hash.Add(this.ValLine);
            return hash.ToHashCode();
        }

        public static bool operator ==(DeviceBitC left, DeviceBitC right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DeviceBitC left, DeviceBitC right)
        {
            return !(left == right);
        }
    }
}
