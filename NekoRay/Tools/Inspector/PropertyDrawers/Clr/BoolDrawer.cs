using System.Numerics;
using ImGuiNET;

namespace NekoRay.Tools;

[CustomDrawer(typeof(bool))]
public class BoolDrawer : SimpleDrawer<bool> {
    protected override bool DrawInput(string label, ref bool value) => 
        ImGui.Checkbox(label, ref value);
}