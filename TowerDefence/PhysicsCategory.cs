namespace TowerDefence;

[Flags]
public enum PhysicsCategory
{
    None = 0,
    Default = 1 << 0,
    Player = 1 << 1,
    Enemy = 1 << 3,
    Buildings = 1 << 2,
    Trigger = 1 << 15,

    All = Default | Player | Enemy | Buildings | Trigger
}
