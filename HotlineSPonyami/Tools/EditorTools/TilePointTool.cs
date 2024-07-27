using ZeroElectric.Vinculum;

namespace HotlineSPonyami.Tools.EditorTools;

public class TilePointTool : TileTool
{
    public override string Name => "Single Tile";

    public override void OnSelect()
    {
        
    }

    protected override void OnUpdate(int x, int y)
    {
        if (Raylib.IsMouseButtonDown(0))
        {
            Scene.SetTextureToTile(x, y, SelectedTexture);
        }
    }
}