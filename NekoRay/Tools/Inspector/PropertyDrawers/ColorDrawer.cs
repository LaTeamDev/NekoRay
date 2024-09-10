using ImGuiNET;

namespace NekoRay.Tools;

[CustomDrawer(typeof(Color))]
public class ColorDrawer : SimpleDrawer<Color> {
    protected override bool DrawInput(string label, ref Color value) {
        var color = value.ToVector4();
        if (ImGui.ColorEdit4(label, ref color)) {
            value = color.ToColor();
            return true;
        }

        return false;
    }
}