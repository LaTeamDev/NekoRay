namespace TowerDefence.Gameplay.Buildings;

public struct BuildingTemplate(string name, string texture, Func<BaseBuilding> create)
{
    public readonly string Name = name;
    public readonly string Texture = texture;
    public readonly Func<BaseBuilding> Create = create;
}

public static class BuildingsList
{
    public static readonly BuildingTemplate[] List = new[]
    {
        new BuildingTemplate( "Reactor", "textures/reactor.png", () => new Rector() ),
        new BuildingTemplate( "Manual Turret", "textures/notexture.png", () => new ManualTurret() )
    };
}