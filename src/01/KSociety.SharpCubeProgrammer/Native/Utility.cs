// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Native
{
    using System;
    using System.Runtime.InteropServices;

    internal static class Utility
    {
        //private static SafeLibraryHandle DllHandle;

        /// <summary>
        /// The name of the Windows Kernel library.
        /// </summary>
        private const string KernelLibName = "kernel32.dll";

        /// <summary>
        /// Loads the specified module into the address space of the calling process.
        /// The specified module may cause other modules to be loaded.
        /// </summary>
        /// <param name="lpFileName">The name of the module.</param>
        /// <param name="hFile">This parameter is reserved for future use. It must be <see cref="IntPtr.Zero"/>.</param>
        /// <param name="dwFlags">The action to be taken when loading the module.</param>
        /// <returns>
        /// If the function succeeds, the return value is a handle to the module.
        /// If the function fails, the return value is <see langword="null"/>.
        /// </returns>
        /// <remarks>
        /// See <c>https://docs.microsoft.com/en-gb/windows/desktop/api/libloaderapi/nf-libloaderapi-loadlibraryexa</c>.
        /// </remarks>
        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        [DllImport(KernelLibName, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = true)]
        private static extern SafeLibraryHandle LoadLibraryEx(
            [MarshalAs(UnmanagedType.LPStr)] string lpFileName,
            IntPtr hFile,
            int dwFlags);

        /// <summary>
        /// Frees a specified library.
        /// </summary>
        /// <param name="hModule">The handle to the module to free.</param>
        /// <returns>Whether the library was successfully unloaded.</returns>
        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        [DllImport(KernelLibName)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeLibrary(IntPtr hModule);

        internal static SafeLibraryHandle LoadNativeLibrary(string lpFileName, IntPtr hFile, int dwFlags)
        {
            try
            {
                 return LoadLibraryEx(lpFileName, hFile, dwFlags);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }

        internal static bool FreeNativeLibrary(IntPtr hModule)
        {
            try
            {
                return FreeLibrary(hModule);
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }
        }
    }
}
