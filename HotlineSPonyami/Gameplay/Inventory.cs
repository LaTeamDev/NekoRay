using NekoLib.Core;
using NekoRay;

namespace HotlineSPonyami.Gameplay; 

public class PlayerInventory : Behaviour {
    public float Capacity;
    private Queue<Carryable> _items = new();
    public Carryable[] Items => _items.ToArray();

    private float _carrying = 0f;
    public float Carrying => _carrying;

    public bool Add(Carryable item) {
        if (_carrying + item.Weight > Capacity)
            return false;
        _carrying += _carrying;
        _items.Enqueue(item);
        return true;
    }

    void Update() {
        foreach (var carryable in _items) {
            carryable.Transform.Position = NekoMath.Damp(carryable.Transform.Position, Transform.Position, ref carryable._velocity, 0.1f);
        }
    }
}