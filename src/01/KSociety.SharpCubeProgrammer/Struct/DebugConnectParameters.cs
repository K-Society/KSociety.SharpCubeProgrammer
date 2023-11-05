// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Struct
{
    using System;
    using System.Runtime.InteropServices;
    using Enum;

    /// <summary>
    /// Get device characterization and specify connection parameters through ST-LINK interface.
    /// </summary>

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DebugConnectParameters : ICloneable
    {
        /// <summary>
        /// Select the type of debug interface #debugPort.
        /// </summary>
        public DebugPort DebugPort;

        /// <summary>
        /// Select one of the debug ports connected.
        /// </summary>
        public int Index;

        /// <summary>
        /// ST-LINK serial number.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string SerialNumber;

        /// <summary>
        /// Firmware version.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string FirmwareVersion;

        /// <summary>
        /// Operate voltage.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string TargetVoltage;

        /// <summary>
        /// Number of available access port.
        /// </summary>
        public int AccessPortNumber;

        /// <summary>
        /// Select access port controller.
        /// </summary>
        public int AccessPort;

        /// <summary>
        /// Select the debug CONNECT mode #debugConnectMode.
        /// </summary>
        public DebugConnectionMode ConnectionMode;

        /// <summary>
        /// Select the debug RESET mode #debugResetMode.
        /// </summary>
        public DebugResetMode ResetMode;

        /// <summary>
        /// Check Old ST-LINK firmware version.
        /// </summary>
        public int IsOldFirmware;

        /// <summary>
        /// Supported frequencies #frequencies.
        /// </summary>
        public Frequencies Frequencies;

        /// <summary>
        /// Select specific frequency.
        /// </summary>
        public int Frequency;

        /// <summary>
        /// Indicates if it's Bridge device or not.
        /// </summary>
        public int IsBridge;

        /// <summary>
        /// Select connection type, if it's shared, use ST-LINK Server.
        /// </summary>
        public int Shared;

        /// <summary>
        /// Board name.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Board;

        public int DBG_Sleep;

        /// <summary>
        /// Select speed flashing of Cortex M33 series.
        /// </summary>
        public int Speed;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
