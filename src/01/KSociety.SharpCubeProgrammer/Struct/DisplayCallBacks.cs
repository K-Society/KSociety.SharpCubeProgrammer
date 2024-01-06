// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;

    public delegate void InitProgressBar();

    public delegate void LogMessageReceived(int messageType, [MarshalAs(UnmanagedType.LPWStr)] string message);

    public delegate void ProgressBarUpdateReceived(int currentProgress, int total);

    /// <summary>
    /// Functions must be implemented to personalize the display of messages.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DisplayCallBacks
    {
        public InitProgressBar InitProgressBar;
        public LogMessageReceived LogMessage;
        public ProgressBarUpdateReceived LoadBar;
    }
}
