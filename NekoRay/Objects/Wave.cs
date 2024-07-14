using ZeroElectric.Vinculum.Extensions;

namespace NekoRay; 

public class Wave : NekoObject {
    internal RayWave _wave;

    internal Wave(RayWave wave) {
        _wave = wave;
    }

    public static Wave Load(string filename) => new(Raylib.LoadWave(filename));
    
    // TODO: public static Wave LoadFromMemory(string filetype)

    public bool IsReady => Raylib.IsWaveReady(_wave);

    public override void Dispose() {
        Raylib.UnloadWave(_wave);
    }

    public uint Channels => _wave.channels;
    public uint SampleRate => _wave.sampleRate;
    public uint SampleSize => _wave.sampleSize;

    public Sound ToSound() {
        return new(Raylib.LoadSoundFromWave(_wave));
    }

    public bool Export(string filename) {
        return Raylib.ExportWave(_wave, filename);
    }

    public Wave Copy() {
        return new(Raylib.WaveCopy(_wave));
    }

    public unsafe void Crop(int initSample, int finalSample) {
        var wave = _wave.GcPin();
        Raylib.WaveCrop((RayWave*)wave.AddrOfPinnedObject(), initSample, finalSample);
        wave.Free();
    }

    public void Crop(float startSecond, int endSecond) {
        Crop((int)(startSecond * SampleRate), (int)(endSecond * SampleRate));
    }

    public unsafe void SetFormat(int newSampleRate, int newSampleSize, int newChannels) {
        var wave = _wave.GcPin();
        Raylib.WaveFormat((RayWave*)wave.AddrOfPinnedObject(), newSampleRate, newSampleRate, newChannels);
        wave.Free();
    }
}