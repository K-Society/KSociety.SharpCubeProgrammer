// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Native
{
    using System;
    using System.Runtime.InteropServices;

    public static class DisplayCallBacksFunctions
    {
        #region [DisplayCallBacks]

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        public delegate void InitProgressBar();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public delegate void LogMessageReceived(int messageType, [MarshalAs(UnmanagedType.LPWStr)] string message);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        public delegate void ProgressBarUpdateReceived(int currentProgress, int total);

        #endregion

        #region [DisplayCallBacksLinux]

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public delegate void LogMessageReceivedLinux(int messageType, IntPtr messageIntPtr);

        #endregion
    }
}
