// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Dfu.Memory
{
    public class NamedMemory : RawMemory
    {
        public string Name { get; private set; }

        public NamedMemory(string name)
        {
            this.Name = name;
        }
    }
}
