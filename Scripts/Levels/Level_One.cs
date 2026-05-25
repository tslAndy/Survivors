using Arch.Core;
using Components.Other;
using Components.Physics;
using Engine.Common;
using Engine.Tilemaps;
using Systems.Basic;

namespace Levels;

class Level_One : Level
{
    private readonly Func<Entity> _createPlayer;
    private readonly Wave[] _waves;

    private readonly Tilemap _floor,
        _walls;

    private int _waveIndex;
    private float _waveTime;

    public Level_One(
        World world,
        TilemapManager tilemapManager,
        LayerMap layerMap,
        Func<Entity> createPlayer,
        Wave[] waves
    )
        : base(world)
    {
        _floor = tilemapManager.Get("./Resources/Tilemaps/Level_A/Level_A_Floor.tilemap");
        _walls = tilemapManager.Get("./Resources/Tilemaps/Level_A/Level_A_Walls.tilemap");

        world.Create<TilemapComp>(new TilemapComp { tilemap = _floor, drawOrder = 0 });
        world.Create<TilemapComp, RigidComp>(
            new TilemapComp { tilemap = _walls, drawOrder = 1 },
            new RigidComp { layer = layerMap["Walls"] }
        );

        _createPlayer = createPlayer;
        _waves = waves;

        _createPlayer();
    }

    protected override void OnUpdate(float dt)
    {
        if (_waveIndex >= _waves.Length)
            return;

        _waveTime -= dt;
        if (_waveTime > 0.0f)
            return;

        _waveIndex++;
        if (_waveIndex < _waves.Length)
            InitWave(_waveIndex);
    }

    private void InitWave(int index)
    {
        Wave wave = _waves[index];

        _waveIndex = index;
        _waveTime = wave.time;

        for (int i = 0; i < wave.builders.Length; i++)
            wave.builders[i].Invoke();
    }

    public override void Dispose()
    {
        _floor.Dispose();
        _walls.Dispose();
    }
}

class Wave
{
    // цикл встроен в функцию создания
    public readonly Func<Entity>[] builders;
    public readonly float time;

    public Wave(Func<Entity>[] entitites, float time)
    {
        this.builders = entitites;
        this.time = time;
    }
}
