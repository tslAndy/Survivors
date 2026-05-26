using Arch.Core.Extensions;
using Components.Basic;
using Cysharp.Text;
using Engine.Common;
using ImGuiNET;
using Systems;
using Systems.Basic;
using Systems.Drawing;

namespace UI;

public class ModsUI : ElemUI
{
    private readonly WorldContext _context;

    public ModsUI(WorldContext context) => _context = context;

    public override void Draw()
    {
        ref ModComp mod = ref _context.player.Get<ModComp>();

        ImGui.Begin("Mods", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoTitleBar);

        using var sb = ZString.CreateStringBuilder();
        for (int i = 0; i < ModRegistry.GetCount(); i++)
        {
            (Hash hash, string modName) = ModRegistry.GetElement(i);
            sb.AppendFormat("{0}: {1:.##}", modName, mod[hash]);
            ImGui.Text(sb.AsSpan());
            sb.Clear();
        }
        ImGui.End();
    }
}
