namespace NekoRay; 

[Obsolete("Unfinished")]
public class AudioStream : IPlayable {
    internal RayAudioStream _AudioStream;
    
    public bool IsReady => Raylib.IsAudioStreamReady(_AudioStream);
    public bool IsPlaying => Raylib.IsAudioStreamPlaying(_AudioStream);

    internal AudioStream(RayAudioStream AudioStream) {
        _AudioStream = AudioStream;
    }

    public static AudioStream Load(uint sampleRate, uint sampleSize, uint channels) => 
        new(Raylib.LoadAudioStream(sampleRate, sampleSize, channels));

    public void Dispose() {
        Raylib.UnloadAudioStream(_AudioStream);
    }

    public void Play() =>
        Raylib.PlayAudioStream(_AudioStream);

    public void Stop() =>
        Raylib.StopAudioStream(_AudioStream);

    public void Pause() =>
        Raylib.PauseAudioStream(_AudioStream);

    public void Resume() =>
        Raylib.ResumeAudioStream(_AudioStream);

    private float _volume = 1f;
    public float Volume {
        get => _volume;
        set {
            _volume = value;
            Raylib.SetAudioStreamVolume(_AudioStream, _volume);
        }
    }

    private float _pitch = 1f;
    public float Pitch {
        get => _pitch;
        set {
            _pitch = value;
            Raylib.SetAudioStreamPitch(_AudioStream, _volume);
        }
    }

    private float _pan = 0.5f;
    public float Pan {
        get => _pan;
        set {
            _pan = value;
            Raylib.SetAudioStreamPan(_AudioStream, _volume);
        }
    }
    
    public float? Length => null;
    public float? TimePlayed => null;

    public void Seek(float seconds) {
        //Raylib.SeekAudioStream(_AudioStream, seconds);
    }

    public bool Loop { get; set; } = false;
}