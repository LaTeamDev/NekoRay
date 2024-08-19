using ZeroElectric.Vinculum;
using Texture = NekoRay.Texture;

namespace HotlineSPonyami.Tools;

public abstract class TileTool : BaseTool
{
    private static byte _selectedTexture = 0; protected static byte SelectedTexture => _selectedTexture;
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
        UnpackedTextures.DrawImGuiSelector(ref _selectedTexture, ref _previewTexture, ref _isSelecting, UnpackedTextures.GetAllFloorTextures());
    }
}