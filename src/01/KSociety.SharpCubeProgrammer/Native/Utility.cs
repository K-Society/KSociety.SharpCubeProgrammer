// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Native
{
    using System;
    using System.Runtime.InteropServices;

    internal static class Utility
    {
        private static IntPtr DllHandle = IntPtr.Zero;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "LoadLibraryExW", SetLastError = true)]
        private static extern IntPtr LoadLibraryExW(string lpFileName, IntPtr hFile, uint dwFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "FreeLibrary", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        internal static bool LoadNativeLibrary(string lpFileName, IntPtr hFile, uint dwFlags)
        {
            try
            {
                DllHandle = LoadLibraryExW(lpFileName, hFile, dwFlags);

                if (DllHandle != IntPtr.Zero)
                {
                    return true;
                }
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }

            return false;
        }

        internal static bool FreeNativeLibrary()
        {
            try
            {
                if (DllHandle != IntPtr.Zero)
                {
                    return FreeLibrary(DllHandle);
                }
            }
            catch (DllNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer implementation not found.", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new Exception("K-Society CubeProgrammer operation not found.", ex);
            }

            return false;
        }
    }
}
