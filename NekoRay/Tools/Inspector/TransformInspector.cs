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
        ImGui.InputFloat3("Position", ref pos);
        if (pos != target.LocalPosition) target.LocalPosition = pos;
        var scale = target.LocalScale;
        ImGui.InputFloat3("Scale", ref scale);
        if (scale != target.LocalScale) target.LocalScale = scale;
        var rot = target.LocalRotation;
        var rotvec = rot.GetEulerAngles();
        ImGui.SliderFloat3("Rotation", ref rotvec, -MathF.PI/2, MathF.PI/2);
        rot = Quaternion.CreateFromYawPitchRoll(rotvec.X, rotvec.Y, rotvec.Z);
        if (rot != target.LocalRotation) target.LocalRotation = rot;
    }
}