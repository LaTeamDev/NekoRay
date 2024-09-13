using System.Drawing;

namespace TowerDefence.Objects;

public class Frame {
    public int Width { get; set; }
    public int Height { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    
    public Size Size => new(Width, Height);
    public Point Position => new(X, Y);
}