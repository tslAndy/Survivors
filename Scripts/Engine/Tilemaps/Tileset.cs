using Engine.ResourceManagement;

namespace Engine.Tilemaps;

class Tileset : Atlas<Tile>
{
    public Tileset(Entry[] entries)
        : base(entries) { }
}
