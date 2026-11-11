// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    /// <include file='..\Doc\DisplayCallBacks.xml' path='docs/members[@name="displayCallBacks"]/InitProgressBar/*'/>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
    public delegate void InitProgressBar();

    /// <include file='..\Doc\DisplayCallBacks.xml' path='docs/members[@name="displayCallBacks"]/LogMessageReceived/*'/>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
    public delegate void LogMessageReceived(int messageType, [MarshalAs(UnmanagedType.LPWStr)] string message);

    /// <include file='..\Doc\DisplayCallBacks.xml' path='docs/members[@name="displayCallBacks"]/ProgressBarUpdateReceived/*'/>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
    public delegate void ProgressBarUpdateReceived(int currentProgress, int total);

    /// <include file='..\Doc\DisplayCallBacks.xml' path='docs/members[@name="displayCallBacks"]/DisplayCallBacks/*'/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct DisplayCallBacks
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <include file='..\Doc\DisplayCallBacks.xml' path='docs/members[@name="displayCallBacks"]/InitProgressBar/*'/>
        public InitProgressBar InitProgressBar;

        /// <include file='..\Doc\DisplayCallBacks.xml' path='docs/members[@name="displayCallBacks"]/LogMessageReceived/*'/>
        public LogMessageReceived LogMessage;

        /// <include file='..\Doc\DisplayCallBacks.xml' path='docs/members[@name="displayCallBacks"]/ProgressBarUpdateReceived/*'/>
        public ProgressBarUpdateReceived LoadBar;
    }
}
