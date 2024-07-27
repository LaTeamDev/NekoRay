using System.Numerics;
using ImGuiNET;
using ZeroElectric.Vinculum;
using Texture = NekoRay.Texture;

namespace HotlineSPonyami.Tools;

public abstract class TileTool : BaseTool
{
    private static int _selectedTexture = -1; protected static int SelectedTexture => _selectedTexture;
    private static Texture? _previewTexture = null;
    private bool _isSelecting = false;
    
    public override void OnSelect()
    {
        
    }

    protected abstract void OnUpdate(int x, int y);
    public sealed override void OnUpdate()
    {
        int cellX;
        int cellY;
        Scene.GetCurrentCell(out cellX, out cellY);
        OnUpdate(cellX, cellY);
    }

    public sealed override void OnDraw()
    {
        int cellX;
        int cellY;
        Scene.GetCurrentCell(out cellX, out cellY);
        OnDraw(cellX, cellY);
    }

    protected virtual void OnDraw(int x, int y)
    {
        if(x >= 0 && y >= 0 && x < Scene.Field.SizeX && y < Scene.Field.SizeY) 
            Raylib.DrawRectangleLines(x * TileField.TextureSize, y * TileField.TextureSize, TileField.TextureSize, TileField.TextureSize, Raylib.RED);
    }

    public override void DrawGui()
    {
        //ImGui.InputText("Selected Texture", ref _selectedTexture, 25); //Temp
        
        ImGui.Text("Selected: " + _selectedTexture);
        if (SelectedTexture >= 0 && _previewTexture == null)
        {
            _previewTexture = UnpackedTextures.GetFloorTextureById(SelectedTexture);
        }
        if(_previewTexture != null) ImGui.Image((IntPtr)_previewTexture.Id, new Vector2(32, 32));
        if (ImGui.Button("Clear"))
        {
            _previewTexture = null;
            _selectedTexture = -1;
        }
        ImGui.SameLine();
        if (ImGui.Button("Select"))
        {
            _isSelecting = true;
            ImGui.OpenPopup("Texture Selector");
        }

        if (ImGui.BeginPopupModal("Texture Selector", ref _isSelecting, ImGuiWindowFlags.AlwaysAutoResize))
        {
            if (ImGui.BeginChild("Preview", new Vector2(200, 150)))
            {
                const int perLine = 4;
                int l = 0;
                int i = 0;
                foreach (var image in UnpackedTextures.GetAllTextures())
                {
                    if (ImGui.ImageButton("floor_tex" + i, (IntPtr)image.Id, new Vector2(32, 32)))
                    {
                        _previewTexture = null;
                        _selectedTexture = i;
                        _isSelecting = false;
                    }
                    l++;
                    i++;
                    if (l >= perLine)
                        l = 0;
                    else
                        ImGui.SameLine();
                }
                ImGui.EndChild();
            }
            
            if (ImGui.Button("Cancel"))
            {
                _isSelecting = false;
            }
            ImGui.EndPopup();
        }
    }
}