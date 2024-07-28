using ZeroElectric.Vinculum;

namespace HotlineSPonyami.Tools.EditorTools;

public class LineWallTool : BaseWallTool
{
    private int startX, startY;
    
    public override string Name => "Line Wall Tool";
    public override void OnDraw()
    {
        Scene.GetCurrentCorner(out int x, out int y);

        if (Raylib.IsMouseButtonPressed(0))
        {
            startX = x;
            startY = y;
        }
        
        int endX;
        int endY;
        if (Math.Abs(startX - x) > Math.Abs(startY - y))
        {
            endY = startY;
            endX = x;
        }
        else
        {
            endY = y;
            endX = startX;
        }

        if (startX > endX)
        {
            (endX, startX) = (startX, endX);
        }
        if (startY > endY)
        {
            (endY, startY) = (startY, endY);
        }

        if (Raylib.IsMouseButtonDown(0))
        {
            DrawCorner(startX, startY, new Color(0,255,0,100));
            Raylib.DrawLine(startX, startY, endX, endY, Raylib.GREEN);
            DrawCorner(endX, endY, new Color(0,255,0,100));
        }

        if (Raylib.IsMouseButtonReleased(0))
        {
            Scene.Field.SetLine(startX, startY, endX, endY, SelectedTexture);
        }
        
        DrawCorner(x, y, new Color(100,125,100,100));
    }

    public override void OnUpdate()
    {
        
    }
}