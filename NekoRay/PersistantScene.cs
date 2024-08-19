namespace NekoRay; 

public class PersistantScene : BaseScene {
    public override string Name => "DontDestroyOnLoad";

    public PersistantScene() {
        DestroyOnLoad = false;
    }
}