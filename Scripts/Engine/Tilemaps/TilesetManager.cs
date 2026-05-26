using Engine.ResourceManagement;
using Engine.Sprites;
using Raylib_cs;

namespace Engine.Tilemaps;

public class TilesetManager : ResourceManager<Tileset>
{
    private TextureManager _textureManager;

    public TilesetManager(TextureManager textureManager)
    {
        _textureManager = textureManager;
    }

    protected override Tileset Load(string path)
    {
        Texture2D? texture = null;
        int? tileSize = null;

        List<Tileset.Entry> entries = new List<Tileset.Entry>();

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
                texture = _textureManager.Get(texturePath);
            }
            else if (tokens[0] == "tilesize")
            {
                if (tileSize != null)
                    throw new Exception($"Tile size is already defined. Parsing {path}");
                tileSize = int.Parse(tokens[1]);
            }
            else if (tokens[0] == "key")
            {
                if (texture == null || tileSize == null)
                    throw new Exception($"Texture or tile size is not defined. Parsing {path}");

                int x = int.Parse(tokens[1]);
                int y = int.Parse(tokens[2]);
                Tile tile = new Tile(
                    texture.Value,
                    new Rectangle(x, y, tileSize.Value, tileSize.Value)
                );
                entries.Add(new Tileset.Entry(tile, ""));
            }
        }

        return new Tileset(entries.ToArray());
    }
}

/*
 * desert.tileAtlas
 *
 * texture Textures/DesertTiles.png
 * tileSize 16
 *
 *     x  y
 * key 64 64
 * */
