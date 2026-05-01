namespace Engine.Common;

struct Hash
{
    public int val;

    public Hash(int val) => this.val = val;

    public static bool operator ==(Hash a, Hash b) => a.val == b.val;

    public static bool operator !=(Hash a, Hash b) => a.val != b.val;

    public override int GetHashCode() => HashCode.Combine(val);

    public override bool Equals(object? obj) => obj is Hash hash && val == hash.val;
}
