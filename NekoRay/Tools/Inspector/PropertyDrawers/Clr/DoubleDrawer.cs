using System.Reflection;
using ImGuiNET;

namespace NekoRay.Tools;

[CustomDrawer(typeof(double))]
public class DoubleDrawer : SimpleDrawer<double> {
    protected override bool DrawInput(string label, ref double value) =>
        ImGui.InputDouble(label, ref value);

    protected override bool DrawRange(string label, ref double value, float min, float max) {
        var fl = ((float) value);
        if (ImGui.SliderFloat(label, ref fl, min, max)) {
            value = fl;
            return true;
        }
        return false;
    }
}