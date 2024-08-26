using ImGuiNET;

namespace NekoRay.Tools; 

public class ConsoleTokenString : IConsoleToken {
    public string Text { get; set; } = "";

    public ConsoleTokenString(string text) {
        Text = text;
    }
    
    public virtual void Render() {
        ImGui.Text(Text);
    }
}