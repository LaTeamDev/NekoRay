using System.Reflection;
using ImGuiNET;

namespace NekoRay.Tools;

[CustomDrawer(typeof(int))]
public class IntDrawer : SimpleDrawer<int> {
    protected override bool DrawInput(string label, ref int value) =>
        ImGui.InputInt(label, ref value);

    protected override bool DrawRange(string label, ref int value, float min, float max) =>
        ImGui.SliderInt(label, ref value, (int)min, (int)max);
}