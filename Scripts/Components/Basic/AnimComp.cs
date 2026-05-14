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

    public Hash groupHash { get; private set; }

    public void SetAnim(Hash groupHash)
    {
        this.groupHash = groupHash;
        this.anim = atlas[groupHash, (int)animDir];
    }

    public void SetAnim(Hash groupHash, AnimDir animDir)
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
        if (MathF.Abs(vec.Y) > 0.001f)
            return vec.Y < 0 ? AnimDir.Up : AnimDir.Down;
        if (MathF.Abs(vec.X) > 0.001f)
            return vec.X < 0 ? AnimDir.Left : AnimDir.Right;
        return AnimDir.Up;
    }
}
