using Arch.Core;
using Arch.System;
using Components.Basic;
using Engine.Animations;

namespace Systems.Animation;

partial class AnimSys : BaseSystem<World, float>
{
    private readonly SoundSys soundSys;

    public AnimSys(World world, SoundSys soundSys)
        : base(world)
    {
        this.soundSys = soundSys;
    }

    [Query]
    private void Execute([Data] in float dt, ref AnimComp animator, ref SpriteComp spriteComp)
    {
        Anim anim = animator.anim;
        if (anim == null || animator.keyIndex == anim.keys.Length)
            return;

        if (animator.time == default)
            ChangeKey(anim.keys[0], ref spriteComp);

        animator.time += dt * animator.timeScale;
        if (animator.time < anim.frameTime)
            return;

        animator.keyIndex++;
        if (animator.keyIndex == anim.keys.Length)
            if (anim.repeating)
                animator.keyIndex = 0;
            else
                return;

        animator.time -= anim.frameTime;
        ChangeKey(anim.keys[animator.keyIndex], ref spriteComp);
    }

    private void ChangeKey(Anim.Key key, ref SpriteComp spriteComp)
    {
        spriteComp.sprite = key.sprite;
        if (key.sound != null)
            soundSys.AddSound(key.sound.Value);
    }
}
