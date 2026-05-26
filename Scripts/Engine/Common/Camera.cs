using System.Numerics;

namespace Engine.Common;

// TODO: cache aspect and inverted size
public class Camera
{
    public readonly int width,
        height;

    public Vector2 position;
    public float size;

    public float pixelsPerUnit => width / size;

    public Camera(int width, int height, Vector2 position, float size)
    {
        this.width = width;
        this.height = height;
        this.position = position;
        this.size = size;
    }

    public AABB frustum
    {
        get
        {
            Vector2 vecSize = new Vector2(size, size * height / width);
            return AABB.BySize(position, vecSize);
        }
    }
}
