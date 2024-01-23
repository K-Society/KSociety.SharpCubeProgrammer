// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Enum
{
    /// <summary>
    /// List of verbosity levels.
    /// </summary>
    public enum CubeProgrammerVerbosityLevel
    {
        /// <summary>
        /// No messages ever printed by the library.
        /// </summary>
        CubeprogrammerVerLevelNone = 0,

        /// <summary>
        /// Warning, error and success messages are printed (default).
        /// </summary>
        CubeprogrammerVerLevelOne = 1,

        /// <summary>
        /// Error roots informational messages are printed.
        /// </summary>
        CubeprogrammerVerLevelTwo = 2,

        /// <summary>
        /// Debug and informational messages are printed.
        /// </summary>
        CubeprogrammerVerLevelDebug = 3,

        /// <summary>
        /// No progress bar is printed in the output of the library.
        /// Progress bars are printed only with verbosity level one.
        /// </summary>
        CubeprogrammerNoProgressBar = 4
    }
}
