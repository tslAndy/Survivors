using Engine.Common;
using Utils;

namespace Engine.ResourceManagement;

class Atlas<T>
{
    private readonly Entry[] _entries;
    private readonly Group[]? _groups;

    private static readonly Dictionary<string, Hash> _hashes = new Dictionary<string, Hash>();

    public Atlas(Entry[] entries, Group[]? subgroups = null)
    {
        _entries = entries;
        _groups = subgroups;
    }

    public T this[int entryIndex] => _entries[entryIndex].val;
    public T this[Hash entryHash] =>
        _entries.Find((entry, hash) => entry.hash == hash, entryHash).val;
    public T this[string entryName] => this[CountHash(entryName)];

    public T this[string groupName, int indexInGroup] => this[CountHash(groupName), indexInGroup];
    public T this[Hash groupHash, int indexInGroup]
    {
        get
        {
            if (_groups is null)
                throw new Exception("Subgroups are not initialized");

            int index = _groups.IndexOf((group, hash) => group.hash == hash, groupHash);
            if (index < 0)
                throw new ArgumentException("Wrong group hash", nameof(groupHash));

            Group group = _groups[index];
            if (indexInGroup >= group.end - group.start)
                throw new ArgumentOutOfRangeException(nameof(indexInGroup), "Index is wrong");

            return _entries[group.start + indexInGroup].val;
        }
    }

    public int GetGroupLength(string groupName) => GetGroupLength(CountHash(groupName));

    public int GetGroupLength(Hash groupHash)
    {
        if (_groups is null)
            throw new Exception("Subgroups are not initialized");

        int index = _groups.IndexOf((group, hash) => group.hash == hash, groupHash);
        if (index < 0)
            throw new ArgumentException("Wrong group hash", nameof(groupHash));

        ref Group group = ref _groups[index];
        return group.end - group.start;
    }

    // can be used both for animations and groups
    public static Hash CountHash(string name)
    {
        if (_hashes.TryGetValue(name, out Hash val))
            return val;

        Hash hash = new Hash(_hashes.Count);
        _hashes[name] = hash;
        return hash;
    }

    public struct Entry
    {
        public T val;
        public Hash hash;

        public Entry(T val, string name)
        {
            this.val = val;
            this.hash = CountHash(name);
        }
    }

    public struct Group
    {
        public Hash hash;
        public int start,
            end;

        public Group(string name, int start, int end)
        {
            this.hash = CountHash(name);
            this.start = start;
            this.end = end;
        }
    }
}
