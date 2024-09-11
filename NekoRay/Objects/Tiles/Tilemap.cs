using System.Collections;
using System.Drawing;

namespace NekoRay;
 
public struct TilemapEntry {
    public Point Position;
    public Tile? Tile;
 
    internal TilemapEntry(Point point, Tile tile) {
        Position = point;
        Tile = tile;
    }
}

public class Tilemap : NekoObject, IEnumerable<TilemapEntry> {
    public Texture Texture;
    
    public Tilemap(Size size, Texture texture, Size tileSize) {
        _tiles = new Tile[size.Width, size.Height];
        for (var i0 = 0; i0 < _tiles.GetLength(0); i0++)
        for (var i1 = 0; i1 < _tiles.GetLength(1); i1++) {
            _tiles[i0, i1] = new Tile(this, new Point(i0, i1));
        }
        
        Size = size;
        Texture = texture;
        TileSize = tileSize;
    }
 
    private Tile[,] _tiles;
    public readonly Size Size;
    public Size TileSize;
    public IEnumerator<TilemapEntry> GetEnumerator() => new TilemapEnumerator(_tiles);
 
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public ref Tile this[int x, int y] => ref _tiles[x, y];
}
 
public class TilemapEnumerator : IEnumerator<TilemapEntry> {
    private Tile[,] _tiles;
    public TilemapEnumerator(Tile[,] tiles) {
        _tiles = tiles;
    }
    public bool MoveNext() => Cursor++ < _tiles.Length;
 
    public void Reset() => Cursor = 0;
 
    public int Cursor = 0;
    private int CursorX => Cursor % _tiles.GetLength(0);
    private int CursorY => Cursor / _tiles.GetLength(0);
    public Point Cursor2D => new(CursorX, CursorY);
    public TilemapEntry Current => new(Cursor2D, _tiles[Cursor2D.X, Cursor2D.Y]);
 
    object IEnumerator.Current => Current;
 
    public void Dispose() { }
}