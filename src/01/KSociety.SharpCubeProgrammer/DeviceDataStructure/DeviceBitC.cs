// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.DeviceDataStructure
{
    using System.Collections.Generic;

    public struct DeviceBitC
    {
        public string Name;

        public string Description;

        public uint WordOffset;
        public uint BitOffset;
        public uint BitWidth;
        public byte Access;

        public uint ValuesNbr;

        public List<BitValueC> Values;
        public BitCoefficientC Equation;
        public string Reference;
        public uint BitValue;
        public uint ValLine;
    }
}
