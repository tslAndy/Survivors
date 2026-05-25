using Achieves;
using Arch.Bus;
using ImGuiNET;
using Systems.Drawing;

namespace UI;

partial class AchievesUI : ElemUI
{
    public AchievesUI() => Hook();

    public override void Draw()
    {
        ImGui.Begin("Achieves");
        ImGui.Text("hehehehe");
        ImGui.Text("huhu");
        ImGui.Text("hahaha");
        ImGui.End();
    }

    public override void Update(float dt) { }

    [Event]
    public void OnAchieveUnlocked(Achieve achieve)
    {
        Console.WriteLine($"{achieve.name}: {achieve.description}");
    }
}
