using NekoLib.Core;

namespace FlappyPegasus.Gui; 

public class ButtonLayout : Behaviour {
    public float Spacing = 10f;
    public void Calculate() {
        var y = 0f;
        foreach (var button in GameObject.GetComponentsInChildren<Button>()) {
            button.Transform.LocalPosition = button.Transform.LocalPosition with { Y = y };
            y = y + button.Height + Spacing;
        }
    }
}