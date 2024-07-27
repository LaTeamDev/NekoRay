using ImGuiNET;
using NekoLib.Core;

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
        }
        ImGui.End();
    }
}