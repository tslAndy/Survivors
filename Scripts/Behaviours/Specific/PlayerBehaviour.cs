using System.Numerics;
using Components.Basic;
using Components.Behaviour;
using Engine.Animations;
using Engine.Common;
using Engine.Input;
using Systems;
using Systems.Basic;
using Systems.Physics;
using Utils;

namespace Behaviours.Specific;

class PlayerBehaviour : BaseBehaviour
{
    private readonly InputHandler _inputHandler;
    private readonly int _wallsLayer;

    private readonly Hash IdleGroupHash = AnimAtlas.CountHash("Idle");
    private readonly Hash WalkGroupHash = AnimAtlas.CountHash("Walk");
    private readonly Hash MoveFactorHash = ModRegistry.CountHash("moveFactor");

    private const float CORR_RATE = 0.4f;

    public PlayerBehaviour(WorldContext context, InputHandler inputHandler)
        : base(context)
    {
        _inputHandler = inputHandler;
        _wallsLayer = context.layerMap["Walls"];
    }

    public override void Update(ref EntityContext entityCtx)
    {
        SolveCollisions(ref entityCtx);

        switch (entityCtx.behaviour.state)
        {
            case (int)PlayerState.Idle:
                HandleIdle(ref entityCtx);
                break;

            case (int)PlayerState.Walk:
                HandleWalk(ref entityCtx);
                break;

            default:
                throw new Exception("Uknown state");
        }
    }

    private void SolveCollisions(ref EntityContext entityCtx)
    {
        using CachedList<TileColl> tileColls = CachedList<TileColl>.Create();
        context.tileSys.GetOverlap(
            entityCtx.trs.position,
            entityCtx.trs.scale * entityCtx.coll.radius,
            _wallsLayer,
            tileColls
        );

        if (tileColls.Count == 0)
            return;

        Vector2 corr = Vector2.Zero;
        for (int i = 0; i < tileColls.Count; i++)
        {
            ref TileColl tileColl = ref tileColls[i];
            corr += tileColl.normal * tileColl.depth;
        }
        corr /= tileColls.Count;

        entityCtx.rigid.velocity = Vector2.Zero;
        entityCtx.trs.position += corr * CORR_RATE;
    }

    private void HandleIdle(ref EntityContext entityCtx)
    {
        Vector2 input = _inputHandler.GetInput();

        entityCtx.rigid.velocity = Vector2.Zero;
        if (input.LengthSquared() < 0.001f)
            return;

        entityCtx.animator.SetAnim(WalkGroupHash, input.AsAnimDir());
        entityCtx.behaviour.state = (int)PlayerState.Walk;
    }

    private void HandleWalk(ref EntityContext entityCtx)
    {
        Vector2 input = _inputHandler.GetInput();

        if (input.LengthSquared() < 0.001f)
        {
            entityCtx.animator.SetAnim(IdleGroupHash);
            entityCtx.behaviour.state = (int)PlayerState.Idle;
            return;
        }

        entityCtx.rigid.velocity = entityCtx.move.maxSpeed * entityCtx.mod[MoveFactorHash] * input;

        AnimDir animDir = input.AsAnimDir();
        if (animDir != entityCtx.animator.animDir)
            entityCtx.animator.SetAnim(WalkGroupHash, animDir);
    }

    private enum PlayerState
    {
        Idle,
        Walk,
    }
}
