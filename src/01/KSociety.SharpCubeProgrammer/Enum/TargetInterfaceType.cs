// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Enum
{
    /// <summary>
    /// Indicates the supported interfaces.
    /// </summary>
    public enum TargetInterfaceType
    {
        /// <summary>
        /// STLINK used as connection interface.
        /// </summary>
        StlinkInterface = 0,

        /// <summary>
        /// USART used as connection interface.
        /// </summary>
        UsartInterface = 1,

        /// <summary>
        /// USB DFU used as connection interface.
        /// </summary>
        UsbInterface = 2,

        /// <summary>
        /// SPI used as connection interface.
        /// </summary>
        SpiInterface = 3,

        /// <summary>
        /// I2C used as connection interface.
        /// </summary>
        I2CInterface = 4,

        /// <summary>
        /// CAN used as connection interface.
        /// </summary>
        CanInterface = 5
    }
}
