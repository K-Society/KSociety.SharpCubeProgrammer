// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Struct
{
    using System.Collections.Generic;

    public struct DeviceFileDataC
    {
        public int Type;
        public int segmentsNbr;
        public List<DeviceSegmentDataC> segments;
    }
}
