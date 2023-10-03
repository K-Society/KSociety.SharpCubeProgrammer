// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Enum
{
    public enum UsartFlowControl
    {
        /// <summary>
        /// No flow control.
        /// </summary>
        Off = 0,

        /// <summary>
        /// Hardware flow control : RTS/CTS.
        /// </summary>
        Hardware = 1,

        /// <summary>
        /// Software flow control : Transmission is started and stopped by sending special characters.
        /// </summary>
        Software = 2
    }
}
