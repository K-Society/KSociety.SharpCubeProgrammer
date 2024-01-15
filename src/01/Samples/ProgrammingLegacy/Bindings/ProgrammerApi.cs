// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace ProgrammingLegacy.Bindings
{
    using Autofac;
    using KSociety.SharpCubeProgrammer;
    using KSociety.SharpCubeProgrammer.Interface;

    public class ProgrammerApi : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CubeProgrammerApi>().As<ICubeProgrammerApi>().SingleInstance();
        }
    }
}
