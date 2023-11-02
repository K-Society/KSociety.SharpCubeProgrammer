// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace KSociety.SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    //x86 = 104 byte
    //x64 = 208 byte
    /// <summary>
    /// Get supported frequencies for JTAG and SWD interfaces.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class Frequencies
    {
        //x86 = 4 byte x 12 = 48 byte
        //x64 = 8 byte x 12 = 96 byte
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.SysUInt)]
        //[MarshalAs(UnmanagedType.SysUInt, SizeConst = 12)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public uint[] JTagFrequency;

        //x86 = 4 byte
        //x64 = 8 byte
        //[MarshalAs(UnmanagedType.SysUInt)]
        public uint JTagFrequencyNumber;

        //x86 = 4 byte x 12 = 48 byte
        //x64 = 8 byte x 12 = 96 byte
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.SysUInt)]
        //[MarshalAs(UnmanagedType.SysUInt, SizeConst = 12)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public uint[] SwdFrequency;

        //x86 = 4 byte
        //x64 = 8 byte
        //[MarshalAs(UnmanagedType.SysUInt)]
        public uint SwdFrequencyNumber;
    }
}
