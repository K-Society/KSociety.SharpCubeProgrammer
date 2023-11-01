// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace KSociety.SharpCubeProgrammer.Struct
{
    using System;
    using System.Runtime.InteropServices;
    using Enum;

    /// <summary>
    /// Get device characterization and specify connection parameters through ST-LINK interface.
    /// </summary>

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class DebugConnectParameters : ICloneable
    {
        public DebugPort DebugPort;
        public int Index;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string SerialNumber;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string FirmwareVersion;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string TargetVoltage;

        public int AccessPortNumber;
        public int AccessPort;
        public DebugConnectionMode ConnectionMode;
        public DebugResetMode ResetMode;
        public int IsOldFirmware;
        public Frequencies Frequencies;
        public int Frequency;
        public int IsBridge;
        public int Shared;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Board;

        [MarshalAs(UnmanagedType.SysInt)]
        public int DBG_Sleep;

        /// <summary>
        /// Select speed flashing of Cortex M33 series.
        /// </summary>
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
