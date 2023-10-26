// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.SharpCubeProgrammer.Bindings
{
    using Autofac;
    using Interface;

    public class ProgrammerApi : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CubeProgrammerApi>().As<ICubeProgrammerApi>().SingleInstance()
                .OnActivated(programmer => programmer.Instance.GetStLinkPorts());
        }
    }
}
