using Utils;

namespace Engine.Tilemaps;

class Tilemap
{
    private readonly Dictionary<(int, int), TileChunk> _chunks;
    private CachedChunk _lastChunk;

    public Tilemap()
    {
        _chunks = new Dictionary<(int, int), TileChunk>();
    }

    public Tile? this[int x, int y]
    {
        get
        {
            int cx = x / TileChunk.SIZE;
            int cy = y / TileChunk.SIZE;

            if (_lastChunk.Check(cx, cy))
                return _lastChunk.chunk[x % TileChunk.SIZE, y % TileChunk.SIZE];

            if (_chunks.TryGetValue((cx, cy), out TileChunk? chunk))
            {
                _lastChunk = new CachedChunk(cx, cy, chunk);
                return _lastChunk.chunk[x % TileChunk.SIZE, y % TileChunk.SIZE];
            }

            return null;
        }
        set
        {
            int cx = x / TileChunk.SIZE;
            int cy = y / TileChunk.SIZE;

            if (!_lastChunk.Check(cx, cy))
            {
                if (_chunks.TryGetValue((cx, cy), out TileChunk? chunk))
                {
                    _lastChunk = new CachedChunk(cx, cy, chunk);
                }
                else
                {
                    chunk = ObjectPool<TileChunk>.Shared.Get();
                    _chunks[(cx, cy)] = chunk;
                    _lastChunk = new CachedChunk(cx, cy, chunk);
                }
            }

            _lastChunk.chunk[x % TileChunk.SIZE, y % TileChunk.SIZE] = value;
            if (_lastChunk.chunk.IsEmpty)
            {
                _chunks.Remove((cx, cy));
                ObjectPool<TileChunk>.Shared.Return(_lastChunk.chunk);
                _lastChunk = default;
            }
        }
    }

    public TileChunk? GetChunk(int cx, int cy)
    {
        _chunks.TryGetValue((cx, cy), out TileChunk? chunk);
        return chunk;
    }

    private struct CachedChunk
    {
        public TileChunk chunk;
        private int cx,
            cy;

        public CachedChunk(int cx, int cy, TileChunk chunk)
        {
            this.cx = cx;
            this.cy = cy;
            this.chunk = chunk;
        }

        public bool Check(int cx, int cy) => this.chunk != null && this.cx == cx && this.cy == cy;
    }
}

class TileChunk
{
    private int _count;
    private Tile?[] _tiles;

    public const int SIZE = 8;

    public TileChunk() => _tiles = new Tile[SIZE * SIZE];

    public bool IsEmpty => _count == 0;

    public Tile? this[int x, int y]
    {
        get
        {
            if (x <= -SIZE || x >= SIZE || y <= -SIZE || y >= SIZE)
                throw new ArgumentOutOfRangeException($"Coords {x} {y} are invalid.");

            int tx = (TileChunk.SIZE + (x % TileChunk.SIZE)) % TileChunk.SIZE;
            int ty = (TileChunk.SIZE + (y % TileChunk.SIZE)) % TileChunk.SIZE;
            return _tiles?[ty * SIZE + tx];
        }
        set
        {
            if (x <= -SIZE || x >= SIZE || y <= -SIZE || y >= SIZE)
                throw new ArgumentOutOfRangeException($"Coords {x} {y} are invalid.");

            int tx = (TileChunk.SIZE + (x % TileChunk.SIZE)) % TileChunk.SIZE;
            int ty = (TileChunk.SIZE + (y % TileChunk.SIZE)) % TileChunk.SIZE;

            int ind = ty * SIZE + tx;
            _count += (value == null ? 0 : 1) - (_tiles[ind] == null ? 0 : 1);
            _tiles[ind] = value;
        }
    }
}
