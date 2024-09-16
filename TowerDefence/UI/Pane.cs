using NekoRay;
using ZeroElectric.Vinculum;

namespace TowerDefence.UI;

public class Pane : UiElement {
    public Color Color = Raylib.WHITE;
    protected virtual Rectangle Rect => new(Transform.Position.X, Transform.Position.Y, Size.X, Size.Y);
    public float Roundness = 0f;
    
    public override void RecomputeSize() {
        Size = Transform.LocalScale.ToVector2();
    }

    protected virtual void Render() {
        Raylib.DrawRectangleRounded(Rect, Roundness, 8, Color);
    }
}