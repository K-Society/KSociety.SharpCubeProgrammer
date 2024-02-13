// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Enum
{
    /// <summary>
    /// Choose the way to apply a system reset.
    /// </summary>
    public enum DebugResetMode
    {
        SoftwareReset = 0,
        HardwareReset = 1,
        CoreReset = 2
    }
}
