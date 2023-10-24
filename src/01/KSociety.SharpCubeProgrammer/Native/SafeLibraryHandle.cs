// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Native
{
    using System.Runtime.ConstrainedExecution;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// A class that represents a handle to a library.  This class cannot be inherited.
    /// </summary>
    internal sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeLibraryHandle"/> class.
        /// </summary>
        public SafeLibraryHandle()
            : base(true)
        {
        }

        /// <summary>
        /// Executes the code required to free the handle.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the handle is released successfully;
        /// otherwise, in the event of a catastrophic failure,
        /// <see langword="false"/>. In this case, it generates a ReleaseHandleFailed
        /// Managed Debugging Assistant.
        /// </returns>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            return Utility.FreeNativeLibrary(this.handle);
        }
    }
}
