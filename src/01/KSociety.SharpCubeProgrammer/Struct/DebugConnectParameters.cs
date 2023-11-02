// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace KSociety.SharpCubeProgrammer.Struct
{
    using System;
    using System.Runtime.InteropServices;
    using Enum;

    //x86 = 311 byte
    //x64 = 8 byte

    /// <summary>
    /// Get device characterization and specify connection parameters through ST-LINK interface.
    /// </summary>

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class DebugConnectParameters : ICloneable
    {
        //x86 = 4 byte
        //x64 = 1 byte
        public DebugPort DebugPort;

        //x86 = 4 byte
        //x64 = 8 byte
        //[MarshalAs(UnmanagedType.SysInt)]
        public int Index;

        //x86 = 33 byte
        //x64 = 8 byte
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string SerialNumber;

        //x86 = 20 byte
        //x64 = 8 byte
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string FirmwareVersion;

        //x86 = 5 byte
        //x64 = 8 byte
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string TargetVoltage;

        //x86 = 4 byte
        //x64 = 8 byte
        //[MarshalAs(UnmanagedType.SysInt)]
        public int AccessPortNumber;

        //x86 = 4 byte 71
        //x64 = 8 byte
        //[MarshalAs(UnmanagedType.SysInt)]
        public int AccessPort;

        //x86 = 4 byte
        //x64 = 8 byte
        public DebugConnectionMode ConnectionMode;

        //x86 = 4 byte
        //x64 = 8 byte
        public DebugResetMode ResetMode;

        //x86 = 4 byte
        //x64 = 8 byte
        //[MarshalAs(UnmanagedType.SysInt)]
        public int IsOldFirmware;

        //x86 = 104 byte
        //x64 = 208 byte
        public Frequencies Frequencies;

        //x86 = 4 byte
        //x64 = 8 byte
        //[MarshalAs(UnmanagedType.SysInt)]
        public int Frequency;

        //x86 = 4 byte
        //x64 = 8 byte
        //[MarshalAs(UnmanagedType.SysInt)]
        public int IsBridge;

        //x86 = 4 byte
        //x64 = 8 byte
        //[MarshalAs(UnmanagedType.SysInt)]
        public int Shared;

        //x86 = 100 byte
        //x64 = 8 byte
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Board;

        //x86 = 4 byte
        //x64 = 8 byte
        //[MarshalAs(UnmanagedType.SysInt)]
        public int DBG_Sleep;

        //x86 = 4 byte
        //x64 = 8 byte
        /// <summary>
        /// Select speed flashing of Cortex M33 series.
        /// </summary>
        //[MarshalAs(UnmanagedType.SysInt)]
        public int Speed;

        public object Clone()
        {
            var clone = this.MemberwiseClone();

            //clone.SerialNumber = new string(SerialNumber);
            //clone.FirmwareVersion = new string(FirmwareVersion);
            //clone.TargetVoltage = new string(TargetVoltage);


            return clone;
        }
    }
}
