namespace TowerDefence.Gameplay;

public interface Usable
{
    public void Use(Player player);
    public bool CanUse(Player player);
}