namespace TowerDefence.Gameplay;

public interface IController {
    public Player Player { get; }
    public void Update();
}

public interface IController<TEntity> : IController where TEntity : Entity {
    public TEntity Controllable { get; }
}