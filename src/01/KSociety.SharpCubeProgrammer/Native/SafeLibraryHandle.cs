// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Native
{
    using Microsoft.Win32.SafeHandles;
#if NET
    using System;
    using System.Runtime.InteropServices;    
#endif

    /// <summary>
    /// A class that represents a handle to a library.  This class cannot be inherited.
    /// </summary>
    internal sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
#if NET
    /// <summary>
    /// Initializes a new instance of the <see cref="SafeLibraryHandle"/> class.
    /// </summary>
    /// <param name="handle">The pre-existing handle to use.</param>
    public SafeLibraryHandle(IntPtr handle)
        : base(true)
    {
            this.SetHandle(handle);
    }
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeLibraryHandle"/> class.
        /// </summary>
        public SafeLibraryHandle()
            : base(true)
        {
        }
#endif

        /// <summary>
        /// Executes the code required to free the handle.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the handle is released successfully;
        /// otherwise, in the event of a catastrophic failure,
        /// <see langword="false"/>. In this case, it generates a ReleaseHandleFailed
        /// Managed Debugging Assistant.
        /// </returns>
        protected override bool ReleaseHandle()
#if NET
        {
            NativeLibrary.Free(this.handle);
            return true;
        }
#else
        {
            return Utility.FreeLibrary(this.handle);
        }
#endif
    }
}
