using Arch.Bus;
using Arch.Core;
using Arch.System;
using Events;
using Raylib_cs;
using Utils;

namespace Systems.Animation;

partial class SoundSys : BaseSystem<World, float>
{
    private readonly CachedList<Sound> playing;
    private readonly CircularSet<Sound> queue;

    // background music
    private Music? _music;
    public Music? music
    {
        get => _music;
        set
        {
            if (_music != null)
                Raylib.PauseMusicStream(_music.Value);

            _music = value;

            if (_music != null)
                Raylib.PlayMusicStream(_music.Value);
        }
    }

    private const int MAX_SOUNDS = 5;

    public SoundSys(World world)
        : base(world)
    {
        playing = CachedList<Sound>.Create();
        queue = new CircularSet<Sound>(MAX_SOUNDS);
    }

    public override void Update(in float dt)
    {
        if (_music != null)
            Raylib.UpdateMusicStream(_music.Value);

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

    [Event]
    public void OnPaused(ref PausedEvent @event)
    {
        if (@event.isPaused)
        {
            if (_music != null)
                Raylib.PauseMusicStream(_music.Value);

            for (int i = 0; i < playing.Count; i++)
                Raylib.PauseSound(playing[i]);
        }
        else
        {
            if (_music != null)
                Raylib.ResumeMusicStream(_music.Value);

            for (int i = 0; i < playing.Count; i++)
                Raylib.ResumeSound(playing[i]);
        }
    }
}
