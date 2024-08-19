using ZeroElectric.Vinculum;
using Texture = NekoRay.Texture;

namespace HotlineSPonyami.Tools.EditorTools;

public abstract class BaseWallTool : BaseTool
{
    private static byte _selectedTexture = 0; protected static byte SelectedTexture => _selectedTexture;
    private static Texture? _previewTexture = null;
    private bool _isSelecting = false;
    protected void DrawCorner(int x, int y, Color color)
    {
        int s = 10;
        Raylib.DrawRectangle(x * 32 - s / 2, y * 32 - s / 2, s, s, color);
    }

    public override void OnSelect()
    {
        
    }

    public override void DrawGui()
    {
        UnpackedTextures.DrawImGuiSelector(ref _selectedTexture, ref _previewTexture, ref _isSelecting, UnpackedTextures.GetAllWallTextures());
    }
}