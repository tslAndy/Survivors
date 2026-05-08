using System.Numerics;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Characters;
using Components.Physics;
using Engine.Animations;
using Engine.Common;
using Engine.Input;
using Systems.Physics;
using Utils;

namespace Systems.Characters;

partial class PlayerSys : BaseSystem<World, float>
{
    private readonly InputHandler _inputHandler;
    private readonly WorldContext _context;

    private const float CORR_RATE = 0.4f;

    private readonly Hash IdleGroupHash = AnimAtlas.CountHash("Idle");
    private readonly Hash WalkGroupHash = AnimAtlas.CountHash("Walk");
    private readonly Hash RunGroupHash = AnimAtlas.CountHash("Run");

    private readonly int _wallsLayer;

    public PlayerSys(World world, InputHandler inputHandler, WorldContext context)
        : base(world)
    {
        _inputHandler = inputHandler;
        _context = context;
        _wallsLayer = context.layerMap["Walls"];
    }

    [Query]
    private void UpdateCollisions(
        ref PlayerComp player,
        ref TrsComp trs,
        in CollComp coll,
        ref RigidComp rigid
    )
    {
        using CachedList<TileColl> tileColls = CachedList<TileColl>.Create();
        _context.tileCollSys.GetOverlap(
            trs.position,
            trs.scale * coll.radius,
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

        rigid.velocity = Vector2.Zero;
        trs.position += corr * CORR_RATE;
    }

    [Query]
    private void UpdatePlayer(
        ref PlayerComp player,
        ref AnimComp animator,
        ref RigidComp rigid,
        ref MoveComp moveComp
    )
    {
        Vector2 input = _inputHandler.GetInput();
        bool modifier = _inputHandler.IsModifierPressed();

        switch (player.state)
        {
            case PlayerState.Idle:
                Idle(input, modifier, ref player, ref animator, ref rigid, ref moveComp);
                return;

            case PlayerState.Walk:
                Walk(input, modifier, ref player, ref animator, ref rigid, ref moveComp);
                return;

            case PlayerState.Run:
                Run(input, modifier, ref player, ref animator, ref rigid, ref moveComp);
                return;

            case PlayerState.Die:
                return;
        }
    }

    private void Idle(
        Vector2 input,
        bool modifier,
        ref PlayerComp player,
        ref AnimComp animator,
        ref RigidComp rigid,
        ref MoveComp moveComp
    )
    {
        rigid.velocity = Vector2.Zero;
        if (input.LengthSquared() < 0.001f)
            return;

        animator.animDir = input.AsAnimDir();
        if (modifier)
        {
            animator.SetAnim(RunGroupHash, animator.animDir);
            player.state = PlayerState.Run;
        }
        else
        {
            animator.SetAnim(WalkGroupHash, animator.animDir);
            player.state = PlayerState.Walk;
        }
    }

    private void Walk(
        Vector2 input,
        bool modifier,
        ref PlayerComp player,
        ref AnimComp animator,
        ref RigidComp rigid,
        ref MoveComp moveComp
    )
    {
        if (input.LengthSquared() < 0.001f)
        {
            animator.SetAnim(IdleGroupHash, animator.animDir);
            player.state = PlayerState.Idle;
            return;
        }

        if (modifier)
        {
            animator.SetAnim(RunGroupHash, animator.animDir);
            player.state = PlayerState.Run;
            return;
        }

        rigid.velocity = 0.75f * moveComp.maxSpeed * moveComp.speedFactor * input;

        AnimDir animDir = input.AsAnimDir();
        if (animDir != animator.animDir)
            animator.SetAnim(WalkGroupHash, animDir);
    }

    private void Run(
        Vector2 input,
        bool modifier,
        ref PlayerComp player,
        ref AnimComp animator,
        ref RigidComp rigid,
        ref MoveComp moveComp
    )
    {
        if (input.LengthSquared() < 0.001f)
        {
            animator.SetAnim(IdleGroupHash, animator.animDir);
            player.state = PlayerState.Idle;
            return;
        }

        if (!modifier)
        {
            animator.SetAnim(WalkGroupHash, animator.animDir);
            player.state = PlayerState.Walk;
            return;
        }

        rigid.velocity = moveComp.maxSpeed * moveComp.speedFactor * input;

        AnimDir animDir = input.AsAnimDir();
        if (animDir != animator.animDir)
            animator.SetAnim(RunGroupHash, animDir);
    }
}
