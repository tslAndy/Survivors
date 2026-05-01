using Engine.ResourceManagement;
using Raylib_cs;
using Utils;

namespace Engine.Sprites;

class SpriteAtlasManager : ResourceManager<SpriteAtlas>
{
    public SpriteAtlasManager()
        : base(Load, null) { }

    private static SpriteAtlas Load(string path)
    {
        Texture2D? texture = null;

        string? groupName = null;
        int groupStart = 0;
        int groupEnd = 0;

        List<SpriteAtlas.Entry> entries = new List<SpriteAtlas.Entry>();
        List<SpriteAtlas.Group> groups = new List<SpriteAtlas.Group>();

        foreach (string line in File.ReadLines(path))
        {
            if (line.Length == 0)
                continue;

            string[] tokens = line.Split(' ');
            if (tokens[0] == "texture")
            {
                if (texture != null)
                    throw new Exception($"Texture already exists. Parsing {path}");

                string texturePath = tokens[1];
                texture = ServiceLocator.Get<TextureManager>().Get(texturePath);
            }
            else if (tokens[0] == "key")
            {
                if (texture == null)
                    throw new Exception($"Texture is null. Parsing {path}");

                string spriteName = tokens[1];
                int x = int.Parse(tokens[2]);
                int y = int.Parse(tokens[3]);
                int w = int.Parse(tokens[4]);
                int h = int.Parse(tokens[5]);

                Sprite sprite = new Sprite(texture.Value, new Rectangle(x, y, w, h));
                entries.Add(new SpriteAtlas.Entry(sprite, spriteName));
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
                groups.Add(new SpriteAtlas.Group(groupName, groupStart, groupEnd));

                groupName = null;
                groupStart = 0;
                groupEnd = 0;
            }
        }

        return new SpriteAtlas(entries.ToArray(), groups.Count == 0 ? null : groups.ToArray());
    }
}

/*
 *
 * texture bull.png
 *
 * key walk_1 0 0 16 16
 * key walk_2 0 16 16 16
 *
 * */
