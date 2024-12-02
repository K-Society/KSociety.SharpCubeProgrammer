// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Enum
{
    /// <summary>
    /// Choose the way to apply a system reset.
    /// </summary>
    public enum DebugResetMode
    {
        /// <summary>
        /// Apply a reset by the software.
        /// </summary>
        SoftwareReset = 0,

        /// <summary>
        /// Apply a reset by the hardware.
        /// </summary>
        HardwareReset = 1,

        /// <summary>
        /// Apply a reset by the internal core peripheral.
        /// </summary>
        CoreReset = 2
    }
}
