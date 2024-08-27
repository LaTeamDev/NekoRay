using System.Numerics;
using System.Reflection;
using ImGuiNET;

namespace NekoRay.Tools;

[CustomDrawer(typeof(string))]
public class StringDrawer : SimpleDrawer<string> {
    private bool _drawMultiline;
    protected override bool DrawInput(string label, ref string value) {
        if (_drawMultiline) return ImGui.InputTextMultiline(label, ref value, 256, Vector2.Zero);
        return ImGui.InputText(label, ref value, 256);
    }

    public override void DrawGui(MemberInfo info, object? obj) {
        _drawMultiline = Attribute.IsDefined(info, typeof(MultilineAttribute));
        base.DrawGui(info, obj);
    }
}