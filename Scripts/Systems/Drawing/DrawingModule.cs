using Autofac;

namespace Systems.Drawing;

class DrawingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TilemapDrawSys>().InstancePerLifetimeScope();
        builder.RegisterType<SpriteDrawSys>().InstancePerLifetimeScope();
        builder.RegisterType<LineDrawSys>().InstancePerLifetimeScope();
        builder.RegisterType<TextDrawSys>().InstancePerLifetimeScope();
    }
}
