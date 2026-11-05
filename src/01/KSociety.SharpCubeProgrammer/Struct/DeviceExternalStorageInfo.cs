// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System;
    using System.Collections.Generic;

    public struct DeviceExternalStorageInfo : IEquatable<DeviceExternalStorageInfo>
    {
        public uint ExternalLoaderNbr { get; set; }

        public IList<DeviceExternalLoader> ExternalLoader { get; private set; }

        public DeviceExternalStorageInfo(uint externalLoaderNbr, IList<DeviceExternalLoader> externalLoader)
        {
            this.ExternalLoaderNbr = externalLoaderNbr;
            this.ExternalLoader = externalLoader;
        }

        public void SetExternalLoader(IList<DeviceExternalLoader> externalLoader)
        {
            this.ExternalLoader = externalLoader;
        }

        public override bool Equals(object obj)
        {
            return obj is DeviceExternalStorageInfo other && this.Equals(other);
        }

        public bool Equals(DeviceExternalStorageInfo other)
        {
            return this.ExternalLoaderNbr == other.ExternalLoaderNbr &&
                   EqualityComparer<IList<DeviceExternalLoader>>.Default.Equals(this.ExternalLoader, other.ExternalLoader);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.ExternalLoaderNbr, this.ExternalLoader);
        }

        public static bool operator ==(DeviceExternalStorageInfo left, DeviceExternalStorageInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DeviceExternalStorageInfo left, DeviceExternalStorageInfo right)
        {
            return !(left == right);
        }
    }
}
