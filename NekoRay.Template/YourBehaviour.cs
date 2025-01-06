using NekoLib.Core;
using ZeroElectric.Vinculum;

namespace NekoRay.Template;

public class YourBehaviour : Behaviour {
    void Awake() {
        // On component initialization (inside Scene.Initialize())
    }

    void Start() {
        // On first frame after this component is created and enabled
    }

    void Update() {
        // Every frame
    }

    void FixedUpdate() {
        // Every 1/60 of a second, used e.g. for physics
    }

    void LateUpdate() {
        // Every frame after each component have run their Update()
    }
    
    void Render() {
        // OnCameraDraw
        Raylib.DrawText("Hello world!", 0, 0, 11, Raylib.GRAY);
    }
}