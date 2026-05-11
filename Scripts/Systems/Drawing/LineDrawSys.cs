using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Components.Basic;
using Engine.Common;
using Raylib_cs;
using Utils;

namespace Systems.Drawing;

partial class LineDrawSys : BaseSystem<World, float>
{
    private const int MAX_LAYERS = 64;

    private readonly Camera _cam;
    private readonly CachedList<Entity>[] _layers;

    public LineDrawSys(World world, Camera cam)
        : base(world)
    {
        _cam = cam;
        _layers = new CachedList<Entity>[MAX_LAYERS];

        for (int i = 0; i < _layers.Length; i++)
            _layers[i] = CachedList<Entity>.Create();
    }

    public override void Update(in float t)
    {
        FillLayersQuery(World);
        DrawLayers();
    }

    [Query]
    private void FillLayers(Entity entity, in LineComp lineComp)
    {
        CachedList<Entity> layer = _layers[lineComp.drawOrder];
        layer.Add(entity);
    }

    private void DrawLayers()
    {
        for (int i = 0; i < _layers.Length; i++)
        {
            CachedList<Entity> layer = _layers[i];
            for (int j = 0; j < layer.Count; j++)
                DrawEntity(layer[j]);
            layer.Reset();
        }
    }

    private void DrawEntity(Entity entity)
    {
        AABB camBox = _cam.frustum;

        ref LineComp lineComp = ref entity.Get<LineComp>();
        CachedList<Line> lines = lineComp.lines;
        for (int i = 0; i < lines.Count; i++)
        {
            ref Line line = ref lines[i];
            AABB aabb = AABB.ByBounds(line.start, line.end);
            if (!AABB.CheckOverlap(camBox, aabb))
                continue;

            Vector2 start = _cam.pixelsPerUnit * (line.start - camBox.min);
            Vector2 end = _cam.pixelsPerUnit * (line.end - camBox.min);
            float thick = _cam.pixelsPerUnit * line.thick;
            Raylib.DrawLineEx(start, end, thick, line.color);
        }
        lineComp.lines.Reset();
    }
}
