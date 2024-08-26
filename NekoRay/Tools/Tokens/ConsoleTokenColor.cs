using System.Numerics;
using ImGuiNET;

namespace NekoRay.Tools; 

public class ConsoleTokenColor : IPoppableConsoleToken {
    public string Text => "";
    public bool Pop { get; set; }

    public ConsoleTokenColor(uint color) {
        Color = color;
    }

    public ConsoleTokenColor(string color) {
        if (!uint.TryParse(color, out Color)) Color = 0xFFFFFFFF;
    }
    
    public uint Color;
    
    public virtual void Render() {
        if (Pop) ImGui.PopStyleColor();
        ImGui.PushStyleColor(ImGuiCol.Text, Color);
    }
}