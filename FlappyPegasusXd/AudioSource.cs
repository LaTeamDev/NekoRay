
using System.Numerics;
using Lamoon.Audio;
using Lamoon.Filesystem;
using NekoLib.Core;
using Silk.NET.OpenAL;

namespace Lamoon.Engine.Components; 

public class AudioSource : Behaviour {
    private SoundSource _soundSource = new ();
    private ISoundFile? _track;
    private SoundBuffer _bufferA = new();
    private SoundBuffer _bufferB = new();

    public ISoundFile? Track {
        get => _track;
        
        set {
            _track = value;
            _bufferA.SetData(BufferFormat.Stereo16, _track.Buffer, _track.SampleRate);
            _soundSource.QueueBuffer(_bufferA);
        }
    }

    private string _audioPath;


    void Awake() {
        _prevPos = AudioListener.Position-Transform.Position;
    }

    private Vector3 _prevPos;
    private bool _flip;
    void Update() {
        _soundSource.Position = AudioListener.Position-Transform.Position;
        _soundSource.Velocity = _soundSource.Position - _prevPos;
        if ( Track is null ) return;
        if ( !IsPlaying ) return;
        if ( !Track.Stream ) return;
        var processed = _soundSource.BuffersProcessed;
        while (processed > 0) {
            var curBuf = _flip ? _bufferB : _bufferA;
            processed--;
            _soundSource.UnqueueBuffer(_flip?_bufferA:_bufferB);
            if (Track.GetStreamBuffer(out var streamData) == 0) {
                
                if (!IsLooping) return;
                Track.SeekTo(0);
            }
            _soundSource.QueueBuffer(_flip?_bufferB:_bufferA);
            _flip = !_flip;
        }
    }

    public override void Dispose() {
        base.Dispose();
        _soundSource.Dispose();
        _bufferA.Dispose();
        _bufferB.Dispose();
        _track?.Dispose();
    }

    public void Play() => _soundSource.Play();
    public void Pause() => _soundSource.Pause();
    public void Stop() => _soundSource.Stop();
    public void Rewind() => _soundSource.Rewind();

    public bool IsLooping {
        get => _soundSource.Looping;
        set => _soundSource.Looping = value;
    }
    
    public bool IsRelative {
        get => _soundSource.SourceRelative;
        set => _soundSource.SourceRelative = value;
    }
    
    public float Volume {
        get => _soundSource.Gain;
        set => _soundSource.Gain = value;
    }
    
    public float Pitch {
        get => _soundSource.Pitch;
        set => _soundSource.Pitch = value;
    }
    
    public float DistanceMax {
        get => _soundSource.MaxDistance;
        set => _soundSource.MaxDistance = value;
    }
    
    public float VolumeMax {
        get => _soundSource.MaxGain;
        set => _soundSource.MaxGain = value;
    }
    
    public float DistanceReference {
        get => _soundSource.ReferenceDistance;
        set => _soundSource.ReferenceDistance = value;
    }
    
    public float VolumeMin {
        get => _soundSource.MinGain;
        set => _soundSource.MinGain = value;
    }
    
    public float RolloffFactor {
        get => _soundSource.RolloffFactor;
        set => _soundSource.RolloffFactor = value;
    }
    
    public float TimePlayed {
        get => _soundSource.SecOffset;
        set => _soundSource.SecOffset = value;
    }
    
    public float ConeInnerAngle {
        get => _soundSource.ConeInnerAngle;
        set => _soundSource.ConeInnerAngle = value;
    }
    
    public float ConeOuterAngle {
        get => _soundSource.ConeOuterAngle;
        set => _soundSource.ConeOuterAngle = value;
    }
    
    public float ConeOuterVolume {
        get => _soundSource.ConeOuterGain;
        set => _soundSource.ConeOuterGain = value;
    }
    
    public Vector3 SoundDirection {
        get => _soundSource.Direction;
        set => _soundSource.Direction = value;
    }
    
    public SourceType SourceType => _soundSource.SourceType;

    public SourceState State => _soundSource.SourceState;

    public bool IsPlaying => State == SourceState.Playing;
}