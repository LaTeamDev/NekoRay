namespace NekoRay;

public class TilemapRenderer2D {
    public Tilemap Map;

    void Render() {
        foreach (var entry in Map) {
            if (entry.Tile is null) continue;
            var pos = entry.Position.ToVector2()*Map.TileSize.ToVector2();
            //var sourceStart = (entry.Tile.TextureIndex * Map.TileSize.Width)%Map.Texture.Width
            throw new NotImplementedException();
            //Raylib.DrawTexturePro(Map.Texture, );
        }
    }
}