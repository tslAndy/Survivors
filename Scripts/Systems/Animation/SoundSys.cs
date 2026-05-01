using Arch.Core;
using Arch.System;
using Raylib_cs;
using Utils;

namespace Systems.Animation;

class SoundSys : BaseSystem<World, float>
{
    private readonly List<Sound> playing;
    private readonly CircularSet<Sound> queue;

    private const int MAX_SOUNDS = 5;

    public SoundSys(World world)
        : base(world)
    {
        playing = new List<Sound>(MAX_SOUNDS);
        queue = new CircularSet<Sound>(MAX_SOUNDS);
    }

    public override void Update(in float dt)
    {
        int i = 0;
        while (i < playing.Count)
        {
            Sound sound = playing[i];
            if (Raylib.IsSoundPlaying(sound))
            {
                i++;
                continue;
            }

            Raylib.UnloadSoundAlias(sound);
            playing.SwapRemove(i);
        }

        int count = Math.Min(MAX_SOUNDS - playing.Count, queue.Count);
        for (i = 0; i < count; i++)
        {
            Sound sound = Raylib.LoadSoundAlias(queue.Pop());
            Raylib.PlaySound(sound);
            playing.Add(sound);
        }
    }

    public void AddSound(Sound sound) => queue.Add(sound);
}
