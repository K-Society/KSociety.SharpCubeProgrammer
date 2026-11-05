// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Get external Loader parameters to launch the process of programming an external flash memory.
    /// </summary>
    public struct DeviceExternalLoader : IEquatable<DeviceExternalLoader>
    {
        /// <summary>
        /// FlashLoader file path.
        /// </summary>
        public string filePath { get; set; }

        /// <summary>
        /// Device Name and Description.
        /// </summary>
        public string deviceName { get; set; }

        /// <summary>
        /// Device Type: ONCHIP, EXT8BIT, EXT16BIT, ...
        /// </summary>
        public int deviceType { get; set; }

        /// <summary>
        /// Default Device Start Address.
        /// </summary>
        public uint deviceStartAddress { get; set; }

        /// <summary>
        /// Total Size of Device.
        /// </summary>
        public uint deviceSize { get; set; }

        /// <summary>
        /// Programming Page Size.
        /// </summary>
        public uint pageSize { get; set; }

        /// <summary>
        /// Content of Erased Memory.
        /// <summary>
        //  unsigned char EraseValue;

        /// <summary>
        /// Type number.
        /// </summary>
        public uint sectorsTypeNbr { get; set; }

        /// <summary>
        /// Device sectors.
        /// </summary>
        public IList<DeviceSector> sectors { get; private set; }

        public DeviceExternalLoader(string filePath, string deviceName, int deviceType, uint deviceStartAddress, uint deviceSize, uint pageSize, uint sectorsTypeNbr, IList<DeviceSector> sectors)
        {
            this.filePath = filePath;
            this.deviceName = deviceName;
            this.deviceType = deviceType;
            this.deviceStartAddress = deviceStartAddress;
            this.deviceSize = deviceSize;
            this.pageSize = pageSize;
            this.sectorsTypeNbr = sectorsTypeNbr;
            this.sectors = sectors;
        }

        public void SetSectors(IList<DeviceSector> sectors)
        {
            this.sectors = sectors;
        }

        public override bool Equals(object obj)
        {
            return obj is DeviceExternalLoader other && this.Equals(other);
        }

        public bool Equals(DeviceExternalLoader other)
        {
            return this.filePath == other.filePath &&
                   this.deviceName == other.deviceName &&
                   this.deviceType == other.deviceType &&
                   this.deviceStartAddress == other.deviceStartAddress &&
                   this.deviceSize == other.deviceSize &&
                   this.pageSize == other.pageSize &&
                   this.sectorsTypeNbr == other.sectorsTypeNbr &&
                   EqualityComparer<IList<DeviceSector>>.Default.Equals(this.sectors, other.sectors);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.filePath, this.deviceName, this.deviceType, this.deviceStartAddress, this.deviceSize, this.pageSize, this.sectorsTypeNbr, this.sectors);
        }

        public static bool operator ==(DeviceExternalLoader left, DeviceExternalLoader right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DeviceExternalLoader left, DeviceExternalLoader right)
        {
            return !(left == right);
        }
    }
}
