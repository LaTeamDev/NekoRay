namespace NekoRay;

public class ParticleSystemComponent : Behaviour {
    public ParticleSystem ParticleSystem { get; } = new( );

    private Sprite? _sprite;
    public Sprite Sprite {
        get => _sprite!;
        set {
            _sprite = value;
            ParticleSystem.Texture = _sprite.Texture;
            ParticleSystem.Bounds = _sprite.Bounds.ToDotNet();
        }
    }

    public ParticleSystemComponent() {
        Sprite = Texture.Load("textures/_missing_texture.png").ToSprite();
    }
    void Update() {
        ParticleSystem.Position = Transform.Position;
        ParticleSystem.Update(Time.DeltaF);
    }

    void Render() {
        ParticleSystem.Draw();
    }
}