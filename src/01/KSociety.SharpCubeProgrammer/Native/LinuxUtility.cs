// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Native
{
    using System;
    using System.Runtime.InteropServices;

    internal static class LinuxUtility
    {
        [Flags]
        internal enum Mode : int
        {
            None = 0x0,
            Lazy = 0x1,
            Now = 0x2,
            Local = 0x4,
            Global = 0x8,
            NoLoad = 0x10,
            NoDelete = 0x80,
            First = 0x100,
        }

        /// <summary>
        /// The name of the Linux library.
        /// </summary>
        internal const string LinuxLibName = "libdl.so.2";

        [DllImport(LinuxLibName, EntryPoint = "dlopen", CharSet = CharSet.Ansi)]
        internal static extern IntPtr DlOpen([MarshalAs(UnmanagedType.LPStr)] string dllname, int mode);

        [DllImport(LinuxLibName, EntryPoint = "dlsym", CharSet = CharSet.Ansi)]
        internal static extern IntPtr DlSym(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string symbol);

        [DllImport(LinuxLibName, EntryPoint = "dlclose", CharSet = CharSet.Ansi)]
        internal static extern int DlClose(IntPtr handle);

        [DllImport(LinuxLibName, EntryPoint = "dlerror", CharSet = CharSet.Ansi)]
        internal static extern IntPtr DlError();
    }
}
