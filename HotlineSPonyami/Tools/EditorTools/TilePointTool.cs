using ZeroElectric.Vinculum;

namespace HotlineSPonyami.Tools.EditorTools;

public class TilePointTool : TileTool
{
    public override string Name => "Single Tile";

    protected override void OnUpdate(int x, int y)
    {
        if (Raylib.IsMouseButtonDown(0))
        {
            Scene.Field.SetTileFloor(x, y, SelectedTexture);
        }
    }
}