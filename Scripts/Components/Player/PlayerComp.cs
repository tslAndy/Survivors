namespace Components.Player;

struct PlayerComp
{
    public PlayerState state;
    public float walkSpeed,
        runSpeed;
}

enum PlayerState
{
    Idle,
    Walk,
    Run,
    Die,
}
