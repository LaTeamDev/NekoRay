using ImGuiNET;
using NekoLib.Core;
using NekoRay.Tools;

namespace WebRenderingTest.Tools;

[CustomInspector(typeof(HtmlRenderer))]
public class HtmlInspector : Inspector {
    private string url = "";
    public override void DrawGui() {
        var target = ((HtmlRenderer) Target);
        ImGui.InputTextWithHint("URL", target.Url, ref url, 512);
        ImGui.SameLine();
        if (ImGui.Button("Go")) {
            target.OpenPage(url);
            url = "";
        }
    }
}