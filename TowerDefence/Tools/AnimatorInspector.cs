using ImGuiNET;
using NekoRay.Tools;
using TowerDefence.Objects;

namespace TowerDefence.Tools;

[CustomInspector(typeof(Animator2D))]
public class AnimatorInspector : Inspector {
    public override void DrawGui() {
        if (Target is null) return;
        var target = (Animator2D)Target;
        if (ImGui.BeginCombo("Animation", target.AnimationName)) {
            foreach (var animation in target.Animation.Animations) {
                var selected = animation.Key == target.AnimationName;
                if (ImGui.Selectable(animation.Key, selected)) {
                    target.AnimationName = animation.Key;
                }

                // Set the initial focus when opening the combo (scrolling + keyboard navigation focus)
                if (selected)
                    ImGui.SetItemDefaultFocus();
            }
            ImGui.EndCombo();
        }
        ImGui.DragFloat("Animation Speed", ref target.AnimationSpeed, 0.01f, 0f);
        if (ImGui.CollapsingHeader("Debug")) {
            ImGui.BeginDisabled();
            var time = (float)target.GetType().GetField("_animationTime").GetValue(target);
            ImGui.InputFloat("Time", ref time);
            ImGui.InputInt("Animation Frame", ref target.CurrentAnimationIndex, 0);
            var frame = target.CurrentAnimation.Frames[target.CurrentAnimationIndex];
            ImGui.InputInt("Current Frame index", ref frame, 0);
            ImGui.EndDisabled();
        }
    }
}