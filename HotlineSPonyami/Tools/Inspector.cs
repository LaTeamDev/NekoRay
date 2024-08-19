using System.Reflection;
using ImGuiNET;

namespace HotlineSPonyami.Tools;

public class Inspector : EditorWindow
{
    void DrawGui()
    {
        if (Scene == null) return;
        if (ImGui.Begin("Inspector")) {
            try {
                Scene.GetCurrentCell(out var x, out var y);
                if ((x > Scene.Field.SizeX  && x < 0) || (y > Scene.Field.SizeY && y < 0)) return;
                var tile = Scene.Field[x, y];
                ImGui.Text("Selected tile info:");
                foreach (var field in tile.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {
                    ImGui.Text(field.Name+":");
                    ImGui.SameLine();
                    ImGui.Text((string)field.FieldType
                        .GetMethod("ToString", 
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy, new Type[]{})
                        .Invoke(field.GetValue(tile), null));
                }
                ImGui.Text("Floor Id: "+tile.FloorId);
            }
            catch (Exception e) {
                ImGui.TextWrapped($"There was an error while rendering inspector\n{e}");
            }
        }
        ImGui.End();
    }
}