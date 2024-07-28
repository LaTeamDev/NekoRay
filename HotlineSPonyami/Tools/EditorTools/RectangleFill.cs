using ZeroElectric.Vinculum;

namespace HotlineSPonyami.Tools.EditorTools;

public class RectangleFill : TileTool
{
    public override string Name => "Rectangle Fill";
    
    private int _startX, _startY;

    protected override void OnUpdate(int x, int y)
    {
        if (Raylib.IsMouseButtonPressed(0))
        {
            _startX = x;
            _startY = y;
        }

        int width = x - _startX;
        int height = y - _startY;
        
        if (Raylib.IsMouseButtonReleased(0))
        {
            int minX = x > _startX ? _startX : x;
            int minY = y > _startY ? _startY : y;
            int maxX = x > _startX ? x : _startX;
            int maxY = y > _startY ? y : _startY;

            for (int tileX = minX; tileX <= maxX; tileX++)
            {
                for (int tileY = minY; tileY <= maxY; tileY++)
                {
                    Scene.Field.SetTileFloor(tileX, tileY, SelectedTexture);
                }
            }
        }
    }

    protected override void OnDraw(int x, int y)
    {
        if (!Raylib.IsMouseButtonDown(0))
        {
            base.OnDraw(x, y);
            return;
        }
        int width = x - _startX + 1;
        int height = y - _startY + 1;
        Raylib.DrawRectangleLines(_startX * TileField.TextureSize, _startY * TileField.TextureSize, TileField.TextureSize * width, TileField.TextureSize * height, Raylib.RED);
        base.OnDraw(x, y);
    }
}