using System.Numerics;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Physics;
using Components.Player;
using Engine.Animations;
using Engine.Common;
using Engine.Input;
using Systems.Physics;
using Utils;

namespace Systems.Player;

partial class PlayerSys : BaseSystem<World, float>
{
    private readonly TileCollSys _tileCollSys;

    private const float CORR_RATE = 0.4f;

    private readonly Hash IdleGroupHash = AnimAtlas.CountHash("Idle");
    private readonly Hash WalkGroupHash = AnimAtlas.CountHash("Walk");
    private readonly Hash RunGroupHash = AnimAtlas.CountHash("Run");

    public PlayerSys(World world)
        : base(world)
    {
        _tileCollSys = ServiceLocator.Get<TileCollSys>();
    }

    [Query]
    private void UpdateCollisions(
        ref PlayerComp player,
        ref TransformComp trs,
        in CollComp coll,
        ref RigidComp rigid
    )
    {
        CachedList<TileColl> tileColls = ObjectPool<CachedList<TileColl>>.Shared.Get();
        _tileCollSys.GetOverlap(trs.position, trs.scale * coll.radius, default, tileColls);

        if (tileColls.Count == 0)
        {
            ObjectPool<CachedList<TileColl>>.Shared.Return(tileColls);
            return;
        }

        Vector2 corr = Vector2.Zero;
        for (int i = 0; i < tileColls.Count; i++)
        {
            ref TileColl tileColl = ref tileColls[i];
            corr += tileColl.normal * tileColl.depth;
        }
        corr /= tileColls.Count;

        rigid.velocity = Vector2.Zero;
        trs.position += corr * CORR_RATE;

        ObjectPool<CachedList<TileColl>>.Shared.Return(tileColls);
    }

    [Query]
    private void UpdatePlayer(ref PlayerComp player, ref AnimComp animator, ref RigidComp rigid)
    {
        Vector2 input = InputHandler.GetInput();
        bool modifier = InputHandler.IsModifierPressed();

        switch (player.state)
        {
            case PlayerState.Idle:
                Idle(input, modifier, ref player, ref animator, ref rigid);
                return;

            case PlayerState.Walk:
                Walk(input, modifier, ref player, ref animator, ref rigid);
                return;

            case PlayerState.Run:
                Run(input, modifier, ref player, ref animator, ref rigid);
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
        ref RigidComp rigid
    )
    {
        rigid.velocity = Vector2.Zero;
        if (input.LengthSquared() < 0.001f)
            return;

        animator.animDir = input.AsAnimDir();
        if (modifier)
        {
            animator.SetAnim(RunGroupHash, animator.animDir);
            // animator.anim = animator.atlas[RunGroupHash, (int)animator.animDir];
            player.state = PlayerState.Run;
        }
        else
        {
            animator.SetAnim(WalkGroupHash, animator.animDir);
            // animator.anim = animator.atlas[WalkGroupHash, (int)animator.animDir];
            player.state = PlayerState.Walk;
        }
    }

    private void Walk(
        Vector2 input,
        bool modifier,
        ref PlayerComp player,
        ref AnimComp animator,
        ref RigidComp rigid
    )
    {
        if (input.LengthSquared() < 0.001f)
        {
            animator.SetAnim(IdleGroupHash, animator.animDir);
            // animator.anim = animator.atlas[IdleGroupHash, (int)animator.animDir];
            player.state = PlayerState.Idle;
            return;
        }

        if (modifier)
        {
            animator.SetAnim(RunGroupHash, animator.animDir);
            // animator.anim = animator.atlas[RunGroupHash, (int)animator.animDir];
            player.state = PlayerState.Run;
            return;
        }

        rigid.velocity = input * player.walkSpeed;

        AnimDir animDir = input.AsAnimDir();
        if (animDir != animator.animDir)
        {
            animator.SetAnim(WalkGroupHash, animDir);
            // animator.animDir = animDir;
            // animator.anim = animator.atlas[WalkGroupHash, (int)animator.animDir];
        }
    }

    private void Run(
        Vector2 input,
        bool modifier,
        ref PlayerComp player,
        ref AnimComp animator,
        ref RigidComp rigid
    )
    {
        if (input.LengthSquared() < 0.001f)
        {
            animator.SetAnim(IdleGroupHash, animator.animDir);
            // animator.anim = animator.atlas[IdleGroupHash, (int)animator.animDir];
            player.state = PlayerState.Idle;
            return;
        }

        if (!modifier)
        {
            animator.SetAnim(WalkGroupHash, animator.animDir);
            // animator.anim = animator.atlas[WalkGroupHash, (int)animator.animDir];
            player.state = PlayerState.Walk;
            return;
        }

        rigid.velocity = input * player.runSpeed;

        AnimDir animDir = input.AsAnimDir();
        if (animDir != animator.animDir)
        {
            animator.SetAnim(RunGroupHash, animDir);
            // animator.animDir = animDir;
            // animator.anim = animator.atlas[RunGroupHash, (int)animator.animDir];
        }
    }
}
