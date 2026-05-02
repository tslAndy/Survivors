using Autofac;

namespace Systems.Drawing;

class DrawingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<SpriteDrawSys>().InstancePerLifetimeScope();
        builder.RegisterType<TextDrawSys>().InstancePerLifetimeScope();
        builder.RegisterType<TilemapDrawSys>().InstancePerLifetimeScope();
    }
}
