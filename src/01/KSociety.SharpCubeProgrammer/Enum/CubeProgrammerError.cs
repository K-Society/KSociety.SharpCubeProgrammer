// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Enum
{
    /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerError/*'/>
    public enum CubeProgrammerError
    {
        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerNoError/*'/>
        CubeProgrammerNoError = 0,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorNotConnected/*'/>
        CubeProgrammerErrorNotConnected = -1,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorNoDevice/*'/>
        CubeProgrammerErrorNoDevice = -2,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorConnection/*'/>
        CubeProgrammerErrorConnection = -3,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorNoFile/*'/>
        CubeProgrammerErrorNoFile = -4,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorNotSupported/*'/>
        CubeProgrammerErrorNotSupported = -5,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorInterfaceNotSupported/*'/>
        CubeProgrammerErrorInterfaceNotSupported = -6,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorNoMem/*'/>
        CubeProgrammerErrorNoMem = -7,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorWrongParam/*'/>
        CubeProgrammerErrorWrongParam = -8,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorReadMem/*'/>
        CubeProgrammerErrorReadMem = -9,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorWriteMem/*'/>
        CubeProgrammerErrorWriteMem = -10,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorEraseMem/*'/>
        CubeProgrammerErrorEraseMem = -11,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorUnsupportedFileFormat/*'/>
        CubeProgrammerErrorUnsupportedFileFormat = -12,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorRefreshRequired/*'/>
        CubeProgrammerErrorRefreshRequired = -13,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorNoSecurity/*'/>
        CubeProgrammerErrorNoSecurity = -14,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorChangeFreq/*'/>
        CubeProgrammerErrorChangeFreq = -15,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorRdpEnabled/*'/>
        CubeProgrammerErrorRdpEnabled = -16,

        /* NB: Remember to update CUBEPROGRAMMER_ERROR_COUNT below. */

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerErrorOther/*'/>
        CubeProgrammerErrorOther = -99
    }
}
