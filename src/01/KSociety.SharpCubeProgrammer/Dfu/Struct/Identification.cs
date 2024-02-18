// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Dfu.Struct
{
    using System;

    /// <summary>
    /// Relevant device information that identifies the target of an update.
    /// </summary>
    public struct Identification
    {
        public readonly ushort VendorId;
        public readonly ushort ProductId;
        public readonly Version ProductVersion;
        public readonly Version DfuVersion;

        public Identification(ushort vendorId, ushort productId, ushort bcdProductVersion, ushort bcdDfuVersion)
        {
            this.VendorId = vendorId;
            this.ProductId = productId;
            this.ProductVersion = new Version(bcdProductVersion >> 8, bcdProductVersion & 0xff);
            this.DfuVersion = new Version(bcdDfuVersion >> 8, bcdDfuVersion & 0xff);
        }

        internal Identification(Suffix dfuSuffix)
            : this(dfuSuffix.idVendor, dfuSuffix.idProduct, dfuSuffix.bcdDevice, dfuSuffix.bcdDFU)
        {
        }
    }
}
