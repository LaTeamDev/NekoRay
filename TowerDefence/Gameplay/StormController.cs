using NekoLib.Core;
using NekoRay.Tools;

namespace TowerDefence.Gameplay;

public static class StormController {
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
    public static float TimeNext { get; set; }

    public static event Action? OnPhaseEnded;

    private static void UpdateTimer() {
        if (Time <= 0f) {
            EndWave();
        }
    }
    
    [ConCommand("td_wave_end")]
    [ConTags("cheat")]
    public static void EndWave() {
        OnPhaseEnded?.Invoke();
        Time = TimeNext;
    }
    
    public static void Update() {
         UpdateTimer();
    }
}