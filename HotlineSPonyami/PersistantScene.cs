using NekoRay;

namespace HotlineSPonyami; 

public class PersistantScene : BaseScene {
    public override string Name => "DontDestroyOnLoad";

    public PersistantScene() {
        DestroyOnLoad = false;
    }
}