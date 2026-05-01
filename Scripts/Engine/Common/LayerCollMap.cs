namespace Engine.Common;

class LayerCollMap
{
    private readonly long[] _layerDetect;

    private const int MAX_LAYERS = 64;

    public LayerCollMap()
    {
        _layerDetect = new long[MAX_LAYERS * MAX_LAYERS / 64];
    }

    public bool this[int x, int y]
    {
        get
        {
            if (x < 0 || x >= MAX_LAYERS || y < 0 || y >= MAX_LAYERS)
                throw new ArgumentException($"Layers {x} and {y} are incorrect");
            return this[y * MAX_LAYERS + x] == 1 ? true : false;
        }
        set
        {
            if (x < 0 || x >= MAX_LAYERS || y < 0 || y >= MAX_LAYERS)
                throw new ArgumentException($"Layers {x} and {y} are incorrect");
            int val = value ? 1 : 0;
            this[y * MAX_LAYERS + x] = val;
            this[x * MAX_LAYERS + y] = val;
        }
    }

    private long this[int index]
    {
        get
        {
            int div = index >> 6;
            int rem = index & 63;
            return (_layerDetect[div] >> rem) & 1;
        }
        set
        {
            int div = index >> 6;
            int rem = index & 63;

            value &= 1;
            _layerDetect[div] &= ~(1L << rem);
            _layerDetect[div] |= value << rem;
        }
    }
}
