using ImGuiNET;

namespace NekoRay.Tools; 

public class ImguiDemoWindow : Behaviour {
    void DrawGui() {
        ImGui.ShowDemoWindow();
    }
}