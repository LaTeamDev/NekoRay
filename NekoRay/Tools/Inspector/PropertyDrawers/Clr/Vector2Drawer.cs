using System.Numerics;
using ImGuiNET;

namespace NekoRay.Tools;

[CustomDrawer(typeof(Vector2))]
public class Vector2Drawer : SimpleDrawer<Vector2> {
    protected override bool DrawInput(string label, ref Vector2 value) => 
        ImGui.InputFloat2(label, ref value);
    
    protected override bool DrawRange(string label, ref Vector2 value, float min, float max) => 
        ImGui.SliderFloat2(label, ref value, min, max);
}