using System.Numerics;
using NekoLib.Core;
using ZeroElectric.Vinculum;
using Shader = NekoRay.Shader;
using Texture = NekoRay.Texture;

namespace HotlineSPonyami;

public class TileRenderer : Behaviour
{
    private Shader _mapShader;
    private Shader _wallShader;
    
    public Texture UvTexture;
    private Texture _tileAtlas;
    private Texture _wallsAtlas;

    void Start()
    {
        _tileAtlas = Data.GetTexture("textures/texture_atlas.png");
        _wallsAtlas = Data.GetTexture("textures/walls_atlas.png");
        _mapShader = Data.GetShader("shaders/field_shader.frag", "shaders/field_shader.vert");
        _wallShader = Data.GetShader("shaders/wall_shader.frag", "shaders/wall_shader.vert");
    }
    
    void Render()
    {
        using (_mapShader.Attach())
        {
            _mapShader.SetTexture("uvMap", UvTexture);
            _mapShader.SetVector2("mapSize", new Vector2(UvTexture.Width, UvTexture.Height));
            _mapShader.SetInt("atlasWidth", _tileAtlas.Width);
            Rectangle source = new Rectangle(0, 0, _tileAtlas.Width, _tileAtlas.Height);
            Rectangle destination = new Rectangle(0, 0, UvTexture.Width * 32, UvTexture.Height * 32);
            _tileAtlas.Draw(source, destination, Vector2.Zero, 0f, Raylib.WHITE);
        }
        using (_wallShader.Attach())
        {
            _wallShader.SetTexture("uvMap", UvTexture);
            _wallShader.SetVector2("mapSize", new Vector2(UvTexture.Width, UvTexture.Height));
            _wallShader.SetInt("atlasWidth", _wallsAtlas.Width);
            Rectangle source = new Rectangle(0, 0, _wallsAtlas.Width, _wallsAtlas.Height);
            Rectangle destination = new Rectangle(0, 0, UvTexture.Width * 32, UvTexture.Height * 32); 
            _wallsAtlas.Draw(source, destination, Vector2.Zero, 0f, Raylib.WHITE);
        }
    }
}