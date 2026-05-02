using System.Numerics;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Engine.Common;
using Raylib_cs;
using Utils;

namespace Systems.Drawing;

partial class SpriteDrawSys : BaseSystem<World, float>
{
    private const int MAX_LAYERS = 64;

    private readonly Camera _cam;
    private readonly CachedList<LayerItem>[] _layers;
    private readonly IComparer<LayerItem> _comparer;

    public SpriteDrawSys(World world, Camera cam)
        : base(world)
    {
        _cam = cam;
        _layers = new CachedList<LayerItem>[MAX_LAYERS];
        _comparer = Comparer<LayerItem>.Create((a, b) => a.y.CompareTo(b.y));

        for (int i = 0; i < _layers.Length; i++)
            _layers[i] = CachedList<LayerItem>.Create();
    }

    public override void Update(in float t)
    {
        FillLayersQuery(World);
        SortLayers();
        DrawLayers();
    }

    [Query]
    private void FillLayers(Entity entity, in SpriteComp spriteComp, in TransformComp trsComp)
    {
        Vector2 size = 0.0625f * trsComp.scale * spriteComp.sprite.rect.Size;
        Vector2 position = trsComp.position;

        AABB box = AABB.BySize(position, size);
        if (!AABB.CheckOverlap(box, _cam.frustum))
            return;

        CachedList<LayerItem> layer = _layers[spriteComp.drawOrder];
        layer.Add(new LayerItem(entity, trsComp.position.Y));
    }

    private void SortLayers()
    {
        for (int i = 0; i < _layers.Length; i++)
        {
            CachedList<LayerItem> layer = _layers[i];
            Array.Sort(layer.Arr, 0, layer.Count, _comparer);
        }
    }

    private void DrawLayers()
    {
        for (int i = 0; i < _layers.Length; i++)
        {
            CachedList<LayerItem> layer = _layers[i];
            for (int j = 0; j < layer.Count; j++)
                DrawEntity(layer[j].entity);
            layer.Reset();
        }
    }

    private void DrawEntity(Entity entity)
    {
        Components<SpriteComp, TransformComp> comps = entity.Get<SpriteComp, TransformComp>();
        ref SpriteComp spriteComp = ref comps.t0;
        ref TransformComp trsComp = ref comps.t1;

        Vector2 size = 0.0625f * trsComp.scale * spriteComp.sprite.rect.Size;
        Vector2 position = trsComp.position;

        // translate to camera space
        position -= _cam.frustum.min;

        // translate to screen space
        position *= _cam.pixelsPerUnit;
        size *= _cam.pixelsPerUnit;

        // rotation logic
        Vector2 offset = (0.5f * size).TurnDeg(trsComp.rotation);
        Rectangle dest = new Rectangle(position - offset, size);

        Raylib.DrawTexturePro(
            spriteComp.sprite.texture,
            spriteComp.sprite.rect,
            dest,
            Vector2.Zero,
            trsComp.rotation,
            Color.White
        );
    }

    private struct LayerItem
    {
        public Entity entity;
        public float y;

        public LayerItem(Entity entity, float y)
        {
            this.entity = entity;
            this.y = y;
        }
    }
}
