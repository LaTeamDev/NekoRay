using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NekoRay; 

public static class Timer {
    private static Stopwatch _sw = new();
    
    public static double Step() { 
        if (!_sw.IsRunning) 
            _sw.Start();
        Delta = _sw.Elapsed.TotalSeconds;
        Time += Delta;
        _sw.Restart();
        return Delta;
    }

    public static double Time { get; private set; }
    public static double Delta { get; private set; }
    
    public static float DeltaF => (float)Delta;
    public static float Fps => 1 / DeltaF;
}