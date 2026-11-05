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
        public string FilePath { get; set; }

        /// <summary>
        /// Device Name and Description.
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Device Type: ONCHIP, EXT8BIT, EXT16BIT, ...
        /// </summary>
        public int DeviceType { get; set; }

        /// <summary>
        /// Default Device Start Address.
        /// </summary>
        public uint DeviceStartAddress { get; set; }

        /// <summary>
        /// Total Size of Device.
        /// </summary>
        public uint DeviceSize { get; set; }

        /// <summary>
        /// Programming Page Size.
        /// </summary>
        public uint PageSize { get; set; }

        /// <summary>
        /// Content of Erased Memory.
        /// <summary>
        //  unsigned char EraseValue;

        /// <summary>
        /// Type number.
        /// </summary>
        public uint SectorsTypeNumber { get; set; }

        /// <summary>
        /// Device sectors.
        /// </summary>
        public IList<DeviceSector> Sectors  { get; private set; }

        public DeviceExternalLoader(string filePath, string deviceName, int deviceType, uint deviceStartAddress, uint deviceSize, uint pageSize, uint sectorsTypeNumber, IList<DeviceSector> sectors)
        {
            this.FilePath = filePath;
            this.DeviceName = deviceName;
            this.DeviceType = deviceType;
            this.DeviceStartAddress = deviceStartAddress;
            this.DeviceSize = deviceSize;
            this.PageSize = pageSize;
            this.SectorsTypeNumber = sectorsTypeNumber;
            this.Sectors = sectors;
        }

        public void SetSectors(IList<DeviceSector> sectors)
        {
            this.Sectors = sectors;
        }

        public override bool Equals(object obj)
        {
            return obj is DeviceExternalLoader other && this.Equals(other);
        }

        public bool Equals(DeviceExternalLoader other)
        {
            return this.FilePath == other.FilePath &&
                   this.DeviceName == other.DeviceName &&
                   this.DeviceType == other.DeviceType &&
                   this.DeviceStartAddress == other.DeviceStartAddress &&
                   this.DeviceSize == other.DeviceSize &&
                   this.PageSize == other.PageSize &&
                   this.SectorsTypeNumber == other.SectorsTypeNumber &&
                   EqualityComparer<IList<DeviceSector>>.Default.Equals(this.Sectors, other.Sectors);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.FilePath, this.DeviceName, this.DeviceType, this.DeviceStartAddress, this.DeviceSize, this.PageSize, this.SectorsTypeNumber, this.Sectors);
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
