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
    private bool _autoSize = true;
    private bool _fill = false;
    private int _displayWidth;
    private int _displayHeight;

    private bool DrawSize(string id, ref int width, ref int height) {
        ImGui.SetNextItemWidth(64f);
        if (ImGui.InputInt($"##{id}-width", ref width, 0)) {
            return true;
        }
        ImGui.SameLine();
        ImGui.Text("x");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(64f);
        if (ImGui.InputInt($"##{id}-height", ref height, 0)) {
            return true;
        }

        return false;
    }

    [UsedImplicitly]
    void DrawGui() {
        //TODO: make it use different fill methods
        _renderWidth = BaseCamera.Main?.RenderWidth ?? -1;
        _renderHeight = BaseCamera.Main?.RenderHeight ?? -1;
        if (ImGui.Begin("Game View", ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.HorizontalScrollbar)) {
            if (ImGui.BeginMenuBar()) {
                if (ImGui.BeginMenu("Display Settings")) {
                    ImGui.SeparatorText("Render Size");
                    //ImGui.InputInt2("##render-size", ref lol.a);
                    if (DrawSize("render", ref _renderWidth, ref _renderHeight)) {
                        BaseCamera.Main.RenderWidth = _renderWidth;
                        BaseCamera.Main.RenderHeight = _renderHeight;
                        BaseCamera.Main.RecreateRenderTexture();
                        _autoSize = false;
                    }

                    if (ImGui.Checkbox("Auto-size", ref _autoSize)) {
                        if (_autoSize) {
                            BaseCamera.Main.RenderWidth = -1;
                            BaseCamera.Main.RenderHeight = -1;
                            BaseCamera.Main.RecreateRenderTexture();
                        }
                        else {
                            BaseCamera.Main.RenderWidth = _renderWidth;
                            BaseCamera.Main.RenderHeight = _renderHeight;
                            BaseCamera.Main.RecreateRenderTexture();
                        }
                    }
                    
                    ImGui.SeparatorText("Display");
                    ImGui.Checkbox("Fill", ref _fill);
                    if (_fill) ImGui.BeginDisabled();
                    DrawSize("display", ref _displayWidth, ref _displayHeight);
                    if (_fill) ImGui.EndDisabled();
                    
                    ImGui.EndMenu();
                }
                ImGui.Text("FPS:"+Raylib.GetFPS());
                ImGui.EndMenuBar();
            }
            ImGui.BeginChild("GameRenderer");
            var aspectRatio = _renderWidth/_renderHeight;
            Vector2 wsize;
            if (!_fill) wsize = new Vector2(_displayWidth, _displayHeight);
            else wsize = new Vector2(_renderWidth, _renderHeight);
            var mousePosScalingFactor = _renderWidth / wsize.X;
            var startPos = ImGui.GetCursorScreenPos();
            if (BaseCamera.Main is not null) {
                //GraphicsReferences.OpenGl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                //ImGui.Image((nint) Camera.MainCamera.RenderTexture.OpenGlHandle, wsize with {Y = wsize.X/aspectRatio}, new(0, 1), new(1, 0));
                var w = ImGui.GetWindowWidth();
                var h = ImGui.GetWindowHeight();
                //if (w > wsize.X) wsize
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