namespace Engine.ResourceManagement;

class ResourceManager<T> : IDisposable
{
    private readonly Dictionary<string, T> _resources;
    private readonly Func<string, T> _load;
    private readonly Action<T>? _unload;

    protected ResourceManager(Func<string, T> load, Action<T>? unload)
    {
        _resources = new Dictionary<string, T>();
        _load = load;
        _unload = unload;
    }

    public T Get(string path)
    {
        T? resource;
        if (_resources.TryGetValue(path, out resource))
            return resource;

        resource = _load(path);
        _resources[path] = resource;
        return resource;
    }

    public void Dispose()
    {
        if (_unload is null)
            return;

        foreach (T resource in _resources.Values)
            _unload(resource);
    }
}
