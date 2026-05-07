namespace Utils;

ref struct RefSet<T>
    where T : unmanaged
{
    private int _index;
    private Span<T> _span;

    public RefSet(Span<T> span)
    {
        _index = 0;
        _span = span;
    }

    public bool TryAdd(T elem)
    {
        for (int i = 0; i < _index; i++)
        {
            if (EqualityComparer<T>.Default.Equals(_span[i], elem))
                return false;
        }

        _index = (_index + 1) % _span.Length;
        _span[_index] = elem;
        return true;
    }

    public void Reset() => _index = 0;
}
