using Engine.ResourceManagement;

namespace Engine.Tilemaps;

public class TilemapManager : ResourceManager<Tilemap>
{
    private readonly TilesetManager _tilesetManager;

    public TilemapManager(TilesetManager tilesetManager)
    {
        _tilesetManager = tilesetManager;
    }

    protected override Tilemap Load(string path)
    {
        Tilemap tilemap = new Tilemap();
        Tileset? tileset = null;

        int y = 0;

        foreach (string line in File.ReadLines(path))
        {
            if (line.Length == 0)
                continue;

            string[] tokens = line.Split(' ');
            if (tokens[0] == "tileatlas")
            {
                if (tileset != null)
                    throw new Exception($"Tileset already exists. Parsing {path}");
                string tilesetPath = tokens[1];
                tileset = _tilesetManager.Get(tilesetPath);
            }
            else if (tokens[0] == "key")
            {
                if (tileset == null)
                    throw new Exception($"Tileset is not defined. Parsing {path}");

                for (int x = 1; x < tokens.Length; x++)
                {
                    int id = int.Parse(tokens[x]) - 1;
                    if (id >= 0)
                        tilemap[x - 1, y] = tileset[id];
                }

                y++;
            }
        }

        return tilemap;
    }
}

/*
 * level.tilemap
 *
 * tileatlas desert.tileAtlas
 *
 * key id id id id
 * key id id id id
 * key id id id id
 * key id id id id
 *
 *
 * */
