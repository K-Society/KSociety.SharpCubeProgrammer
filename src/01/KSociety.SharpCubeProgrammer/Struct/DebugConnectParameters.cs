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

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi/*, CharSet = CharSet.Ansi, Size = 312*/)]
    public class DebugConnectParameters : ICloneable
    {
        /// <summary>
        /// Select the type of debug interface #debugPort.
        /// </summary>
        //[FieldOffset(0)]
        [MarshalAs(UnmanagedType.I4)]
        public DebugPort DebugPort;

        /// <summary>
        /// Select one of the debug ports connected.
        /// </summary>
        //[FieldOffset(4)]
        [MarshalAs(UnmanagedType.I4)]
        public int Index;

        /// <summary>
        /// ST-LINK serial number.
        /// </summary>
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        //[FieldOffset(8)]
        public SerialNumber SerialNumber;

        /// <summary>
        /// Firmware version.
        /// </summary>
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        //[FieldOffset(41)] //41
        public FirmwareVersion FirmwareVersion;

        /// <summary>
        /// Operate voltage.
        /// </summary>
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        //[FieldOffset(61)]
        public TargetVoltage TargetVoltage;

        /// <summary>
        /// Number of available access port.
        /// </summary>
        //[FieldOffset(66)]
        [MarshalAs(UnmanagedType.I4)]
        public int AccessPortNumber;

        /// <summary>
        /// Select access port controller.
        /// </summary>
        //[FieldOffset(72)] //72
        [MarshalAs(UnmanagedType.I4)]
        public int AccessPort;

        /// <summary>
        /// Select the debug CONNECT mode #debugConnectMode.
        /// </summary>
        //[FieldOffset(76)]
        [MarshalAs(UnmanagedType.I4)]
        public DebugConnectionMode ConnectionMode;

        /// <summary>
        /// Select the debug RESET mode #debugResetMode.
        /// </summary>
        //[FieldOffset(80)]
        [MarshalAs(UnmanagedType.I4)]
        public DebugResetMode ResetMode;

        /// <summary>
        /// Check Old ST-LINK firmware version.
        /// </summary>
        //[FieldOffset(84)]
        [MarshalAs(UnmanagedType.I4)]
        public int IsOldFirmware;

        /// <summary>
        /// Supported frequencies #frequencies.
        /// </summary>
        //[FieldOffset(88)] //89
        public Frequencies Frequencies;

        /// <summary>
        /// Select specific frequency.
        /// </summary>
        //[FieldOffset(192)] //192
        [MarshalAs(UnmanagedType.I4)]
        public int Frequency;

        /// <summary>
        /// Indicates if it's Bridge device or not.
        /// </summary>
        //[FieldOffset(196)]
        [MarshalAs(UnmanagedType.I4)]
        public int IsBridge;

        /// <summary>
        /// Select connection type, if it's shared, use ST-LINK Server.
        /// </summary>
        //[FieldOffset(200)]
        [MarshalAs(UnmanagedType.I4)]
        public int Shared;

        /// <summary>
        /// Board name.
        /// </summary>
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        //[FieldOffset(204)]
        public Board Board;

        //FieldOffset(304)]
        [MarshalAs(UnmanagedType.I4)]
        public int DBG_Sleep;

        /// <summary>
        /// Select speed flashing of Cortex M33 series.
        /// </summary>
        //[FieldOffset(308)]
        [MarshalAs(UnmanagedType.I4)]
        public int Speed;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
