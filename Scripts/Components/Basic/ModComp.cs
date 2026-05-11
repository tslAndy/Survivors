using Engine.Common;

namespace Components.Basic;

[System.Runtime.CompilerServices.InlineArray(32)]
struct ModComp
{
    private float num;

    public ModComp()
    {
        for (int i = 0; i < 32; i++)
            this[i] = 1.0f;
    }

    public float this[Hash hash]
    {
        get => this[hash.val];
        set => this[hash.val] = value;
    }
}
