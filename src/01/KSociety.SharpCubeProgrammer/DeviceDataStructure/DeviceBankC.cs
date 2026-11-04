// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System;
    using System.Collections.Generic;

    public struct DeviceBankC : IEquatable<DeviceBankC>
    {
        public uint Size { get; set; }
        public uint Address { get; set; }
        public byte Access { get; set; }
        public uint CategoriesNbr { get; set; }
        public IList<DeviceCategoryC> Categories { get; private set; }

        public DeviceBankC(uint size, uint address, byte access, uint categoriesNbr, IList<DeviceCategoryC> categories)
        {
            this.Size = size;
            this.Address = address;
            this.Access = access;
            this.CategoriesNbr = categoriesNbr;
            this.Categories = categories;
        }

        public void SetCategories(IList<DeviceCategoryC> categories)
        {
            this.Categories = categories;
        }

        public override bool Equals(object obj)
        {
            return obj is DeviceBankC other && this.Equals(other);
        }

        public bool Equals(DeviceBankC other)
        {
            return this.Size == other.Size &&
                   this.Address == other.Address &&
                   this.Access == other.Access &&
                   this.CategoriesNbr == other.CategoriesNbr &&
                   EqualityComparer<IList<DeviceCategoryC>>.Default.Equals(this.Categories, other.Categories);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Size, this.Address, this.Access, this.CategoriesNbr, this.Categories);
        }

        public static bool operator ==(DeviceBankC left, DeviceBankC right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DeviceBankC left, DeviceBankC right)
        {
            return !(left == right);
        }
    }
}
