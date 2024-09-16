using System.Numerics;
using NekoRay.Tools;
using ZeroElectric.Vinculum;

namespace TowerDefence.UI;

public class ProgressBar : Pane {
    public Color FillColor = Raylib.GREEN;
    
    [Range(0, 1f)]
    public float Progress;

    protected override void Render() {
        base.Render();
        Raylib.DrawRectangleRounded(Rect with{width = Rect.Width*Progress}, Roundness, 8, FillColor);
    }
}