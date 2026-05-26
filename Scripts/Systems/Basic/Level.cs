using Engine.Tilemaps;
using Other;
using Utils;

namespace Systems.Basic;

public class Level : IDisposable
{
    public float time { get; protected set; }

    public readonly string name;
    public readonly ShuffleSelector<Item> itemSelector;

    private readonly Action _createPlayer;
    private readonly EnemyWave[] _waves;
    private readonly Tilemap _floor,
        _walls;

    private int _waveIndex;
    private float _waveTime;

    public Level(
        string name,
        Tilemap floor,
        Tilemap walls,
        Action createPlayer,
        EnemyWave[] waves,
        ShuffleSelector<Item> itemSelector
    )
    {
        this.name = name;
        this.itemSelector = itemSelector;

        this._floor = floor;
        this._walls = walls;

        this._createPlayer = createPlayer;
        this._waves = waves;

        _createPlayer();
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

        wave.init();
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
    public readonly Action init;

    public EnemyWave(float time, Action init)
    {
        this.time = time;
        this.init = init;
    }
}
