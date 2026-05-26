using Engine.ResourceManagement;
using Raylib_cs;

namespace Engine.Sounds;

public class SoundAtlasManager : ResourceManager<SoundAtlas>
{
    private readonly SoundManager _soundManager;

    public SoundAtlasManager(SoundManager soundManager)
    {
        _soundManager = soundManager;
    }

    protected override SoundAtlas Load(string path)
    {
        List<SoundAtlas.Entry> entries = new List<SoundAtlas.Entry>();
        foreach (string line in File.ReadLines(path))
        {
            string[] tokens = line.Split(' ');
            if (tokens.Length != 3)
                continue;

            if (tokens[0] == "key")
            {
                string soundName = tokens[1];
                string soundPath = tokens[2];

                Sound sound = _soundManager.Get(soundPath);
                entries.Add(new SoundAtlas.Entry(sound, soundName));
            }
        }

        return new SoundAtlas(entries.ToArray());
    }
}


/*
 *
 * key soundName soundPath
 * key soundName soundPath
 * key soundName soundPath
 * key soundName soundPath
 *
 *
 * key walk ./Sounds/Enemies/bullWalk.wav
 * key run ./Sounds/Enemies/bullRun.wav
 *
 * */
