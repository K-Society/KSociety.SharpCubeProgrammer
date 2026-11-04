// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Enumeration
{
    public enum WbFunctionArguments
    {
        FirstInstallActive = 1,
        FirstInstallNotActive = 0,
        StartStackActive = FirstInstallActive,
        StartStackNotActive = FirstInstallActive,
        VerifyFileDownloadFile = FirstInstallActive,
        DoNotVerifyDownloadFile = FirstInstallNotActive
    }
}
