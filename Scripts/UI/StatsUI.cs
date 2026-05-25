using System.Numerics;
using Arch.Core.Extensions;
using Components.Health;
using ImGuiNET;
using Other;
using Systems;
using Systems.Drawing;

namespace UI;

class StatsUI : ElemUI
{
    private readonly WorldContext _context;
    private readonly ExpSys _expSys;

    private readonly Vector4 healthbarColor = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
    private readonly Vector4 expbarColor = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);

    public StatsUI(WorldContext context, ExpSys expSys)
    {
        _context = context;
        _expSys = expSys;
    }

    public override void Draw()
    {
        ImGui.Begin("Stats", ImGuiWindowFlags.NoTitleBar);

        ImGui.PushStyleColor(ImGuiCol.PlotHistogram, healthbarColor);
        ref HealthComp health = ref _context.player.Get<HealthComp>();
        float healthRate = health.currentHP / (float)health.maxHP;
        ImGui.ProgressBar(healthRate, new Vector2(1000, 20));
        ImGui.PopStyleColor();

        ImGui.PushStyleColor(ImGuiCol.PlotHistogram, expbarColor);
        float expRate = _expSys.currentExp / (float)_expSys.totalExp;
        ImGui.ProgressBar(expRate, new Vector2(1000, 20));
        ImGui.PopStyleColor();

        ImGui.End();
    }
}
