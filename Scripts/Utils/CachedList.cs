using System.Buffers;

namespace Utils;

class CachedList<T> : IResettable
{
    private T[] _elems;
    private int _index;

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

    public T Find<U>(Func<T, U, bool> func, U param) => _elems.Find(func, param, 0, _index);

    public int IndexOf<U>(Func<T, U, bool> func, U param) => _elems.IndexOf(func, param, 0, _index);
}
