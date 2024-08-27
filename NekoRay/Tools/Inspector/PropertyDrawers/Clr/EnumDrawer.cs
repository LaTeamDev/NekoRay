using System.Reflection;
using ImGuiNET;

namespace NekoRay.Tools;

[CustomDrawer(typeof(Enum))]
public class EnumDrawer : SimpleDrawer<Enum> {
    protected override bool DrawInput(string label, ref Enum value) {
        if (value.GetType().GetCustomAttribute<FlagsAttribute>() is not null) return DrawFlags(label, ref value);
        return DrawCombo(label, ref value);
    }

    protected virtual bool DrawFlags(string label, ref Enum value) {
        ImGui.BeginGroup();
        try {
            ImGui.Text(label);
            var oldFlags = Convert.ToInt32(value);
            var flags = oldFlags;
            foreach (Enum enumValue in Enum.GetValues(value.GetType())) {
                ImGui.CheckboxFlags(enumValue.ToString(), ref flags, Convert.ToInt32(enumValue));
            }

            if (oldFlags == flags) {
                ImGui.EndGroup();
                return false;
            }
            value = (Enum)Convert.ChangeType(flags, value.GetType());
        }
        catch (Exception e) {
            ImGui.Text("Failed to render flags");
        }
        ImGui.EndGroup();
        return false;
    }

    protected virtual bool DrawCombo(string label, ref Enum value) {
        if(ImGui.BeginCombo(label, value.ToString())) {
            foreach (Enum enumValue in Enum.GetValues(value.GetType())) {
                var selected = value.Equals(enumValue);
                if (ImGui.Selectable(enumValue.ToString(), selected)) {
                    value = enumValue;
                    return true;
                }

                // Set the initial focus when opening the combo (scrolling + keyboard navigation focus)
                if (selected)
                    ImGui.SetItemDefaultFocus();
            }
        ImGui.EndCombo();
        }
        return false;
    }
}