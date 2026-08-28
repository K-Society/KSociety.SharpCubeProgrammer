// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;
    using Enum;

    /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/DebugConnectParameters/*'/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DebugConnectParameters
    {
        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/DebugPort/*'/>
        public DebugPort DebugPort;

        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/DebugPort/*'/>
        public int Index;

        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/SerialNumber/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string SerialNumber;

        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/FirmwareVersion/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string FirmwareVersion;

        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/TargetVoltage/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string TargetVoltage;

        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/AccessPortNumber/*'/>
        public int AccessPortNumber;

        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/AccessPort/*'/>
        public int AccessPort;

        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/ConnectionMode/*'/>
        public DebugConnectionMode ConnectionMode;

        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/ResetMode/*'/>
        public DebugResetMode ResetMode;

        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/IsOldFirmware/*'/>
        public int IsOldFirmware;

        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/Frequencies/*'/>
        public Frequencies Frequencies;

        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/Frequency/*'/>
        public int Frequency;

        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/IsBridge/*'/>
        public int IsBridge;

        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/Shared/*'/>
        public int Shared;

        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/Board/*'/>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Board;

        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/DBG_Sleep/*'/>
        public int DBG_Sleep;

        /// <include file='..\Doc\DebugConnectParameters.xml' path='docs/members[@name="debugConnectParameters"]/Speed/*'/>
        public int Speed;
    }
}
