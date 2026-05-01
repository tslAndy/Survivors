using Raylib_cs;

namespace Engine.Tilemaps;

class Tile
{
    public readonly Texture2D texture;
    public readonly Rectangle rect;

    public Tile(Texture2D texture, Rectangle rect)
    {
        this.texture = texture;
        this.rect = rect;
    }
}
