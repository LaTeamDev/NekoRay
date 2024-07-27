using ImGuiNET;
using ZeroElectric.Vinculum;

namespace HotlineSPonyami.Tools;

public abstract class TileTool : BaseTool
{
    private static string _selectedTexture = "dev_floor.png"; protected static string SelectedTexture => _selectedTexture;
    
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
        if(x >= 0 && y >= 0 && x < Scene.SizeX && y < Scene.SizeY) 
            Raylib.DrawRectangleLines(x * EditorScene.TextureSize, y * EditorScene.TextureSize, EditorScene.TextureSize, EditorScene.TextureSize, Raylib.RED);
    }

    public override void DrawGui()
    {
        ImGui.InputText("Selected Texture", ref _selectedTexture, 25); //Temp
    }
}