using System.Reflection;
using ImGuiNET;

namespace NekoRay.Tools;

public abstract class Drawer {
    public virtual void DrawGui(MemberInfo info, object? obj) {
        var header = info.GetCustomAttribute<SeparatorAttribute>();
        if (header is not null) ImGui.SeparatorText(header.Text);
    }
}