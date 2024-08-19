using NekoLib.Core;
using ZeroElectric.Vinculum;

namespace FlappyPegasus.Gui; 

public class Button : Behaviour {
    public string Text;
    public float Width;
    public float Height;
    public bool Disabled;
    public event Action OnClick;

    void Render() {
        if (Disabled) RayGui.GuiDisable();
        if (RayGui.GuiButton(new Rectangle(Transform.Position.X, Transform.Position.Y, Width, Height), Text) == 1)
            OnClick.Invoke();
        if (Disabled) RayGui.GuiEnable();
    }
}