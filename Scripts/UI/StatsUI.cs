using System.Numerics;
using Arch.Core;
using Components.Health;
using Components.Loot;
using ImGuiNET;
using Systems;
using Systems.Drawing;

namespace UI;

class StatsUI : ElemUI
{
    private WorldContext _context;

    private readonly Vector4 healthbarColor = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
    private readonly Vector4 expbarColor = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);

    public StatsUI(WorldContext context) => _context = context;

    public override void Draw()
    {
        Components<HealthComp, LootComp> comps = _context.player.Get<HealthComp, LootComp>();
        ref HealthComp health = ref comps.t0;
        ref LootComp loot = ref comps.t1;

        float healthRate = 0.7f;

        ImGui.Begin("Stats", ImGuiWindowFlags.NoTitleBar);

        ImGui.PushStyleColor(ImGuiCol.PlotHistogram, healthbarColor);
        ImGui.ProgressBar(healthRate, new Vector2(1000, 20));
        ImGui.PopStyleColor();

        ImGui.End();
    }
}
