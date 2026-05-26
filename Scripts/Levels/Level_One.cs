using Arch.Core;
using Autofac;
using Components.Other;
using Components.Physics;
using Engine.Common;
using Engine.Sounds;
using Engine.Sprites;
using Engine.Tilemaps;
using Other;
using Systems.Animation;
using Systems.Basic;
using Utils;

namespace Levels;

static class Level_One
{
    public static Level Get(ILifetimeScope x)
    {
        TilemapManager tilemapManager = x.Resolve<TilemapManager>();
        Tilemap floor = tilemapManager.Get("./Resources/Tilemaps/Level_A/Level_A_Floor.tilemap");
        Tilemap walls = tilemapManager.Get("./Resources/Tilemaps/Level_A/Level_A_Walls.tilemap");

        World world = x.Resolve<World>();
        LayerMap layerMap = x.Resolve<LayerMap>();

        world.Create<TilemapComp>(new TilemapComp { tilemap = floor, drawOrder = 0 });
        world.Create<TilemapComp, RigidComp>(
            new TilemapComp { tilemap = walls, drawOrder = 1 },
            new RigidComp { layer = layerMap["Walls"] }
        );
        //
        x.Resolve<SoundSys>().music = x.Resolve<MusicManager>()
            .Get("./Resources/Music/01_Invitation.mp3");

        SpriteAtlas itemsAtlas = x.Resolve<SpriteAtlasManager>()
            .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas");

        // TODO: change x in delegates to PrefabCreator
        Level level = new Level(
            "level_1",
            floor,
            walls,
            new EnemyWave[]
            {
                new EnemyWave(
                    30.0f,
                    x =>
                    {
                        const int GOBLIN_COUNT = 100;
                        Entity goblin = x.ResolveNamed<Entity>("goblin");
                        world.EnsureCapacity(x.Resolve<World>().GetSignature(goblin), GOBLIN_COUNT);
                        for (int i = 1; i < GOBLIN_COUNT; i++)
                            x.ResolveNamed<Entity>("goblin");
                    }
                ),
                new EnemyWave(
                    30.0f,
                    x =>
                    {
                        const int GOBLIN_COUNT = 200;
                        Entity goblin = x.ResolveNamed<Entity>("goblin");
                        world.EnsureCapacity(x.Resolve<World>().GetSignature(goblin), GOBLIN_COUNT);
                        for (int i = 1; i < GOBLIN_COUNT; i++)
                            x.ResolveNamed<Entity>("goblin");
                    }
                ),
                new EnemyWave(
                    30.0f,
                    x =>
                    {
                        const int GOBLIN_COUNT = 300;
                        Entity goblin = x.ResolveNamed<Entity>("goblin");
                        world.EnsureCapacity(x.Resolve<World>().GetSignature(goblin), GOBLIN_COUNT);
                        for (int i = 1; i < GOBLIN_COUNT; i++)
                            x.ResolveNamed<Entity>("goblin");
                    }
                ),
                new EnemyWave(
                    30.0f,
                    x =>
                    {
                        const int GOBLIN_COUNT = 200;
                        Entity goblin = x.ResolveNamed<Entity>("goblin");
                        world.EnsureCapacity(x.Resolve<World>().GetSignature(goblin), GOBLIN_COUNT);
                        for (int i = 1; i < GOBLIN_COUNT; i++)
                            x.ResolveNamed<Entity>("goblin");
                    }
                ),
                new EnemyWave(
                    30.0f,
                    x =>
                    {
                        const int GOBLIN_COUNT = 200;
                        Entity goblin = x.ResolveNamed<Entity>("goblin");
                        world.EnsureCapacity(x.Resolve<World>().GetSignature(goblin), GOBLIN_COUNT);
                        for (int i = 1; i < GOBLIN_COUNT; i++)
                            x.ResolveNamed<Entity>("goblin");
                    }
                ),
            },
            new ShuffleSelector<Item>(
                x.ResolveNamed<Item>("simpleBowItem"),
                x.ResolveNamed<Item>("simpleSpinItem"),
                x.ResolveNamed<Item>("simpleKunaiItem"),
                x.ResolveNamed<Item>("simpleBoomerangItem"),
                x.ResolveNamed<Item>("simpleTrailBowItem"),
                x.ResolveNamed<Item>("simpleLaserItem"),
                x.ResolveNamed<Item>("simpleBookItem"),
                x.ResolveNamed<Item>("simpleCardItem"),
                x.ResolveNamed<Item>("curseShieldItem")
            ),
            x
        );

        return level;
    }
}
