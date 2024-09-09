using System.Numerics;
using Box2D;
using ImGuiNET;
using NekoLib;
using NekoLib.Core;

namespace NekoRay.Tools;

[CustomInspector(typeof(NekoLib.Core.Transform))]
public class TransformInspector : Inspector {
    public override void DrawGui() {
        var target = (NekoLib.Core.Transform) Target;
        var pos = target.LocalPosition;
        ImGui.DragFloat3("Position", ref pos);
        if (pos != target.LocalPosition) target.LocalPosition = pos;
        var scale = target.LocalScale;
        if (ImGui.DragFloat3("Scale", ref scale))
        if (scale != target.LocalScale) target.LocalScale = scale;
        var rot = target.LocalRotation;
        var rotvec = rot.GetEulerAngles();
        if (ImGui.SliderFloat3("Rotation", ref rotvec, -MathF.PI / 2, MathF.PI / 2)) {
            rot = Quaternion.CreateFromYawPitchRoll(rotvec.X, rotvec.Y, rotvec.Z);
            target.LocalRotation = rot;
        }
    }
}