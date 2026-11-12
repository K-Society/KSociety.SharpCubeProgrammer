// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Native
{
    using System;

    public class SharpCubeProgrammerException : Exception
    {
        public SharpCubeProgrammerException()
        {
        }

        public SharpCubeProgrammerException(string message) : base(message)
        {
        }

        public SharpCubeProgrammerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
