using NekoLib.Core;
using NekoRay.Tools;

namespace TowerDefence.Gameplay;

public static class StormController {
    [ConCommand("td_start")]
    public static void Start() => _started = true;
    
    private static bool _started;
    
    [ConVariable("td_gamestarted")] 
    public static bool Started => _started;
    
    [ConVariable("td_stormcount")]
    [ConTags("cheat")]
    public static int Count { get; set; }
    
    [ConVariable("td_storm")]
    [ConTags("cheat")]
    public static bool IsInStorm { get; set; }
    
    [ConVariable("td_time")]
    [ConTags("cheat")]
    public static float Time { get; set; }
    
    [ConVariable("td_time_max")]
    [ConTags("cheat")]
    public static float TimeMax { get; set; }

    [ConVariable("td_time_next")]
    [ConTags("cheat")]
    public static float TimeNext { get; set; } = 60f;

    public static event Action? OnPhaseEnded;

    private static void UpdateTimer() {
        Time -= NekoRay.Time.DeltaF;
        if (Time <= 0f) {
            EndWave();
        }
    }
    
    [ConCommand("td_wave_end")]
    [ConTags("cheat")]
    public static void EndWave() {
        OnPhaseEnded?.Invoke();
        Time = TimeNext;
        TimeMax = TimeNext;
    }
    
    public static void Update() {
        if (!Started) return;
         UpdateTimer();
    }
}