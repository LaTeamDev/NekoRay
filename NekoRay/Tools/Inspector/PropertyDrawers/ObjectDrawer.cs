using System.Reflection;
using ImGuiNET;

namespace NekoRay.Tools;
[CustomDrawer(typeof(object))]
public class ObjectDrawer : Drawer {
    public override void DrawGui(MemberInfo info, object? obj) {
        base.DrawGui(info, obj);
        ImGui.Text(info.Name);
        ImGui.SameLine();
        if (!ImGui.Button("inspect##"+info.Name)) return;
        if (info.MemberType == MemberTypes.Property) {
            if (((PropertyInfo)info).GetMethod is not null)
            Inspect.Instance.SelectedObject = ((PropertyInfo)info).GetValue(obj);
            return;
        }

        if (info.MemberType == MemberTypes.Field) {
            Inspect.Instance.SelectedObject = ((FieldInfo)info).GetValue(obj);
        }
    }
}