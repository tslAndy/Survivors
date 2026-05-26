using Raylib_cs;

namespace Engine.Sprites;

public class Sprite
{
    public readonly Texture2D texture;
    public readonly Rectangle rect;

    public Sprite(Texture2D texture, Rectangle rect)
    {
        this.texture = texture;
        this.rect = rect;
    }
}
