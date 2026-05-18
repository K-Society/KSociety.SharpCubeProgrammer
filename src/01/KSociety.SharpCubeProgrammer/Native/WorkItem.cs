// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Native
{
    using System;
    using System.Threading.Tasks;

    internal sealed class WorkItem
    {
        public Func<object> Work;
        public TaskCompletionSource<object> Tcs;
    }
}
