using Arch.Core;
using Autofac;
using Components.Other;
using Components.Physics;
using Engine.Common;
using Engine.Tilemaps;
using Systems.Basic;

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

        Level level = new Level(
            "level_1",
            floor,
            walls,
            () => x.ResolveNamed<Entity>("player"),
            new EnemyWave[]
            {
                new EnemyWave(
                    30.0f,
                    () =>
                    {
                        const int GOBLIN_COUNT = 500;

                        Entity goblin = x.ResolveNamed<Entity>("goblin");
                        world.EnsureCapacity(x.Resolve<World>().GetSignature(goblin), GOBLIN_COUNT);
                        for (int i = 1; i < GOBLIN_COUNT; i++)
                            x.ResolveNamed<Entity>("goblin");

                        // other enemies in same manner
                    }
                ),
                new EnemyWave(
                    10.0f,
                    () =>
                    {
                        const int GOBLIN_COUNT = 200;

                        Entity goblin = x.ResolveNamed<Entity>("goblin");
                        world.EnsureCapacity(x.Resolve<World>().GetSignature(goblin), GOBLIN_COUNT);
                        for (int i = 1; i < GOBLIN_COUNT; i++)
                            x.ResolveNamed<Entity>("goblin");

                        // other enemies in same manner
                    }
                ),
            }
        );

        return level;
    }
}
