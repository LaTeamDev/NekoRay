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

    private static Stack<RenderTexture> _renderTextureStack = new ();

    public AttachMode Attach() {
        _renderTextureStack.Push(this);
        Raylib.BeginTextureMode(_renderTexture);
        return new AttachMode(Detach);
    }

    private void Detach() {
        if (!_renderTextureStack.Contains(this))
            throw new Exception("Huh??? The texture you want to pop isn't even in stack?? wth");
        Raylib.EndTextureMode();
        if (!_renderTextureStack.TryPop(out var renderTexture)) {
            return;
        }
        if (renderTexture != this) {
            throw new Exception("you tried to detach in wrong order");
        } 
        if (_renderTextureStack.TryPeek(out var anotherRenderTexture))
            Raylib.BeginTextureMode(anotherRenderTexture._renderTexture);
    }
}