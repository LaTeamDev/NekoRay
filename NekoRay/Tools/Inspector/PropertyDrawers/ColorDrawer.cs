using ImGuiNET;

namespace NekoRay.Tools;

[CustomDrawer(typeof(RayColor))]
public class ColorDrawer : SimpleDrawer<RayColor> {
    protected override bool DrawInput(string label, ref RayColor value) {
        var color = value.ToVector4();
        if (ImGui.ColorEdit4(label, ref color)) {
            value = color.ToColor();
            return true;
        }

        return false;
    }
}