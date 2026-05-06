namespace Engine.ResourceManagement;

abstract class ResourceManager<T> : IDisposable
{
    private readonly Dictionary<string, T> _resources;

    protected ResourceManager()
    {
        _resources = new Dictionary<string, T>();
    }

    protected abstract T Load(string path);

    protected virtual void Unload(T resource) { }

    public T Get(string path)
    {
        T? resource;
        if (_resources.TryGetValue(path, out resource))
            return resource;

        resource = Load(path);
        _resources[path] = resource;
        return resource;
    }

    public void Dispose()
    {
        foreach (T resource in _resources.Values)
            Unload(resource);
    }
}
