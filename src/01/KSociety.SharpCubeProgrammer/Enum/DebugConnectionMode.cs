// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Enum
{
    /// <summary>
    /// Choose the appropriate mode for connection.
    /// </summary>
    public enum DebugConnectionMode
    {
        NormalMode = 0,
        HotplugMode = 1,
        UnderResetMode = 2,
        PowerDownMode = 3,
        PreResetMode = 4
    }
}
