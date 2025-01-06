using Serilog;
using SoLoud;

namespace NekoRay; 

public static class Audio {

    public static SoLoud.SoLoud SoLoud;
    public static SoLoudBackend Backend = SoLoudBackend.Auto;
    public static void Init() {
        if (SoLoud is not null) return;
        SoLoud = new SoLoud.SoLoud();
        Backend = CliOptions.Instance.AudioBackend;
        SoLoud.Init(SoloudFlags.ClipRoundoff, Backend);
        Log.Information("SoLoud {version} started with {backend} backend, {channels} channels, {samplerate} samplerate", SoLoud.Version, SoLoud.Backend, SoLoud.BackendChannels, SoLoud.BackendSampleRate);
    }

    public static void Close() {
        SoLoud.DeInit();
        Log.Information("Successfully shutdown audio");
    }
}