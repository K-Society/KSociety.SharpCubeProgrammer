// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Enum
{
    /// <summary>
    /// The parity bit in the data frame of the USART communication tells the receiving device if there is any error in the data bits.
    /// </summary>
    public enum UsartParity
    {
        /// <summary>
        /// Even parity bit.
        /// </summary>
        Even = 0,

        /// <summary>
        /// Odd parity bit.
        /// </summary>
        Odd = 1,

        /// <summary>
        /// No check parity.
        /// </summary>
        None = 2
    }
}
