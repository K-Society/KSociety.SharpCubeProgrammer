// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Struct
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FileDataC
    {
        public int Type;
        public int segmentsNbr;
        public IntPtr segments;
    }
}
