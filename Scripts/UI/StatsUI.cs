using System.Numerics;
using Arch.Core.Extensions;
using Components.Health;
using Cysharp.Text;
using ImGuiNET;
using Other;
using Systems;
using Systems.Basic;
using Systems.Drawing;

namespace UI;

public class StatsUI : ElemUI
{
    private readonly WorldContext _context;
    private readonly ExpSys _expSys;
    private readonly LevelSys _levelSys;

    private readonly Vector4 healthbarColor = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
    private readonly Vector4 expbarColor = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);

    public StatsUI(WorldContext context, ExpSys expSys, LevelSys levelSys)
    {
        _context = context;
        _expSys = expSys;
        _levelSys = levelSys;
    }

    public override void Draw()
    {
        ImGui.Begin("Stats", ImGuiWindowFlags.NoTitleBar);

        using (var sb = ZString.CreateStringBuilder())
        {
            sb.AppendFormat("Lvl: {0}", _expSys.level);
            ImGui.Text(sb.AsSpan());
        }

        using (var sb = ZString.CreateStringBuilder())
        {
            int time = (int)MathF.Floor(_levelSys.level?.time ?? 0);
            sb.AppendFormat("Time: {0:00}:{1:00}", time / 60, time % 60);
            ImGui.Text(sb.AsSpan());
        }

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
