// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Dfu.FileFormat
{
    using System;
    using System.IO;
    using SharpCubeProgrammer.Dfu;
    using Memory;
    using Struct;

    public class Dfu
    {
        /// <summary>
        /// Tests if the file extension is associated with this file format.
        /// </summary>
        /// <param name="ext">The file extension, starting with the dot</param>
        /// <returns>True if the file extension is supported, false otherwise</returns>
        public static bool IsExtensionSupported(string ext)
        {
            return ext.Equals(".dfu", StringComparison.OrdinalIgnoreCase);
        }

        private static Suffix ReadSuffix(BinaryReader reader)
        {
            // this is the part of the file that the CRC is calculated on
            var content = new byte[reader.BaseStream.Length - 4];
            reader.BaseStream.Position = 0;
            _ = reader.Read(content, 0, content.Length);

            var suffixData = new byte[Suffix.Size];
            reader.BaseStream.Position = reader.BaseStream.Length - Suffix.Size;
            _ = reader.Read(suffixData, 0, Suffix.Size);
            var suffix = suffixData.ToStruct<Suffix>();

            // verify suffix
            if (suffix.dwCRC != Crc32.Calculate(content))
            {
                throw new ArgumentException("The selected dfu file has invalid CRC.");
            }
            if (suffix.bLength < Suffix.Size)
            {
                throw new ArgumentException("The selected dfu file has invalid suffix length.");
            }
            if (suffix.sDfuSignature != Suffix.Signature)
            {
                throw new ArgumentException("The selected dfu file has invalid suffix signature.");
            }

            return suffix;
        }

        /// <summary>
        /// Extracts the device and firmware version information of a DFU file.
        /// </summary>
        /// <param name="filepath">Path to the DFU file</param>
        /// <returns>The device and image version information</returns>
        public static Identification ParseFileInfo(string filepath)
        {
            using (var reader = new BinaryReader(File.Open(filepath, FileMode.Open)))
            {
                var suffix = ReadSuffix(reader);
                return new Identification(suffix);
            }
        }

        /// <summary>
        /// Extracts the contents of a DFU file.
        /// </summary>
        /// <param name="filepath">Path to the DFU file</param>
        /// <returns>The device and memory image information</returns>
        public static FileContent ParseFile(string filepath)
        {
            using (var reader = new BinaryReader(File.Open(filepath, FileMode.Open)))
            {
                var suffix = ReadSuffix(reader);
                var devInfo = new Identification(suffix);
                var fc = new FileContent(devInfo);

                // remove the suffix from the contents
                var content = new byte[reader.BaseStream.Length - suffix.bLength];
                reader.BaseStream.Position = 0;
                _ = reader.Read(content, 0, content.Length);

                // if the protocol version is according to USB spec
                if (fc.DeviceInfo.DfuVersion <= Protocol.LatestVersion)
                {
                    // no name nor address is stored in the file, use whatever
                    var mem = new NamedMemory("default");

                    // the rest of the contents is entirely the firmware
                    mem.TryAddSegment(new Segment(~0ul, content));
                    fc.ImagesByAltSetting.Add(0, mem);
                }
                // if the protocol version is according to STMicroelectronics Extension
                else if (fc.DeviceInfo.DfuVersion == Protocol.SeVersion)
                {
                    long fileOffset = 0;
                    BufferAllocDelegate getDataChunk = (chunkSize) =>
                    {
                        var chunkData = new byte[chunkSize];
                        Array.Copy(content, fileOffset, chunkData, 0, chunkSize);
                        fileOffset += chunkSize;
                        return chunkData;
                    };

                    // DfuSe prefix
                    var prefix = getDataChunk(SePrefix.Size).ToStruct<SePrefix>();

                    if (prefix.sSignature != SePrefix.Signature)
                    {
                        throw new ArgumentException("The selected dfu file has invalid DfuSe prefix signature.");
                    }

                    // there are a number of targets, each of which is mapped to a different alternate setting
                    for (var tt = 0; tt < prefix.bTargets; tt++)
                    {
                        // image target prefix
                        var target = getDataChunk(SeTargetPrefix.Size).ToStruct<SeTargetPrefix>();

                        if (target.sSignature != SeTargetPrefix.Signature)
                        {
                            throw new ArgumentException("The selected dfu file has invalid DfuSe target prefix signature.");
                        }
                        // TODO
                        //if (!target.bTargetNamed)

                        var nmem = new NamedMemory(target.sTargetName);

                        // each target contains a number of elements (memory segments)
                        for (uint e = 0; e < target.dwNbElements; e++)
                        {
                            var elem = getDataChunk(SeElementHeader.Size).ToStruct<SeElementHeader>();
                            nmem.TryAddSegment(new Segment(elem.dwElementAddress, getDataChunk(elem.dwElementSize)));
                        }

                        // the target's alternate setting is the dictionary index
                        fc.ImagesByAltSetting.Add(target.bAlternateSetting, nmem);
                    }

                    // no leftover data is allowed
                    if ((fileOffset + suffix.bLength) != reader.BaseStream.Length)
                    {
                        throw new ArgumentException(String.Format("The selected dfu file has unprocessed data starting at {0}.", fileOffset));
                    }
                }
                else
                {
                    throw new ArgumentException("The selected dfu file has unsupported DFU specification version.");
                }

                return fc;
            }
        }

        private delegate byte[] BufferAllocDelegate(long size);
    }
}
