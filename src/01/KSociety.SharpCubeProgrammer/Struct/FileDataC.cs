// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct FileDataC
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public int Type;
        public int SegmentsNumber;
        public IntPtr Segments;
    }
}
