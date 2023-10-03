namespace KSociety.SharpCubeProgrammer.Bindings
{
    using Autofac;
    using Interface;

    public class ProgrammerApi : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CubeProgrammerApi>().As<ICubeProgrammerApi>().SingleInstance().OnActivated(programmer => programmer.Instance.GetStLinkPorts());
        }
    }
}
