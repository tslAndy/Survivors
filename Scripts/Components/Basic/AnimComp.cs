using System.Numerics;
using Engine.Animations;
using Engine.Common;

namespace Components.Basic;

struct AnimComp
{
    public int keyIndex;
    public float time;

    public AnimAtlas atlas;
    public AnimDir animDir;

    private Anim _anim;
    public Anim anim
    {
        get => _anim;
        set
        {
            _anim = value;
            keyIndex = default;
            time = default;
        }
    }

    public Hash groupHash;

    public void SetAnimByGroup(Hash groupHash)
    {
        this.groupHash = groupHash;
        this.anim = atlas[groupHash, (int)animDir];
    }

    public void SetAnimByGroup(Hash groupHash, AnimDir animDir)
    {
        this.groupHash = groupHash;
        this.animDir = animDir;
        this.anim = atlas[groupHash, (int)animDir];
    }

    public bool isFinished => (!anim.repeating) && keyIndex == anim.keys.Length;
}

public enum AnimDir : byte
{
    Up,
    Down,
    Left,
    Right,
}

public static class AnimDirExtensions
{
    public static AnimDir AsAnimDir(this Vector2 vec)
    {
        Vector2 abs = Vector2.Abs(vec);
        if (abs.Y >= abs.X)
            return vec.Y <= 0.0f ? AnimDir.Up : AnimDir.Down;
        else if (abs.X > abs.Y)
            return vec.X <= 0.0f ? AnimDir.Left : AnimDir.Right;
        return AnimDir.Up;
    }
}
