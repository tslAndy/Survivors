using System.Numerics;
using Autofac;
using Engine.Animations;
using Engine.Common;
using Engine.Input;
using Engine.Sounds;
using Engine.Sprites;
using Engine.Tilemaps;

namespace Engine;

public class EngineModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TextureManager>().InstancePerLifetimeScope();
        builder.RegisterType<SpriteAtlasManager>().InstancePerLifetimeScope();

        builder.RegisterType<MusicManager>().InstancePerLifetimeScope();
        builder.RegisterType<SoundManager>().InstancePerLifetimeScope();
        builder.RegisterType<SoundAtlasManager>().InstancePerLifetimeScope();

        builder.RegisterType<AnimAtlasManager>().InstancePerLifetimeScope();

        builder.RegisterType<TilesetManager>().InstancePerLifetimeScope();
        builder.RegisterType<TilemapManager>().InstancePerLifetimeScope();

        builder.RegisterType<InputHandler>().InstancePerLifetimeScope();

        builder
            .Register<Camera>(_ => new Camera(1280, 720, new Vector2(20.0f, 15.0f), 50.0f))
            .InstancePerLifetimeScope();

        builder
            .Register<LayerMap>(_ =>
            {
                LayerMap layerMap = new LayerMap(
                    "PlayerEnts",
                    "EnemyEnts",
                    "PlayerBullets",
                    "EnemyBullets",
                    "Loot",
                    "Walls"
                );

                layerMap.SetCollSolve("PlayerEnts", true);
                layerMap.SetCollSolve("EnemyEnts", true);

                return layerMap;
            })
            .InstancePerLifetimeScope();
    }
}
