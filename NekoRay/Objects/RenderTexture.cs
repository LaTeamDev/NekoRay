namespace NekoRay; 

public class RenderTexture : NekoObject, IDisposable {
    internal RayRenderTexture _renderTexture;
    internal RenderTexture() { }

    public Texture Texture { get; internal set; }
    public Texture DepthTexture { get; internal set; }

    public uint TextureId => _renderTexture.id;

    public static RenderTexture Load(int width, int height) {
        var rt =  new RenderTexture {
            _renderTexture = Raylib.LoadRenderTexture(width, height)
        };
        rt.Texture = new Texture {
            _texture = rt._renderTexture.texture
        };
        rt.DepthTexture = new Texture {
            _texture = rt._renderTexture.depth
        };
        return rt;
    }

    public bool IsReady => Raylib.IsRenderTextureReady(_renderTexture);

    public override void Dispose() {
        Raylib.UnloadRenderTexture(_renderTexture);
    }

    public AttachMode Attach() {
        return new AttachMode(this);
    }

    public class AttachMode : IDisposable {
        internal RenderTexture _renderTexture;

        internal AttachMode(RenderTexture renderTexture) {
            _renderTexture = renderTexture;
            Raylib.BeginTextureMode(_renderTexture._renderTexture);
        }
        
        public void Dispose() {
            Raylib.EndTextureMode();
        }
    }
}