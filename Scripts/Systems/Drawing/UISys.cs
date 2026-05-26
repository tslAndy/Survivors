using Arch.Core;
using Arch.System;
using rlImGui_cs;

namespace Systems.Drawing;

public class ElemUI
{
    public virtual void Update(float dt) { }

    public virtual void Draw() { }
}

public partial class UISys : BaseSystem<World, float>
{
    private readonly List<ElemUI> _elems;

    public UISys(World world)
        : base(world) => _elems = new List<ElemUI>();

    public void AddElem(ElemUI elem) => _elems.Add(elem);

    public void RemoveElem(ElemUI elem) => _elems.Remove(elem);

    public override void Update(in float dt)
    {
        foreach (ElemUI elem in _elems)
            elem.Update(dt);

        rlImGui.Begin();
        foreach (ElemUI elem in _elems)
            elem.Draw();
        rlImGui.End();
    }
}
