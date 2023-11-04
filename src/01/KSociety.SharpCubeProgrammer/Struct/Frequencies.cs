// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace KSociety.SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Get supported frequencies for JTAG and SWD interfaces 104.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Frequencies
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public uint[] JtagFrequency;

        [MarshalAs(UnmanagedType.U4)]
        public uint JTagFrequencyNumber;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public uint[] SwdFrequency;

        [MarshalAs(UnmanagedType.U4)]
        public uint SwdFrequencyNumber;
    }
}
