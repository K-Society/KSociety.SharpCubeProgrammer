// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace KSociety.SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class ExternalLoader
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string filePath;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string deviceName;

        public int deviceType;
        public uint deviceStartAddress;
        public uint deviceSize;

        public uint pageSize;

        //  unsigned char  	 EraseValue;            /**< Content of Erased Memory. */
        public uint sectorsTypeNbr;
        public DeviceSector sectors;
    }
}
