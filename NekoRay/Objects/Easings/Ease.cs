using System.Data;
using System.Reflection;
using System.Numerics;

namespace NekoRay.Easings;

public class Ease {
    public float TimeMax { get; set; }
    public float TimeNow { get; set; }
    
    public float Factor => TimeNow / TimeMax;
    public IEasing Easing = new EaseLinear();
    public Getter GetterFunc;
    public Setter SetterFunc;
    
    public Action? AfterFunc;
    
    private static List<Ease> _list = new();
    
    public float Start;
    public float End;
    
    internal Ease(Getter from, Setter setter, float time, float to) {
        TimeMax = time;
        GetterFunc = from;
        SetterFunc = setter;
        End = to;
        Start = from();
    }

    public void Update(float dt) {
        TimeNow += dt;
        SetterFunc(Start + (End-Start)*Easing.Eval(Factor));
    }
    
    
    public Ease SetEasing(IEasing easing) {
        Easing = easing;
        return this;
    }

    public delegate float Getter();
    public delegate void Setter(float value);
    
    public static Ease To(Getter from, Setter setter, float time, float to) {
        var a = new Ease(from, setter, time, to);
        _list.Add(a);
        return a;
    }
    
    public Ease After(Action a) {
        AfterFunc = a;
        return this;
    }

    public bool Finished = false;

    public static void UpdateAll(float dt) {
        var thisFrame = new Ease[_list.Count];
        _list.CopyTo(thisFrame);
        foreach (var ease in thisFrame) {
            if (ease.TimeMax < ease.TimeNow) {
                ease.Stop();
                continue;
            }
            ease.Update(dt);
        }
    }

    public static void Stop(Ease ease) {
        Cancel(ease);
        ease.AfterFunc?.Invoke();
    }

    public static void CancelAll() {
        var thisFrame = new Ease[_list.Count];
        _list.CopyTo(thisFrame);
        foreach (var ease in thisFrame) {
            ease.Cancel();
        }
    }
    
    public static void Cancel(Ease ease) {
        if (ease.Finished) return;
        ease.Finished = true;
        _list.Remove(ease);
        ease.SetterFunc(ease.End);
    }

    public void Stop() => Stop(this);
    public void Cancel() => Cancel(this);
}