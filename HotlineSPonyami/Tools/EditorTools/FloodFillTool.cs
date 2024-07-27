using ZeroElectric.Vinculum;

namespace HotlineSPonyami.Tools.EditorTools;

public class FloodFillTool : TileTool
{
    public override string Name => "Flood Fill";

    private void Flood(int x, int y, int from, int to)
    {
        if(x < 0 || y < 0 || x >= Scene.Field.SizeX || y >= Scene.Field.SizeY) return;
        if(Scene.Field.GetTile(x, y) != from) return;
        Scene.Field.SetTile(x, y, to);
        Flood(x + 1, y, from, to);
        Flood(x - 1, y, from, to);
        Flood(x, y + 1, from, to);
        Flood(x, y - 1, from, to);
    }
    
    protected override void OnUpdate(int x, int y)
    {
        if (Raylib.IsMouseButtonPressed(0))
        {
            Flood(x, y, Scene.Field.GetTile(x, y), SelectedTexture);
        }
    }
}