using System.Drawing;

namespace NekoRay;

public class Tile {
    public Tilemap Tilemap;
    public int TextureIndex;
    public Point Position;

    internal Tile(Tilemap tilemap, Point position) {
        Tilemap = tilemap;
        Position = position;
    }
}