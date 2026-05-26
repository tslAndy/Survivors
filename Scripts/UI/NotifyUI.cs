using Achieves;
using Arch.Bus;
using Events;
using ImGuiNET;
using Systems.Drawing;

namespace UI;

public partial class NotifyUI : ElemUI
{
    private readonly List<Elem> _elems = new List<Elem>();

    private const float NOTIFICATION_TIME = 5.0f;

    public NotifyUI() => Hook();

    public override void Update(float dt)
    {
        int i = 0;
        while (i < _elems.Count)
        {
            Elem elem = _elems[i];
            elem.time -= dt;

            if (elem.time > 0.0f)
            {
                _elems[i] = elem;
                i++;
            }
            else
            {
                _elems[i] = _elems[_elems.Count - 1];
                _elems.RemoveAt(_elems.Count - 1);
            }
        }
    }

    public override void Draw()
    {
        ImGui.Begin(
            "Notifications",
            ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoTitleBar
        );

        for (int i = 0; i < _elems.Count; i++)
            ImGui.Text(_elems[i].text);

        ImGui.End();
    }

    [Event]
    public void OnLevelup(ref LevelupEvent @event) =>
        _elems.Add(new Elem($"Got new level: {@event.level}", NOTIFICATION_TIME));

    [Event]
    public void OnAchieveUnlocked(Achieve achieve) =>
        _elems.Add(new Elem($"{achieve.name}: {achieve.description}", NOTIFICATION_TIME));

    private record struct Elem(string text, float time);
}
