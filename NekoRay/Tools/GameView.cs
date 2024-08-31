using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ImGuiNET;
using JetBrains.Annotations;
using rlImGui_cs;

namespace NekoRay.Tools; 

public class GameView : ToolBehaviour {
    private int _renderWidth;
    private int _renderHeight;

    [UsedImplicitly]
    void DrawGui() {
        //TODO: make it use different fill methods
        _renderWidth = BaseCamera.Main?.RenderWidth ?? -1;
        _renderHeight = BaseCamera.Main?.RenderHeight ?? -1;
        if (ImGui.Begin("Game View", ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.HorizontalScrollbar)) {
            if (ImGui.BeginMenuBar()) {
                if (ImGui.BeginMenu($"Screen Size")) {
                    //ImGui.InputInt2("##render-size", ref lol.a);

                    ImGui.SetNextItemWidth(64f);
                    if (ImGui.InputInt("##render-width", ref _renderWidth, 0)) {
                        BaseCamera.Main.RenderWidth = _renderWidth;
                        BaseCamera.Main.RecreateRenderTexture();
                    }
                    ImGui.SameLine();
                    ImGui.Text("x");
                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(64f);
                    if (ImGui.InputInt("##render-height", ref _renderHeight, 0)) {
                        BaseCamera.Main.RenderHeight = _renderHeight;
                        BaseCamera.Main.RecreateRenderTexture();
                    }ImGui.EndMenu();
                }
                ImGui.Text("FPS:"+Raylib.GetFPS());
                ImGui.EndMenuBar();
            }
            ImGui.BeginChild("GameRenderer");
            var aspectRatio = _renderWidth/_renderHeight;
            var wsize = new Vector2(_renderWidth, _renderHeight);
            var mousePosScalingFactor = _renderWidth / wsize.X;
            var startPos = ImGui.GetCursorScreenPos();
            if (BaseCamera.Main is not null) {
                //GraphicsReferences.OpenGl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                //ImGui.Image((nint) Camera.MainCamera.RenderTexture.OpenGlHandle, wsize with {Y = wsize.X/aspectRatio}, new(0, 1), new(1, 0));
                ImGui.Image((nint)BaseCamera.Main.RenderTexture.Texture._texture.id,wsize, new(0, 1), new(1, 0));
            }
            //else
            //ImGui.Image((nint)Texture.Missing.OpenGlHandle, wsize, new(0, 1), new(1, 0));
            if (ImGui.IsItemHovered(ImGuiHoveredFlags.DelayNone)) {
                Input.ForceUpdate = true;
                Input._lastMousePos = (ImGui.GetMousePos() - startPos) * mousePosScalingFactor;
            }
            else {
                Input.ForceUpdate = false;
            }
            ImGui.EndChild();
        }
        ImGui.End();
    }

    void OnDisable() {
        if (BaseCamera.Main is null) return;
        BaseCamera.Main.RenderWidth = -1;
        BaseCamera.Main.RenderHeight = -1;
    }

    [ConCommand("game_view")]
    public static void OpenGameView() => ToolsShared.ToggleTool<GameView>();
}