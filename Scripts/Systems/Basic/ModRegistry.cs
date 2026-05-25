using Engine.Common;

namespace Systems.Basic;

static class ModRegistry
{
    private static readonly List<string> _elems = new List<string>();

    public static (Hash, string) GetElement(int index) => (new Hash(index), _elems[index]);

    public static int GetCount() => _elems.Count;

    public static Hash CountHash(string name)
    {
        int ind = _elems.IndexOf(name);
        if (ind < 0)
        {
            ind = _elems.Count;
            _elems.Add(name);
        }
        return new Hash(ind);
    }
}
