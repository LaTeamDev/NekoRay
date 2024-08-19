using ImGuiNET;

namespace HotlineSPonyami.Tools;

public class Tools : EditorWindow
{
    void DrawGui()
    {
        if (ImGui.Begin("Tools"))
        {
            if (ImGui.CollapsingHeader("Tools"))
            {
                int i = 0;
                foreach (var tool in Scene.GetAllTools())
                {
                    if (Scene.SelectedTool != tool && ImGui.Button(tool.Name))
                    {
                        Scene.SelectTool(i);
                    }

                    if (Scene.SelectedTool == tool) ImGui.Text(Scene.SelectedTool.Name + " - Selected");
                    i++;
                }
            }

            if (ImGui.CollapsingHeader("Parameters"))
            {
                Scene?.SelectedTool?.DrawGui();
            }
            if (ImGui.CollapsingHeader("Camera")) {
                var zoom = Scene.Camera.Zoom;
                ImGui.DragFloat("Zoom", ref zoom, 0.01f, 0.01f, 10f);
                if (zoom != Scene.Camera.Zoom) Scene.Camera.Zoom = zoom;
            }
        }
        ImGui.End();
    }
}