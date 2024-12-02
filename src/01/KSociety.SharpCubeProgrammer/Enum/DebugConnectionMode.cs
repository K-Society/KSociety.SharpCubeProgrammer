// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Enum
{
    /// <summary>
    /// Choose the appropriate mode for connection.
    /// </summary>
    public enum DebugConnectionMode
    {
        /// <summary>
        /// Connect with normal mode, the target is reset then halted while the type of reset is selected using the [debugResetMode].
        /// </summary>
        NormalMode = 0,

        /// <summary>
        /// Connect with hotplug mode,  this option allows the user to connect to the target without halt or reset.
        /// </summary>
        HotplugMode = 1,

        /// <summary>
        /// Connect with under reset mode, option allows the user to connect to the target using a reset vector catch before executing any instruction.
        /// </summary>
        UnderResetMode = 2,

        /// <summary>
        /// Connect with power down mode.
        /// </summary>
        PowerDownMode = 3,

        /// <summary>
        /// Connect with pre reset mode.
        /// </summary>
        PreResetMode = 4,

        /// <summary>
        /// Connect with hwRstPulse mode.
        /// </summary>
        HwRstPulseMode = 5
    }
}
