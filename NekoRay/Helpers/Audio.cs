namespace NekoRay; 

public static class Audio {
    public static void Init() {
        Raylib.InitAudioDevice();
    }

    public static void Close() {
        Raylib.CloseAudioDevice();
    }

    public static bool IsReady => Raylib.IsAudioDeviceReady();

    public static float MasterVolume {
        get => Raylib.GetMasterVolume();
        set => Raylib.SetMasterVolume(value);
    }
}