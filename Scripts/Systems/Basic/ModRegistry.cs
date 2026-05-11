using Engine.Common;
using Utils;

namespace Systems.Basic;

class ModRegistry
{
    private readonly Elem[] _elems;

    public ModRegistry(params string[] names)
    {
        _elems = new Elem[names.Length];
        for (int i = 0; i < names.Length; i++)
            _elems[i] = new Elem { name = names[i], hash = new Hash(i) };
    }

    public Hash this[string name]
    {
        get
        {
            int index = _elems.IndexOf((x, xname) => x.name == xname, name);
            if (index < 0)
                throw new ArgumentException("Wrong modifier name", nameof(name));
            return _elems[index].hash;
        }
    }

    private struct Elem
    {
        public string name;
        public Hash hash;
    }
}
