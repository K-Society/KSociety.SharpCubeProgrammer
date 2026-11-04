// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Enumeration
{
    /// <summary>
    /// List of verbosity levels.
    /// </summary>
    public enum CubeProgrammerVerbosityLevel
    {
        /// <summary>
        /// No messages ever printed by the library.
        /// </summary>
        None = 0,

        /// <summary>
        /// Warning, error and success messages are printed (default).
        /// </summary>
        LevelOne = 1,

        /// <summary>
        /// Error roots informational messages are printed.
        /// </summary>
        LevelTwo = 2,

        /// <summary>
        /// Debug and informational messages are printed.
        /// </summary>
        LevelDebug = 3,

        /// <summary>
        /// No progress bar is printed in the output of the library.
        /// Progress bars are printed only with verbosity level one.
        /// </summary>
        NoProgressBar = 4
    }
}
