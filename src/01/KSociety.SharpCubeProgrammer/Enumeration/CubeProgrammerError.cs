// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Enumeration
{
    /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/CubeProgrammerError/*'/>
    public enum CubeProgrammerError
    {
        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/NoError/*'/>
        NoError = 0,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/NotConnected/*'/>
        NotConnected = -1,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/NoDevice/*'/>
        NoDevice = -2,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/Connection/*'/>
        Connection = -3,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/NoFile/*'/>
        NoFile = -4,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/NotSupported/*'/>
        NotSupported = -5,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/InterfaceNotSupported/*'/>
        InterfaceNotSupported = -6,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/ErrorNoMem/*'/>
        NoMem = -7,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/WrongParam/*'/>
        WrongParam = -8,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/ReadMem/*'/>
        ReadMem = -9,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/WriteMem/*'/>
        WriteMem = -10,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/EraseMem/*'/>
        EraseMem = -11,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/UnsupportedFileFormat/*'/>
        UnsupportedFileFormat = -12,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/RefreshRequired/*'/>
        RefreshRequired = -13,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/NoSecurity/*'/>
        NoSecurity = -14,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/ChangeFreq/*'/>
        ChangeFreq = -15,

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/RdpEnabled/*'/>
        RdpEnabled = -16,

        /* NB: Remember to update CUBEPROGRAMMER_ERROR_COUNT below. */

        /// <include file='..\Doc\CubeProgrammerError.xml' path='docs/members[@name="cubeProgrammerError"]/Other/*'/>
        Other = -99
    }
}
