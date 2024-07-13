namespace NekoRay; 

public class Music : IPlayable {
    internal RayMusic _Music;
    
    public bool IsReady => Raylib.IsMusicReady(_Music);
    public bool IsPlaying => Raylib.IsMusicStreamPlaying(_Music);

    internal Music(RayMusic Music) {
        _Music = Music;
    }

    public static Music Load(string filename) => new(Raylib.LoadMusicStream(filename));

    public void Dispose() {
        Raylib.UnloadMusicStream(_Music);
    }

    public void Play() =>
        Raylib.PlayMusicStream(_Music);

    public void Stop() =>
        Raylib.StopMusicStream(_Music);

    public void Pause() =>
        Raylib.PauseMusicStream(_Music);

    public void Resume() =>
        Raylib.ResumeMusicStream(_Music);

    private float _volume = 1f;
    public float Volume {
        get => _volume;
        set {
            _volume = value;
            Raylib.SetMusicVolume(_Music, _volume);
        }
    }

    private float _pitch = 1f;
    public float Pitch {
        get => _pitch;
        set {
            _pitch = value;
            Raylib.SetMusicPitch(_Music, _volume);
        }
    }

    private float _pan = 0.5f;
    public float Pan {
        get => _pan;
        set {
            _pan = value;
            Raylib.SetMusicPan(_Music, _volume);
        }
    }
    
    public float? Length => Raylib.GetMusicTimeLength(_Music);
    public float? TimePlayed => Raylib.GetMusicTimePlayed(_Music);

    public void Seek(float seconds) {
        Raylib.SeekMusicStream(_Music, seconds);
    }
    
    public bool Loop {
        get => _Music.looping;
        set => _Music.looping = value;
    }

    public void Update() {
        Raylib.UpdateMusicStream(_Music);
    }
}