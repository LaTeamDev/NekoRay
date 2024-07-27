using System.Numerics;
using HotlineSPonyami.Tools;
using NekoLib.Core;
using NekoRay;
using ZeroElectric.Vinculum;
using Shader = NekoRay.Shader;
using Texture = NekoRay.Texture;

namespace HotlineSPonyami;

public class TiledScene : BaseScene
{
    private Texture _uvTexture;
    private Texture _tileAtlas;
    private Shader _mapShader;
    
    private DragCamera _camera;
    
    public TiledScene()
    {
        _mapShader = Data.GetShader("data/shaders/field_shader.frag", "data/shaders/field_shader.vert");
        _tileAtlas = Data.GetTexture("data/textures/texture_atlas.png");
        _uvTexture = Data.GetTexture("data/test_map.png");
        _tileAtlas.Filter = TextureFilter.TEXTURE_FILTER_POINT;
        _uvTexture.Filter = TextureFilter.TEXTURE_FILTER_POINT;
        _tileAtlas.GenMipmaps();
        _uvTexture.GenMipmaps();
    }
    public override void Initialize() {
        _camera = new GameObject("DragCamera").AddComponent<DragCamera>();
        base.Initialize();
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.BeginMode2D(_camera.Camera);
        using (_mapShader.Attach())
        {
            _mapShader.SetTexture("uvMap", _uvTexture);
            _mapShader.SetVector2("mapSize", new Vector2(_uvTexture.Width, _uvTexture.Height));
            _mapShader.SetInt("atlasWidth", _tileAtlas.Width);
            Rectangle source = new Rectangle(0, 0, _tileAtlas.Width, _tileAtlas.Height);
            Rectangle destination = new Rectangle(0, 0, _uvTexture.Width * 32, _uvTexture.Height * 32);
            _tileAtlas.Draw(source, destination, Vector2.Zero, 0f, Raylib.WHITE);
        }
        Raylib.EndMode2D();
    }
}