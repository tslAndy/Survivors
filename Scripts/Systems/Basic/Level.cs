using Arch.Core;
using Autofac;
using Engine.Tilemaps;
using Other;
using Utils;

namespace Systems.Basic;

public class Level : IDisposable
{
    public float time { get; protected set; }

    public readonly string name;
    public readonly ShuffleSelector<Item> itemSelector;
    public readonly ILifetimeScope scope;

    private readonly EnemyWave[] _waves;
    private readonly Tilemap _floor,
        _walls;

    private int _waveIndex;
    private float _waveTime;

    public Level(
        string name,
        Tilemap floor,
        Tilemap walls,
        EnemyWave[] waves,
        ShuffleSelector<Item> itemSelector,
        ILifetimeScope scope
    )
    {
        this.name = name;
        this.itemSelector = itemSelector;
        this.scope = scope;

        this._floor = floor;
        this._walls = walls;

        this._waves = waves;

        scope.ResolveNamed<Entity>("player");
        if (waves.Length > 0)
            InitWave(0);
    }

    public void Update(float dt)
    {
        time += dt;

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
        EnemyWave wave = _waves[index];

        _waveIndex = index;
        _waveTime = wave.time;

        wave.init(scope);
    }

    public void Dispose()
    {
        _floor.Dispose();
        _walls.Dispose();
    }
}

public class EnemyWave
{
    // цикл встроен в функцию создания
    public readonly float time;
    public readonly Action<ILifetimeScope> init;

    public EnemyWave(float time, Action<ILifetimeScope> init)
    {
        this.time = time;
        this.init = init;
    }
}
