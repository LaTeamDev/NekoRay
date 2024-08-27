using System.Numerics;
using ImGuiNET;

namespace NekoRay.Tools;

[CustomDrawer(typeof(Vector4))]
public class Vector4Drawer : SimpleDrawer<Vector4> {
    protected override bool DrawInput(string label, ref Vector4 value) => 
        ImGui.InputFloat4(label, ref value);
    
    protected override bool DrawRange(string label, ref Vector4 value, float min, float max) => 
        ImGui.SliderFloat4(label, ref value, min, max);
}