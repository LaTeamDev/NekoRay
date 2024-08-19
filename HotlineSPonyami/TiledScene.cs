using System.Numerics;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;
using Image = NekoRay.Image;
using Texture = NekoRay.Texture;

namespace HotlineSPonyami;

public class TiledScene : BaseScene
{
    private Texture _uvTexture;
    private Texture _tileAtlas;
    private Texture _wallsAtlas;
    
    private Camera2D _camera;
    
    public TiledScene()
    {
        _tileAtlas = Data.GetTexture("textures/texture_atlas.png");
        _wallsAtlas = Data.GetTexture("textures/walls_atlas.png");
        _uvTexture = Data.GetTexture("test_map.png");
        _tileAtlas.Filter = TextureFilter.TEXTURE_FILTER_POINT;
        _uvTexture.Filter = TextureFilter.TEXTURE_FILTER_POINT;
        _wallsAtlas.Filter = TextureFilter.TEXTURE_FILTER_POINT;
        _tileAtlas.GenMipmaps();
        _uvTexture.GenMipmaps();
        _wallsAtlas.GenMipmaps();
    }
    public override void Initialize() {
        this.CreateWorld();
        _camera = new GameObject("Camera Cam").AddComponent<Camera2D>();
        _camera.IsMain = true;
        _camera.Transform.Position += Vector3.One * 75;
        
        TileRenderer renderer = new GameObject("Tile Renderer").AddComponent<TileRenderer>();
        renderer.UvTexture = _uvTexture;
        Image img = Image.FromTexture(_uvTexture);
        
        
        for (int x = 0; x < _uvTexture.Width; x++)
        {
            for (int y = 0; y < _uvTexture.Height; y++)
            {
                if (img.GetColorAt(x, y).g > 0)
                {
                    Rigidbody2D wall = new GameObject($"Wall_{x}_{y}").AddComponent<Rigidbody2D>();
                    PolygonCollider wallCollider = wall.GameObject.AddComponent<PolygonCollider>();
                    //wallCollider.Filter = new Filter();
                    //wallCollider.Filter.categoryBits = 1;
                    //wallCollider.SetAsBox(7f / Physics.MeterScale / 2f, 32f / Physics.MeterScale / 2f);
                    wall.Transform.Position = new Vector3(x * 32 + 7f / 2f, y * 32 + 32f / 2f, 0);
                }
                if (img.GetColorAt(x, y).b > 0)
                {
                    Rigidbody2D wall = new GameObject($"Wall_{x}_{y}").AddComponent<Rigidbody2D>();
                    PolygonCollider wallCollider = wall.GameObject.AddComponent<PolygonCollider>();
                    //wallCollider.Filter = new Filter();
                    //wallCollider.Filter.categoryBits = 1;
                    //wallCollider.SetAsBox(32f / Physics.MeterScale / 2f, 7f / Physics.MeterScale / 2f);
                    wall.Transform.Position = new Vector3(x * 32 + 32f / 2f, (y + 1) * 32 - 7f / 2f, 0);
                }
            }
        }
        base.Initialize();
    }
}