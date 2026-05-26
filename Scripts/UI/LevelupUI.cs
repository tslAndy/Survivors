using Arch.Bus;
using Cysharp.Text;
using Events;
using ImGuiNET;
using Other;
using Systems;
using Systems.Basic;
using Systems.Drawing;
using Utils;

namespace UI;

partial class LevelupUI : ElemUI
{
    private readonly LevelSys _levelSys;
    private readonly WorldContext _context;

    private const int ITEMS_AMOUNT = 4;

    public LevelupUI(LevelSys levelSys, WorldContext context)
    {
        _levelSys = levelSys;
        _context = context;
        Hook();
    }

    public override void Draw()
    {
        ShuffleSelector<Item>? selector = _levelSys.level?.itemSelector;
        if (selector == null || selector.Count == 0)
        {
            Finish();
            return;
        }

        ImGui.Begin("levelUp");

        ImGui.Text("huhu");
        ImGui.SameLine();
        ImGui.Text(" klklkl");

        using var sb = ZString.CreateStringBuilder();
        for (int i = 0; i < Math.Min(selector.Count, ITEMS_AMOUNT); i++)
        {
            Item item = selector[i];

            sb.Clear();
            sb.AppendFormat("{0}: {1}", item.name, item.description);
            if (!ImGui.Button(sb.AsSpan()))
                continue;

            item.Pickup(_context.player);
            selector.Exclude(i);

            Finish();
            break;
        }

        ImGui.End();
    }

    private void Finish()
    {
        isActive = false;
        PausedEvent pausedEvent = new PausedEvent { isPaused = false };
        EventBus.Send(ref pausedEvent);
    }

    [Event]
    public void OnLevelup(ref LevelupEvent @event)
    {
        isActive = true;
        PausedEvent pausedEvent = new PausedEvent { isPaused = true };
        EventBus.Send(ref pausedEvent);
    }
}
