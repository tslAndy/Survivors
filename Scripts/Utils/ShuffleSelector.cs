namespace Utils;

public class ShuffleSelector<T>
{
    private readonly T[] _elems;
    private int _count;

    public ShuffleSelector(params T[] elems)
    {
        _elems = elems;
        _count = _elems.Length;
    }

    public int Count => _count;
    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
                throw new Exception();
            return _elems[index];
        }
        set
        {
            if (index < 0 || index >= _count)
                throw new Exception();
            _elems[index] = value;
        }
    }

    public void Exclude(int index)
    {
        if (index < 0 || index >= _count)
            throw new Exception();

        _elems[index] = _elems[_count - 1];
        _count--;
        Random.Shared.Shuffle(_elems.AsSpan(0, _count));
    }

    public void Reset() => _count = _elems.Length;
}
