// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Enum
{
    /// <summary>
    /// List of errors that can be occured.
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

        /** No such file  */
        CubeprogrammerErrorNoFile = -4,

        /** Operation not supported or unimplemented on this interface */
        CubeprogrammerErrorNotSupported = -5,

        /** Interface not supported or unimplemented on this plateform */
        CubeprogrammerErrorInterfaceNotSupported = -6,

        /** Insufficient memory */
        CubeprogrammerErrorNoMem = -7,

        /** Wrong parameters */
        CubeprogrammerErrorWrongParam = -8,

        /** Memory read failure */
        CubeprogrammerErrorReadMem = -9,

        /** Memory write failure */
        CubeprogrammerErrorWriteMem = -10,

        /** Memory erase failure */
        CubeprogrammerErrorEraseMem = -11,

        /** File format not supported for this kind of device */
        CubeprogrammerErrorUnsupportedFileFormat = -12,

        /** Refresh required **/
        CubeprogrammerErrorRefreshRequired = -13,

        /** Refresh required **/
        CubeprogrammerErrorNoSecurity = -14,

        /** Changing frequency problem **/
        CubeprogrammerErrorChangeFreq = -15,

        /** RDP Enabled error **/
        CubeprogrammerErrorRdpEnabled = -16,

        /* NB: Remember to update CUBEPROGRAMMER_ERROR_COUNT below. */

        /** Other error */
        CubeprogrammerErrorOther = -99,

        CubeprogrammerErrorDeviceNotConnected = -545,
    }
}
