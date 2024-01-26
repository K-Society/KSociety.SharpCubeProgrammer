// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
    public delegate void InitProgressBar();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
    public delegate void LogMessageReceived(int messageType, [MarshalAs(UnmanagedType.LPWStr)] string message);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
    public delegate void ProgressBarUpdateReceived(int currentProgress, int total);

    /// <summary>
    /// Functions must be implemented to personalize the display of messages.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DisplayCallBacks
    {
        #if NETSTANDARD2_0

        public InitProgressBar InitProgressBar;
        public LogMessageReceived LogMessage;
        public ProgressBarUpdateReceived LoadBar;

        #elif NETSTANDARD2_1

        public InitProgressBar? InitProgressBar;
        public LogMessageReceived? LogMessage;
        public ProgressBarUpdateReceived? LoadBar;

        #endif
    }
}
