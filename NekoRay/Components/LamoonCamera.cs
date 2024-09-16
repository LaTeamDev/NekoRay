using System.Drawing;
using System.Numerics;

namespace NekoRay; 

[ToolsIcon(MaterialIcons.Videocam)]
public class LamoonCamera : BaseCamera {
    public Size? RenderSize { get; private set; }
    private Size SizeToRender => RenderSize ?? new Size(WindowSettings.Instance.Width, WindowSettings.Instance.Height);

    public float AspectRatio => (float)SizeToRender.Width / SizeToRender.Height;
    
    public Matrix4x4 ProjectionMatrix {
        get {
            if (Orthographic)
                return Matrix4x4.CreateOrthographic(SizeToRender.Width * OrthoScale,
                    SizeToRender.Height * OrthoScale, ZNear, ZFar);
            var aspectRatio = (float) SizeToRender.Width / SizeToRender.Height;
            return Matrix4x4.CreatePerspectiveFieldOfView(
                float.DegreesToRadians(FieldOfView),
                aspectRatio,
                ZNear,
                ZFar
            );
        }
    }

    public Matrix4x4 ViewMatrix {
        get {
            if (Matrix4x4.Invert(
                    Matrix4x4.CreateFromQuaternion(Transform.LocalRotation) *
                    Matrix4x4.CreateTranslation(Transform.LocalPosition), out var mat))
                return mat;
            throw new Exception("Could not calculate ViewMatrix!");
        }
    }


    public bool Orthographic = false;

    public float OrthoScale {
        get => _orthoScale;
        set => _orthoScale = value==0f?0.00001f:value;
    }

    public float FieldOfView = 78f;
    public float ZNear = 0.1f;
    public float ZFar = 1000f;
    private float _orthoScale = 1f;

    public override Vector2 WorldToScreen(Vector3 position) =>
        Vector3.Transform(position, ViewMatrix).ToVector2();


    public override Vector3 ScreenToWorld(Vector2 position) =>
        Vector3.Transform(new Vector3(position, 0), ProjectionMatrix);

    void Draw() {
        CurrentCamera = this;
        using (RenderTexture.Attach()) {
            RlGl.rlDrawRenderBatchActive();
            RlGl.rlMatrixMode(RlGl.RL_PROJECTION);
            RlGl.rlPushMatrix();
            RlGl.rlLoadIdentity();

            var aspect = AspectRatio;
            
            RlGl.rlMultMatrixf(ProjectionMatrix);
            RlGl.rlMatrixMode(RlGl.RL_MODELVIEW);
            RlGl.rlLoadIdentity();
            RlGl.rlMultMatrixf(Transform.GlobalMatrix);
            
            RlGl.rlEnableDepthTest();
            foreach (var gameObject in SceneManager.ActiveScene.GameObjects) {
                gameObject.SendMessage("Render");
            }
            
            RlGl.rlDrawRenderBatchActive();

            RlGl.rlMatrixMode(RlGl.RL_PROJECTION);
            RlGl.rlPopMatrix();

            RlGl.rlMatrixMode(RlGl.RL_MODELVIEW);
            RlGl.rlLoadIdentity();

            RlGl.rlDisableDepthTest();
        }

        CurrentCamera = null;
    }
}