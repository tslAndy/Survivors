using Engine.ResourceManagement;

namespace Engine.Tilemaps;

public class Tileset : Atlas<Tile>
{
    public Tileset(Entry[] entries)
        : base(entries) { }
}
