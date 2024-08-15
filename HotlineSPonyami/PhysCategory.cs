namespace HotlineSPonyami;

[Flags]
public enum PhysCategory {
    LevelGeometry = 1 << 0,
    Entity = 1 << 2,
    Player = 1 << 3,
    Enemy = 1 << 4,
    Prop = 1 << 5,
    Attackable = 1 << 6,
    Trigger = 1 << 15,
    All = ushort.MaxValue
}