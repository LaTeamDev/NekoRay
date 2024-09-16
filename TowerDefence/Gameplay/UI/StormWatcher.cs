using NekoLib.Core;
using TowerDefence.UI;

namespace TowerDefence.Gameplay.UI;

public class StormWatcher : Behaviour {
    public ProgressBar Bar;

    void Awake() {
        Bar = GameObject.GetComponent<ProgressBar>();
    }

    void Update() {
        Bar.Progress = StormController.Time / StormController.TimeMax;
    }
}