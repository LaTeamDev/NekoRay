using System.Numerics;
using NekoLib.Core;
using NekoRay;
using Serilog;
using Timer = NekoRay.Timer;

namespace HotlineSPonyami.Gameplay; 

public class PlayerInventory : Behaviour {
    public float Capacity;
    private Queue<Carryable> _items = new();
    public Carryable[] Items => _items.ToArray();
    public float Smooth = 0.75f;
    public float Distance = 16f;
    private float Time = 0f;
    private float RotationSpeed = 2f;

    private float _carrying = 0f;
    public float Carrying => _carrying;

    public bool Add(Carryable item) {
        if (_carrying + item.Weight > Capacity)
            return false;
        _carrying += _carrying;
        if (_items.Contains(item)) {
            Log.Warning("Attempt to collect already collected item");
            return false;
        }
        _items.Enqueue(item);
        return true;
    }

    public bool Shoot() {
        return false;
    }

    void Update() {
        Time += Timer.DeltaF;
        var fullPi = Math.PI * 2;
        var slotCount = _items.Count;
        var slotAngle = fullPi / slotCount;
        var up = Vector2.UnitY*(-Distance);
        for (var i = 0; i<Items.Length; i++) {
            var c = Math.Cos(Time*RotationSpeed+slotAngle*i);
            var s= Math.Sin(Time*RotationSpeed+slotAngle*i);
            var curPos = new Vector2((float) (c * up.X - s * up.Y), (float) (s * up.X + c * up.Y));
            var desiredPosition = Transform.Position.ToVector2();
            desiredPosition += curPos;
            Items[i].RB.Position = NekoMath.Damp(Items[i].RB.Position, desiredPosition/ 64f, ref Items[i]._velocity, Smooth);
        }
    }
}