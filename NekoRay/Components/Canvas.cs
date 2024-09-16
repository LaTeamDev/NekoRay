namespace NekoRay; 

public class Canvas : Behaviour {
    void Awake() {
        GameObject.Tags.Add("SkipRender");
    }

    void LateDraw() {
        var cam = BaseCamera.Main;
        using (cam.RenderTexture.Attach()) {
            GameObject.Broadcast("Render");
        }
    }
}