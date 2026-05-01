namespace Utils;

class CircularSet<T>
{
    private readonly T[] _elems;
    private int _index,
        _count;

    public CircularSet(int length)
    {
        _elems = new T[length];
        _index = 0;
        _count = 0;
    }

    public int Count => _count;

    public void Add(T elem)
    {
        int start = (_elems.Length + _index - _count) % _elems.Length;
        for (int i = 0; i < _count; i++)
        {
            int index = (start + i) % _elems.Length;
            if (EqualityComparer<T>.Default.Equals(_elems[index], elem))
                return;
        }

        if (_count < _elems.Length)
            _count++;

        _elems[_index] = elem;
        _index = (_index + 1) % _elems.Length;
    }

    public T Pop()
    {
        if (_count == 0)
            throw new Exception("Circular buffer is empty");

        _count--;
        _index = (_elems.Length + _index - 1) % _elems.Length;
        return _elems[_index];
    }
}
