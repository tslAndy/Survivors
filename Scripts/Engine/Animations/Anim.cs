using Engine.Sprites;
using Raylib_cs;

namespace Engine.Animations;

class Anim
{
    public readonly Key[] keys;
    public readonly float frameTime;
    public readonly bool repeating;

    public float duration => keys.Length * frameTime;

    public Anim(Key[] keys, float frameTime, bool repeating)
    {
        this.keys = keys;
        this.frameTime = frameTime;
        this.repeating = repeating;
    }

    public readonly struct Key
    {
        public readonly Sprite sprite;
        public readonly Sound? sound;

        public Key(Sprite sprite, Sound? sound)
        {
            this.sprite = sprite;
            this.sound = sound;
        }
    }
}
