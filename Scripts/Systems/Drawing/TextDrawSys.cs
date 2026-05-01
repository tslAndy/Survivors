using System.Numerics;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Engine.Common;
using Raylib_cs;
using Utils;

namespace Systems.Drawing;

partial class TextDrawSys : BaseSystem<World, float>
{
    private readonly Font _font;
    private readonly Camera _cam;

    public TextDrawSys(World world)
        : base(world)
    {
        _font = Raylib.GetFontDefault();
        _cam = ServiceLocator.Get<Camera>();
    }

    [Query]
    private void DrawText(ref TextComp text, ref TransformComp trs)
    {
        float fontSize = text.fontSize * trs.scale * _cam.pixelsPerUnit;
        Vector2 size =
            1.0f / _cam.pixelsPerUnit * Raylib.MeasureTextEx(_font, text.text, fontSize, 1.0f);
        Vector2 position = trs.position;

        AABB textBox = AABB.BySize(position, size);
        AABB camBox = _cam.frustum;
        if (!AABB.CheckOverlap(textBox, camBox))
            return;

        position -= camBox.min;
        position *= _cam.pixelsPerUnit;

        Raylib.DrawTextPro(
            _font,
            text.text,
            position,
            Vector2.Zero,
            0.0f,
            fontSize,
            1.0f,
            text.color
        );
    }
}
