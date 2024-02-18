// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Dfu
{
    using System.Runtime.InteropServices;

    public static class ByteArray
    {
        /// <summary>
        /// Converts a byte array to a fixed size managed struct.
        /// </summary>
        /// <typeparam name="T">Type of the resulting struct</typeparam>
        /// <param name="bytes">The struct data in raw bytes</param>
        /// <returns>The struct created by the byte array input</returns>
        public static T ToStruct<T>(this byte[] bytes) where T : struct
        {
            T s;
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                s = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
            return s;
        }

        /// <summary>
        /// Converts a fixed size struct to a byte array.
        /// </summary>
        /// <param name="obj">The structure to convert</param>
        /// <returns>The struct data in raw bytes</returns>
        public static byte[] ToBytes(this object obj)
        {
            var size = Marshal.SizeOf(obj);
            var arr = new byte[size];

            var handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            try
            {
                Marshal.Copy(handle.AddrOfPinnedObject(), arr, 0, size);
            }
            finally
            {
                handle.Free();
            }
            return arr;
        }
    }
}
