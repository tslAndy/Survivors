using Arch.Buffer;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.Relationships;
using Arch.System;
using Components.Basic;

namespace Systems.Basic;

partial class TrsOwningSys : BaseSystem<World, float>
{
    private readonly CommandBuffer _commandBuffer;

    public TrsOwningSys(World world, CommandBuffer commandBuffer)
        : base(world)
    {
        _commandBuffer = commandBuffer;
    }

    [Query]
    [None(typeof(DeathComp))]
    private void UpdateDuplicate(Entity entity, in TransformComp trs)
    {
        if (!entity.HasRelationship<TrsOwn>())
            return;

        foreach (KeyValuePair<Entity, TrsOwn> kvp in entity.GetRelationships<TrsOwn>())
        {
            ref TransformComp subTrs = ref kvp.Key.Get<TransformComp>();
            subTrs.position = trs.position;
            subTrs.rotation = trs.rotation;
            subTrs.scale = trs.scale;
        }
    }

    [Query]
    private void HandleDeath(Entity entity, in TransformComp trs, in DeathComp death)
    {
        if (!death.isDead)
            return;

        if (!entity.HasRelationship<TrsOwn>())
            return;

        foreach (KeyValuePair<Entity, TrsOwn> kvp in entity.GetRelationships<TrsOwn>())
        {
            World.RemoveRelationship<TrsOwn>(entity, kvp.Key);
            _commandBuffer.Destroy(kvp.Key);
        }
    }
}
