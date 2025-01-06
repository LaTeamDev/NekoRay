using System.Numerics;
using SoLoud;

namespace NekoRay;

public class AudioPlayer : Behaviour {
    public AudioSource AudioSource;
    
    //NOTE: only latest voice is present
    public Voice? Voice;
    private FakeVoice _fakeVoice = new();

    public void Play() {
        Voice = Type switch {
            AudioType.AudioBackground => Audio.SoLoud.PlayBackground(AudioSource, -1f, true),
            AudioType.Audio2D => Audio.SoLoud.Play(AudioSource, -1f, 0f, true),
            AudioType.Audio3D => Audio.SoLoud.Play3D(AudioSource, Transform.Position, Vector3.Zero, -1f, true),
            _ => throw new ArgumentOutOfRangeException()
        };
        _fakeVoice.CopyTo(ref Voice);
        Voice.IsPaused = IsPaused;
    }
    
    private Vector3 _velocity = Vector3.Zero;
    private Vector3 _prevPosition = Vector3.Zero;
    private Vector3 _dampVelocity = Vector3.Zero;
    private float _smooth = 0.01f;

    void LateUpdate() {
        _velocity = NekoMath.Damp((Transform.Position - _prevPosition)/Time.DeltaF,_velocity, ref _dampVelocity, _smooth);
        _prevPosition = Transform.Position;
        Voice?.Set3dSourceParameters(Transform.Position, _velocity);
    }

    public void Stop() => Voice?.Stop();
    public void StopAll() => Audio.SoLoud.Stop(AudioSource);
    public void Seek(double seconds) => Voice?.Seek(seconds);

    public AudioType Type = AudioType.Audio2D;

    public void SetFilterParameter(uint filter, uint attribute, float value) {
        _fakeVoice.SetFilterParameter(filter, attribute, value);
        Voice?.SetFilterParameter(filter, attribute, value);
    }

    public float GetFilterParameter(uint filter, uint attribute) =>
        Voice?.GetFilterParameter(filter, attribute)??_fakeVoice.GetFilterParameter(filter, attribute);
    
    public void FadeFilterParameter(uint filter, uint attribute, float to, double time) =>
        Voice?.FadeFilterParameter(filter, attribute, to, time);
    
    public void OscillateFilterParameter(uint filter, uint attribute, float from, float to, double time) =>
        Voice?.OscillateFilterParameter(filter, attribute, from, to, time);

    public double StreamTime => Voice?.StreamTime??-1f;
    public double StreamPosition => Voice?.StreamPosition??-1f;
    
    public bool IsPaused {
        get => Voice?.IsPaused ?? _fakeVoice.Paused;
        set {
            if (Voice is not null)
                Voice.IsPaused = value;
            _fakeVoice.Paused = value;
        }
    }

    public float Volume {
        get => Voice?.Volume??_fakeVoice.Volume;
        set {
            if (Voice is not null)
                Voice.Volume = value;
            _fakeVoice.Volume = value;
        }
    }

    public float VolumeOverall => Voice?.VolumeOverall??-1f;
    
    public float Pan {
        get => Voice?.Pan??_fakeVoice.Pan;
        set {
            if (Voice is not null)
                Voice.Pan = value;
            _fakeVoice.Pan = value;
        }
    }

    public void SetPanAbsolute(float lVolume, float rVolume) {
        Voice?.SetPanAbsolute(lVolume, rVolume);
        _fakeVoice.PanAbsoluteL = lVolume;
        _fakeVoice.PanAbsoluteR = rVolume;
    }

    public void SetChannelVolume(int channel, float volume) {
        Voice?.SetChannelVolume(channel, volume);
        _fakeVoice.SetChannelVolume(channel, volume);
    }
    
    public float GetChannelVolume(int channel) {
        return _fakeVoice.GetChannelVolume(channel);
    }

    public float Samplerate {
        get => Voice?.Samplerate??_fakeVoice.Samplerate;
        set {
            if (Voice is not null)
                Voice.Samplerate = value;
            _fakeVoice.Samplerate = value;
        }
    }

    public bool Protected {
        get => Voice?.Protected??_fakeVoice.Protected;
        set {
            if (Voice is not null)
                Voice.Protected = value;
            _fakeVoice.Protected = value;
        }
    }
    
    
    public float RelativePlaySpeed {
        get => Voice?.RelativePlaySpeed??_fakeVoice.RelativePlaySpeed;
        set {
            if (Voice is not null)
                Voice.RelativePlaySpeed = value;
            _fakeVoice.RelativePlaySpeed = value;
        }
    }

    public bool Loop {
        get => Voice?.Loop??_fakeVoice.Loop;
        set {
            if (Voice is not null)
                Voice.Loop = value;
            _fakeVoice.Loop = value;
        }
    }

    public bool AutoStop {
        get => Voice?.AutoStop??_fakeVoice.AutoStop;
        set {
            if (Voice is not null)
                Voice.AutoStop = value;
            _fakeVoice.AutoStop = value;
        }
    }

    public double LoopPoint {
        get => Voice?.LoopPoint??_fakeVoice.LoopPoint;
        set {
            if (Voice is not null)
                Voice.LoopPoint = value;
            _fakeVoice.LoopPoint = value;
        }
    }

    public void SetDelaySamples(uint samples) {
        Voice?.SetDelaySamples(samples);
        _fakeVoice.DelaySamples = samples;
    }

    public void FadeVolume(float to, double time) =>
        Voice?.FadeVolume(to, time);
    
    public void FadePan(float to, double time) =>
        Voice?.FadePan(to, time);
    
    public void FadeRelativePlaySpeed(float to, double time) =>
        Voice?.FadeRelativePlaySpeed(to, time);

    public void SchedulePause(double time) =>
        Voice?.SchedulePause(time);
    
    public void ScheduleStop(double time) =>
        Voice?.ScheduleStop(time);

    public void OscillateVolume(float from, float to, double time) =>
        Voice?.OscillateVolume(from, to, time);
    
    public void OscillatePan(float from, float to, double time) =>
        Voice?.OscillatePan(from, to, time);
    
    public void OscillateRelativePlaySpeed(float from, float to, double time) =>
        Voice?.OscillateRelativePlaySpeed(from, to, time);

    public int LoopCount => Voice?.LoopCount??0;

    public float GetInfo(uint infoKey) =>
        Voice?.GetInfo(infoKey)??float.NaN;

    public void Set3dSourceMinMaxDistance(float minDistance, float maxDistance) {
        Voice?.Set3dSourceMinMaxDistance(minDistance, maxDistance);
        _fakeVoice.SourceMinMaxDistance3D = new Range<float>(minDistance, maxDistance);
    }

    public void Set3dSourceAttenuation(SoLoudAttenuationModel attenuationModel, float attenuationRolloffFactor) {
        Voice?.Set3dSourceAttenuation(attenuationModel, attenuationRolloffFactor);
        _fakeVoice.AttenuationModel = attenuationModel;
        _fakeVoice.AttenuationRollofFactor = attenuationRolloffFactor;
    }

    public void Set3dSourceDopplerFactor(float dopplerFactor) {
        Voice?.Set3dSourceDopplerFactor(dopplerFactor);
        _fakeVoice.SourceDopplerFactor3D = dopplerFactor;
    }
}

public enum AudioType {
    AudioBackground,
    Audio2D,
    Audio3D,
}