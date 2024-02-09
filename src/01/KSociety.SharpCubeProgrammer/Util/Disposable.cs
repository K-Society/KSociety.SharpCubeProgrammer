// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Util
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Base class for disposable objects.
    /// </summary>
    public class Disposable : IDisposable, IAsyncDisposable
    {
        private const int DisposedFlag = 1;
        private int _isDisposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Dispose is implemented correctly, FxCop just doesn't see it.")]
        public void Dispose()
        {
            var wasDisposed = Interlocked.Exchange(ref this._isDisposed, DisposedFlag);
            if (wasDisposed == DisposedFlag)
            {
                return;
            }

            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the current instance has been disposed.
        /// </summary>
        protected bool IsDisposed
        {
            get
            {
                Interlocked.MemoryBarrier();
                return this._isDisposed == DisposedFlag;
            }
        }

        /// <inheritdoc/>
        [SuppressMessage(
            "Usage",
            "CA1816:Dispose methods should call SuppressFinalize",
            Justification = "DisposeAsync should also call SuppressFinalize (see various .NET internal implementations).")]
        public ValueTask DisposeAsync()
        {
            // Still need to check if we've already disposed; can't do both.
            var wasDisposed = Interlocked.Exchange(ref this._isDisposed, DisposedFlag);
            if (wasDisposed != DisposedFlag)
            {
                GC.SuppressFinalize(this);

                // Always true, but means we get the similar syntax as Dispose,
                // and separates the two overloads.
                return this.DisposeAsync(true);
            }

            return default;
        }

        /// <summary>
        ///  Releases unmanaged and - optionally - managed resources, asynchronously.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual ValueTask DisposeAsync(bool disposing)
        {
            // Default implementation does a synchronous dispose.
            this.Dispose(disposing);

            return default;
        }
    }
}
