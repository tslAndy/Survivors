using Engine.ResourceManagement;
using Raylib_cs;

namespace Engine.Sprites;

class TextureManager : ResourceManager<Texture2D>
{
    protected override Texture2D Load(string path) => Raylib.LoadTexture(path);

    protected override void Unload(Texture2D resource) => Raylib.UnloadTexture(resource);
}
