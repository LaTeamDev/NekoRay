using System.Reflection;
using ImGuiNET;
using NekoRay.Easings;

namespace NekoRay.Tools;

[CustomDrawer(typeof(IEasing))]
public class EasingDrawer : SimpleDrawer<IEasing> {
    protected override bool DrawInput(string label, ref IEasing value) => throw new NotImplementedException();

    public override void DrawGui(FieldInfo info, object? obj) {
        var name = info.GetValue(obj)?.GetType().ToString() ?? typeof(EaseLinear).ToString();
        if (ImGui.BeginCombo(info.Name, name)) {
            foreach (var type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(domainAssembly => domainAssembly.GetTypes())
                         .Where(type => type.IsAssignableTo(typeof(IEasing)))) {
                var selected = type.ToString().Equals(name);
                if (ImGui.Selectable(type.ToString(), selected)) {
                    IEasing a = (IEasing)Activator.CreateInstance(type);
                    info.SetValue(a, obj);
                    return;
                }
                // Set the initial focus when opening the combo (scrolling + keyboard navigation focus)
                if (selected)
                    ImGui.SetItemDefaultFocus();
            }
            ImGui.EndCombo();
        }
    }
}