namespace Engine.Common;

struct Vector2Int
{
    public int x,
        y;

    public Vector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Vector2Int operator +(Vector2Int operand) => operand;

    public static Vector2Int operator -(Vector2Int operand) =>
        new Vector2Int(-operand.x, -operand.y);

    public static Vector2Int operator +(Vector2Int left, Vector2Int right) =>
        new Vector2Int(left.x + right.x, left.y + right.y);

    public static Vector2Int operator -(Vector2Int left, Vector2Int right) =>
        new Vector2Int(left.x - right.x, left.y - right.y);

    public static Vector2Int operator *(Vector2Int operand, int num) =>
        new Vector2Int(operand.x * num, operand.y * num);

    public static Vector2Int operator /(Vector2Int operand, int num) =>
        new Vector2Int(operand.x / num, operand.y / num);
}
