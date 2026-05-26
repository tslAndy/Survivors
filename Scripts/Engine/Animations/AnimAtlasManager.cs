using Engine.ResourceManagement;
using Engine.Sounds;
using Engine.Sprites;
using Raylib_cs;

namespace Engine.Animations;

public class AnimAtlasManager : ResourceManager<AnimAtlas>
{
    private readonly SpriteAtlasManager _spriteAtlasManager;
    private readonly SoundAtlasManager _soundAtlasManager;

    public AnimAtlasManager(
        SpriteAtlasManager spriteAtlasManager,
        SoundAtlasManager soundAtlasManager
    )
    {
        _spriteAtlasManager = spriteAtlasManager;
        _soundAtlasManager = soundAtlasManager;
    }

    protected override AnimAtlas Load(string path)
    {
        SpriteAtlas? spriteAtlas = null;
        SoundAtlas? soundAtlas = null;

        List<AnimAtlas.Entry> entries = new List<AnimAtlas.Entry>();
        List<AnimAtlas.Group> groups = new List<AnimAtlas.Group>();

        List<Anim.Key> animKeys = new List<Anim.Key>();
        string? animName = null;
        float frametime = 0;
        bool repeating = false;

        string? groupName = null;
        int groupStart = 0;
        int groupEnd = 0;

        foreach (string line in File.ReadLines(path))
        {
            string[] tokens = line.Split(' ');
            if (tokens.Length == 0)
                continue;

            if (tokens[0] == "spriteAtlas")
            {
                if (spriteAtlas != null)
                    throw new Exception($"Sprite atlas already exists. Parsing {path}");

                string spriteAtlasName = tokens[1];
                spriteAtlas = _spriteAtlasManager.Get(spriteAtlasName);
            }
            else if (tokens[0] == "soundAtlas")
            {
                if (soundAtlas != null)
                    throw new Exception($"Sound atlas already exists. Parsing {path}");

                string soundAtlasName = tokens[1];
                soundAtlas = _soundAtlasManager.Get(soundAtlasName);
            }
            else if (tokens[0] == "group")
            {
                if (groupName != null)
                    throw new Exception($"Group already exists. Parsing {path}");

                groupName = tokens[1];
                groupStart = entries.Count;
            }
            else if (tokens[0] == "endgroup")
            {
                if (groupName == null)
                    throw new Exception($"Subgroup doesn't exist. Parsing {path}");

                groupEnd = entries.Count;
                groups.Add(new AnimAtlas.Group(groupName, groupStart, groupEnd));

                groupName = null;
                groupStart = 0;
                groupEnd = 0;
            }
            else if (tokens[0] == "anim")
            {
                animName = tokens[1];
            }
            else if (tokens[0] == "frametime")
            {
                frametime = int.Parse(tokens[1]) * 0.001f;
            }
            else if (tokens[0] == "repeat")
            {
                repeating = bool.Parse(tokens[1]);
            }
            else if (tokens[0] == "key")
            {
                if (animName == null)
                    throw new Exception($"Anim doesn't exists. Parsing {path}");

                if (spriteAtlas == null)
                    throw new Exception($"Sprite Atlas doesn't exists. Parsing {path}");

                string spriteName = tokens[1];
                Sprite sprite = spriteAtlas[spriteName];

                Sound? sound = null;
                if (tokens.Length == 3)
                {
                    if (soundAtlas == null)
                        throw new Exception($"Sound Atlas doesn't exists. Parsing {path}");

                    string soundName = tokens[2];
                    sound = soundAtlas[soundName];
                }

                animKeys.Add(new Anim.Key(sprite, sound));
            }
            else if (tokens[0] == "endkey")
            {
                if (animName == null)
                    throw new Exception($"Anim doesn't exists. Parsing {path}");

                Anim anim = new Anim(animKeys.ToArray(), frametime, repeating);
                entries.Add(new AnimAtlas.Entry(anim, animName));

                animKeys.Clear();
                animName = null;
                frametime = 0;
                repeating = false;
            }
        }

        if (animKeys.Count != 0)
            throw new Exception($"Anim's endkey wasn't found. Parsing {path}");

        return new AnimAtlas(entries.ToArray(), groups.Count == 0 ? null : groups.ToArray());
    }
}


/*
 *
 * spriteAtlas bull.spriteAtlas
 * soundAtlas bull.soundAtlas
 *
 * anim Walk
 * frametime 10
 * repeat false
 *
 *     sprite     sound
 * key walk_up_1 walk
 * key walk_up_2
 *
 * endkey
 * */
