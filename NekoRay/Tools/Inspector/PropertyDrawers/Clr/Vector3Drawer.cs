using System.Numerics;
using ImGuiNET;

namespace NekoRay.Tools;

[CustomDrawer(typeof(Vector3))]
public class Vector3Drawer : SimpleDrawer<Vector3> {
    protected override bool DrawInput(string label, ref Vector3 value) => 
        ImGui.InputFloat3(label, ref value);
    
    protected override bool DrawRange(string label, ref Vector3 value, float min, float max) => 
        ImGui.SliderFloat3(label, ref value, min, max);
}