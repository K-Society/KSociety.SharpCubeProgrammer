// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DfuConnectParameters
    {
        public string usb_index;

        /// <summary>
        /// Request a read unprotect: value in {0,1}.
        /// </summary>
        public string rdu;

        /// <summary>
        /// Request a TZEN regression: value in {0,1}.
        /// </summary>
        public string tzenreg;
    }
}
