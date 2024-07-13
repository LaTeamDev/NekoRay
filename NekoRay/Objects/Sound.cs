namespace NekoRay; 

public class Sound : IPlayable {
    internal RaySound _sound;

    public bool IsAlias { get; private set; } = false;
    public bool IsReady => Raylib.IsSoundReady(_sound);
    public bool IsPlaying => Raylib.IsSoundPlaying(_sound);

    internal Sound(RaySound sound) {
        _sound = sound;
    }

    public static Sound Load(string filename) => new(Raylib.LoadSound(filename));

    public Sound CreateAlias() => new(Raylib.LoadSoundAlias(_sound)) {
        IsAlias = true
    };

    public void Dispose() {
        if (IsAlias) Raylib.UnloadSoundAlias(_sound);
        else Raylib.UnloadSound(_sound);
    }

    public void Play() =>
        Raylib.PlaySound(_sound);

    public void Stop() =>
        Raylib.StopSound(_sound);

    public void Pause() =>
        Raylib.PauseSound(_sound);

    public void Resume() =>
        Raylib.ResumeSound(_sound);

    private float _volume = 1f;
    public float Volume {
        get => _volume;
        set {
            _volume = value;
            Raylib.SetSoundVolume(_sound, _volume);
        }
    }

    private float _pitch = 1f;
    public float Pitch {
        get => _pitch;
        set {
            _pitch = value;
            Raylib.SetSoundPitch(_sound, _volume);
        }
    }

    private float _pan = 0.5f;
    public float Pan {
        get => _pan;
        set {
            _pan = value;
            Raylib.SetSoundPan(_sound, _volume);
        }
    }
    
    public float? Length => null;
    public float? TimePlayed => null;

    public bool Loop { get; set; } = false;
}