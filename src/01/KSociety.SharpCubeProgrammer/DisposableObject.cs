// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer
{
    using System;

    public abstract class DisposableObject : IDisposable
    {
        protected bool Disposed { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DisposableObject()
        {
            this.Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (this.Disposed)
            {
                return;
            }

            if (disposing)
            {
                this.DisposeManagedResources();
            }

            this.DisposeUnmanagedResources();
            this.Disposed = true;
        }

        /// <summary>
        /// DisposeManagedResources
        /// </summary>
        protected virtual void DisposeManagedResources() { }

        /// <summary>
        /// DisposeUnmanagedResources
        /// </summary>
        protected virtual void DisposeUnmanagedResources() { }
    }
}
