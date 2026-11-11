// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System;
    using System.Collections.Generic;

    /// <include file='..\Doc\DeviceExternalStorageInfo.xml' path='docs/members[@name="deviceExternalStorageInfo"]/DeviceExternalStorageInfo/*'/>
    public struct DeviceExternalStorageInfo : IEquatable<DeviceExternalStorageInfo>
    {
        /// <include file='..\Doc\DeviceExternalStorageInfo.xml' path='docs/members[@name="deviceExternalStorageInfo"]/ExternalLoaderNumber/*'/>
        public uint ExternalLoaderNumber { get; set; }

        /// <include file='..\Doc\DeviceExternalStorageInfo.xml' path='docs/members[@name="deviceExternalStorageInfo"]/ExternalLoader/*'/>
        public IList<DeviceExternalLoader> ExternalLoader { get; private set; }

        public DeviceExternalStorageInfo(uint externalLoaderNumber, IList<DeviceExternalLoader> externalLoader)
        {
            this.ExternalLoaderNumber = externalLoaderNumber;
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
            return this.ExternalLoaderNumber == other.ExternalLoaderNumber &&
                   EqualityComparer<IList<DeviceExternalLoader>>.Default.Equals(this.ExternalLoader, other.ExternalLoader);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.ExternalLoaderNumber, this.ExternalLoader);
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
