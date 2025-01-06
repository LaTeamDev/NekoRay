using SoLoud;

namespace NekoRay;

internal class FakeVoice {
    private Dictionary<(uint filter, uint attribute), float> _filterParameter = new();
    
    public void SetFilterParameter(uint filter, uint attribute, float value) =>
        _filterParameter[new(filter, attribute)] = value;
    
    public float GetFilterParameter(uint filter, uint attribute) =>
        _filterParameter[new(filter, attribute)];

    public float Volume = 1f;

    public float Pan = 0f;

    public float? PanAbsoluteL;
    public float? PanAbsoluteR;
    
    private Dictionary<int, float> _channelVolume = new();

    public void SetChannelVolume(int channel, float volume) =>
        _channelVolume[channel] = volume;
    
    public float GetChannelVolume(int channel) =>
        _channelVolume[channel];

    public float Samplerate = 44100f;

    public bool Protected = false;

    public float RelativePlaySpeed =1f;

    public bool Loop = false;

    public bool AutoStop = false;

    public double LoopPoint = 0;

    public uint DelaySamples = 0;

    public bool Paused = false;


    public Range<float> SourceMinMaxDistance3D = new(0f, 1000000.0f);

    public SoLoudAttenuationModel AttenuationModel = SoLoudAttenuationModel.NoAttenuation;
    public float AttenuationRollofFactor = 1f;
    public float SourceDopplerFactor3D =1f;

    public void CopyTo(ref Voice voice) {
        foreach (var param in _filterParameter) {
            voice.SetFilterParameter(param.Key.filter, param.Key.attribute, param.Value);
        }

        voice.Volume = Volume;
        voice.Pan = Pan;
        if (PanAbsoluteL is not null && PanAbsoluteR is not null)
            voice.SetPanAbsolute(PanAbsoluteL??0f,  PanAbsoluteR??0f);

        foreach (var param in _channelVolume) {
            voice.SetChannelVolume(param.Key, param.Value);
        }
        
        voice.Samplerate = Samplerate;
        voice.Protected = Protected;
        voice.RelativePlaySpeed = RelativePlaySpeed;
        voice.Loop = Loop;
        //voice.AutoStop = AutoStop;
        voice.LoopPoint = LoopPoint;
        voice.SetDelaySamples(DelaySamples);
        voice.Set3dSourceMinMaxDistance(SourceMinMaxDistance3D.Min, SourceMinMaxDistance3D.Max);
        voice.Set3dSourceAttenuation(AttenuationModel, AttenuationRollofFactor);
        voice.Set3dSourceDopplerFactor(SourceDopplerFactor3D);
    }

}