namespace Utils;

static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
    private static object? cached;

    public static T Get<T>()
        where T : class
    {
        if (cached?.GetType() == typeof(T))
            return (T)cached;
        T result = (T)_services[typeof(T)];
        cached = result;
        return result;
    }

    public static void Register<T>(T service)
        where T : class
    {
        _services[typeof(T)] = service;
    }

    public static void Dispose()
    {
        foreach (object obj in _services.Values)
        {
            if (obj is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
