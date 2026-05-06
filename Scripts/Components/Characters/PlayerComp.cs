namespace Components.Characters;

struct PlayerComp
{
    public PlayerState state;
}

enum PlayerState
{
    Idle,
    Walk,
    Run,
    Die,
}
