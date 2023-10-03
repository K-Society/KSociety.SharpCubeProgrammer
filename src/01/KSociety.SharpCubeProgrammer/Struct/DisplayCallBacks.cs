namespace KSociety.SharpCubeProgrammer.Struct
{
    using System;
    using System.Runtime.InteropServices;

    public delegate void InitProgressBar();

    public delegate void LogMessageReceived(int messageType, [MarshalAs(UnmanagedType.LPWStr)] string message);

    public delegate void ProgressBarUpdateReceived(int currentProgress, int total);

    /// <summary>
    /// Functions must be implemented to personalize the display of messages.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class DisplayCallBacks
    {
        public IntPtr InitProgressBar; // = IntPtr.Zero;
        public IntPtr LogMessage; // = IntPtr.Zero;
        public IntPtr LoadBar; // = IntPtr.Zero;
    }
}
