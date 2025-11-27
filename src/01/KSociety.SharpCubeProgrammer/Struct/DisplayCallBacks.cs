// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Runtime.InteropServices;
    using static SharpCubeProgrammer.Native.DisplayCallBacksFunctions;

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
