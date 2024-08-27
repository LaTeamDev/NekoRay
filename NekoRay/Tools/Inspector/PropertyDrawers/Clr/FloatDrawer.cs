using System.Reflection;
using ImGuiNET;

namespace NekoRay.Tools;

[CustomDrawer(typeof(float))]
public class FloatDrawer : SimpleDrawer<float> {
    protected override bool DrawInput(string label, ref float value) =>
        ImGui.InputFloat(label, ref value);

    protected override bool DrawRange(string label, ref float value, float min, float max) =>
        ImGui.SliderFloat(label, ref value, min, max);
}