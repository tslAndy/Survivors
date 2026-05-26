namespace Engine.Common;

public class LayerMap
{
    private readonly string[] _layers;
    private readonly bool[] _collSolve;

    public LayerMap(params string[] layers)
    {
        _layers = layers;
        _collSolve = new bool[layers.Length];
    }

    public int this[string name]
    {
        get
        {
            int index = _layers.IndexOf(name);
            if (index < 0)
                throw new ArgumentException($"Layer {name} does not exist", nameof(name));
            return index;
        }
    }

    public void SetCollSolve(string name, bool val) => SetCollSolve(_layers.IndexOf(name), val);

    public bool GetCollSolve(string name) => GetCollSolve(_layers.IndexOf(name));

    public void SetCollSolve(int layerInd, bool val) => _collSolve[layerInd] = val;

    public bool GetCollSolve(int layerInd) => _collSolve[layerInd];
}
