// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Dfu
{
    using Enum;
    using Struct;

    public abstract class Device
    {
        #region Abstract class interface
        /// <summary>
        /// Device identifying information, assembled from device IDs and DfuDescriptor.
        /// </summary>
        public abstract Identification Info { get; }

        /// <summary>
        /// The DFU Functional Descriptor, which is either parsed from the Config descriptor,
        /// or fetched with a GetDescriptor request to the DFU interface
        /// </summary>
        public abstract FunctionalDescriptor DfuDescriptor { get; }

        /// <summary>
        /// Returns the number of available alternate settings of the DFU interface.
        /// </summary>
        protected abstract byte NumberOfAlternateSettings { get; }

        /// <summary>
        /// Gets or sets the DFU interface's alternate setting.
        /// </summary>
        protected abstract byte AlternateSetting { get; set; }

        /// <summary>
        /// Returns the string index of the specified alternate selector.
        /// </summary>
        /// <param name="altSetting">The alternate selector index</param>
        /// <returns>The string index</returns>
        protected abstract byte iAlternateSetting(byte altSetting);

        /// <summary>
        /// Reads the USB device string for a specific string index.
        /// </summary>
        /// <param name="iString">USB device string index to read with</param>
        /// <returns>The string from the device</returns>
        protected abstract string GetString(byte iString);

        /// <summary>
        /// USB class-specific control transfer to the DFU interface, with 0 length.
        /// </summary>
        /// <param name="request">Class request code</param>
        /// <param name="value">Value field of the setup request</param>
        protected abstract void ControlTransfer(Request request, ushort value = 0);

        /// <summary>
        /// USB class-specific control transfer to the DFU interface, with OUT data.
        /// </summary>
        /// <param name="request">Class request code</param>
        /// <param name="value">Value field of the setup request</param>
        /// <param name="outdata">OUT data to send to the device</param>
        protected abstract void ControlTransfer(Request request, ushort value, byte[] outdata);

        /// <summary>
        /// USB class-specific control transfer to the DFU interface, with IN data.
        /// </summary>
        /// <param name="request">Class request code</param>
        /// <param name="value">Value field of the setup request</param>
        /// <param name="indata">IN data to fill from the device</param>
        protected abstract void ControlTransfer(Request request, ushort value, ref byte[] indata);

        // not used, only added for symmetry
        //public abstract void Open();

        /// <summary>
        /// Closes the device's DFU interface.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Checks the status of the interface.
        /// </summary>
        /// <returns>True if the DFU interface is open</returns>
        public abstract bool IsOpen();

        /// <summary>
        /// Performs a USB bus reset for the device (also closing the device in the operation).
        /// Only required for devices which don't WillDetach, or are ManifestationTolerant.
        /// </summary>
        protected abstract void BusReset();
        #endregion
    }
}
