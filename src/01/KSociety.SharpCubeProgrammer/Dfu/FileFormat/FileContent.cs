// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Dfu.FileFormat
{
    using System.Collections.Generic;
    using Memory;
    using Struct;

    /// <summary>
    /// Simple helper class to hold the maximal amount of information that can be encoded into a DFU file.
    /// </summary>
    public class FileContent
    {
        public Identification DeviceInfo { get; private set; }
        public Dictionary<byte, NamedMemory> ImagesByAltSetting { get; private set; }

        public FileContent(Identification devInfo)
        {
            this.DeviceInfo = devInfo;
            this.ImagesByAltSetting = new Dictionary<byte, NamedMemory>();
        }
    }
}
