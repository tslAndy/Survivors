using Engine.ResourceManagement;
using Raylib_cs;

namespace Engine.Sprites;

class TextureManager : ResourceManager<Texture2D>
{
    public TextureManager()
        : base(path => Raylib.LoadTexture(path), texture => Raylib.UnloadTexture(texture)) { }
}
