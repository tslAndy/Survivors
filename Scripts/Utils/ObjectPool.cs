using Components.Fighting;

namespace Utils;

class ObjectPool<T>
    where T : class, new()
{
    public static readonly ObjectPool<T> Shared = new ObjectPool<T>();
    private readonly Stack<T> _elems = new Stack<T>();

    public T Get()
    {
        if (_elems.TryPop(out T? elem))
            return elem;
        return new T();
    }

    public void Return(T elem)
    {
        if (elem is IResettable resettable)
            resettable.Reset();
        _elems.Push(elem);
    }
}

interface IResettable
{
    void Reset();
}
