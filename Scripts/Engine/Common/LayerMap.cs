namespace Engine.Common;

class LayerMap
{
    private readonly string[] _layers;

    public LayerMap(params string[] layers)
    {
        _layers = layers;
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
}
