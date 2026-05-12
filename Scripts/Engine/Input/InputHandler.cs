using System.Numerics;
using Raylib_cs;

namespace Engine.Input;

class InputHandler
{
    public Vector2 GetInput()
    {
        float x = 0.0f;
        if (Raylib.IsKeyDown(KeyboardKey.A))
            x = -1.0f;
        else if (Raylib.IsKeyDown(KeyboardKey.D))
            x = 1.0f;

        float y = 0.0f;
        if (Raylib.IsKeyDown(KeyboardKey.W))
            y = -1.0f;
        else if (Raylib.IsKeyDown(KeyboardKey.S))
            y = 1.0f;

        Vector2 dir = new Vector2(x, y);
        dir /= dir.Length() + 0.001f;
        return dir;
    }
}
