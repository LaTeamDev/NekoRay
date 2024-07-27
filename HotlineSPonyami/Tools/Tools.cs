using ImGuiNET;
using NekoLib.Core;

namespace HotlineSPonyami.Tools;

public class Tools : Behaviour
{
    private EditorScene? _scene;

    public void Initialize(EditorScene scene)
    {
        if (_scene == null)
            _scene = scene;
    }
    
    void DrawGui()
    {
        if (ImGui.Begin("Tools"))
        {
            ImGui.Text("Tools:");
            int i = 0;
            foreach (var tool in _scene.GetAllTools())
            {
                if (ImGui.Button(tool.Name))
                {
                    _scene.SelectTool(i);
                }
                i++;
            }
            ImGui.Text("Parameters:");
            _scene?.SelectedTool?.DrawGui();
        }
        ImGui.End();
    }
}