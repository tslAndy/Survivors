using System.Buffers;

namespace Utils;

public class CachedList<T> : IDisposable
{
    private T[] _elems;
    private int _index;

    private bool _disposed;

    private const int INITIAL_CAPACITY = 4;

    public CachedList()
    {
        _elems = ArrayPool<T>.Shared.Rent(INITIAL_CAPACITY);
        _index = 0;
    }

    public T[] Arr => _elems;
    public int Count => _index;

    public ref T this[int index]
    {
        get
        {
            if (index < 0 || _index <= index)
                throw new ArgumentOutOfRangeException(
                    nameof(index),
                    $"Index {index} is out of bounds"
                );
            return ref _elems[index];
        }
    }

    public void Add(T elem)
    {
        if (_index < _elems.Length)
        {
            _elems[_index++] = elem;
            return;
        }

        T[] newElems = ArrayPool<T>.Shared.Rent(_elems.Length * 2);
        Array.Copy(_elems, newElems, _elems.Length);
        ArrayPool<T>.Shared.Return(_elems);
        _elems = newElems;
        _elems[_index++] = elem;
    }

    public void Reset() => Reserve(INITIAL_CAPACITY);

    public void Reserve(int length)
    {
        // Array.Clear(_elems, 0, _index);

        _index = 0;
        ArrayPool<T>.Shared.Return(_elems);
        _elems = ArrayPool<T>.Shared.Rent(length);
    }

    public void SwapRemove(int index)
    {
        if (index < 0 || _index <= index)
            throw new ArgumentOutOfRangeException(nameof(index), $"Index {index} is out of bounds");

        _elems[index] = _elems[_index - 1];
        _index--;
    }

    public T RandPop()
    {
        int k = Random.Shared.Next(0, _index);
        T result = _elems[k];
        _elems[k] = _elems[_index - 1];
        _index--;
        return result;
    }

    public T Find<U>(Func<T, U, bool> func, U param) => _elems.Find(func, param, 0, _index);

    public int IndexOf<U>(Func<T, U, bool> func, U param) => _elems.IndexOf(func, param, 0, _index);

    public static CachedList<T> Create()
    {
        CachedList<T> list = ObjectPool<CachedList<T>>.Shared.Get();
        list._disposed = false;
        return list;
    }

    public void Dispose()
    {
        if (_disposed)
            throw new Exception("List has already been disposed");

        Reset();
        _disposed = true;
        ObjectPool<CachedList<T>>.Shared.Return(this);
    }
}
