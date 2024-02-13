// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Enum
{
    /// <summary>
    /// List of errors that can be occurred.
    /// </summary>
    public enum CubeProgrammerError
    {
        /// <summary>
        /// Success (no error)
        /// </summary>
        CubeprogrammerNoError = 0,

        /// <summary>
        /// Device not connected.
        /// </summary>
        CubeprogrammerErrorNotConnected = -1,

        /// <summary>
        /// Device not found.
        /// </summary>
        CubeprogrammerErrorNoDevice = -2,

        /// <summary>
        /// Device connection error.
        /// </summary>
        CubeprogrammerErrorConnection = -3,

        /// <summary>
        /// No such file.
        /// </summary> 
        CubeprogrammerErrorNoFile = -4,

        /// <summary>
        /// Operation not supported or unimplemented on this interface.
        /// </summary>
        CubeprogrammerErrorNotSupported = -5,

        /// <summary>
        /// Interface not supported or unimplemented on this platform.
        /// </summary>
        CubeprogrammerErrorInterfaceNotSupported = -6,

        /// <summary>
        /// Insufficient memory.
        /// </summary>
        CubeprogrammerErrorNoMem = -7,

        /// <summary>
        /// Wrong parameters.
        /// </summary>
        CubeprogrammerErrorWrongParam = -8,

        /// <summary>
        /// Memory read failure.
        /// </summary>
        CubeprogrammerErrorReadMem = -9,

        /// <summary>
        /// Memory write failure.
        /// </summary>
        CubeprogrammerErrorWriteMem = -10,

        /// <summary>
        /// Memory erase failure.
        /// </summary>
        CubeprogrammerErrorEraseMem = -11,

        /// <summary>
        /// File format not supported for this kind of device.
        /// </summary>
        CubeprogrammerErrorUnsupportedFileFormat = -12,

        /// <summary>
        /// Refresh required.
        /// </summary>
        CubeprogrammerErrorRefreshRequired = -13,

        /// <summary>
        /// Refresh required.
        /// </summary>
        CubeprogrammerErrorNoSecurity = -14,

        /// <summary>
        /// Changing frequency problem.
        /// </summary>
        CubeprogrammerErrorChangeFreq = -15,

        /// <summary>
        /// RDP Enabled error.
        /// </summary>
        CubeprogrammerErrorRdpEnabled = -16,

        /* NB: Remember to update CUBEPROGRAMMER_ERROR_COUNT below. */

        /// <summary>
        /// Other error.
        /// </summary>
        CubeprogrammerErrorOther = -99,

        /// <summary>
        /// Device Not Connected.
        /// </summary>
        CubeprogrammerErrorDeviceNotConnected = -545,
    }
}
