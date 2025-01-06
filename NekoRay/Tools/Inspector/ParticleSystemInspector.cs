using System.Numerics;
using ImGuiNET;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay.Tools;

[CustomInspector(typeof(ParticleSystemComponent))]
public class ParticleSystemInspector : Inspector {
    public override void DrawGui() {
        var target = (ParticleSystemComponent) Target;
        var ps = target.ParticleSystem;
        
        ImGui.Text($"Active: {ps.Count}/{ps.PoolSize}");
        
        if (ps.Active) ImGui.BeginDisabled();
        if (ImGui.Button("Start")) ps.Start();
        if (ps.Active) ImGui.EndDisabled();
        ImGui.SameLine();
        if (ps.Stopped) ImGui.BeginDisabled();
        if (ImGui.Button("Stop")) ps.Stop();
        if (ps.Stopped) ImGui.EndDisabled();
        ImGui.SameLine();
        if (ps.Paused) ImGui.BeginDisabled();
        if (ImGui.Button("Pause")) ps.Pause();
        if (ps.Paused) ImGui.EndDisabled();
        ImGui.SameLine();
        if (ImGui.Button("Reset")) ps.Reset();
        ImGui.SameLine();
        if (ImGui.Button("Emit")) ps.Emit();
        
        ImGui.SeparatorText("Emitter");
        
        var emitterLifetime = ps.EmitterLifetime;
        if (ImGui.DragFloat("Lifetime", ref emitterLifetime)) {
            ps.EmitterLifetime = emitterLifetime;
        }
        
        var emissionRate = ps.EmissionRate;
        if (ImGui.DragFloat("Emission Rate", ref emissionRate)) {
            ps.EmissionRate = emissionRate;
        }
        
        ImGui.SeparatorText("Particle");
                
        var particleLifetime = ps.ParticleLifetime;
        if (ImGui.DragFloatRange2("Lifetime", ref particleLifetime.Min, ref particleLifetime.Max)) {
            ps.ParticleLifetime = particleLifetime;
        }
        
        var direction = ps.Direction;
        if (ImGui.DragFloat3("Initial Direction", ref direction)) {
            ps.Direction = direction;
        }
                
        var spread = ps.Spread;
        if (ImGui.DragFloat3("Spread", ref spread)) {
            ps.Spread = spread;
        }
        
        var rotation = ps.InitialRotation.YawPitchRollAsVector3();
        if (ImGui.DragFloat3("Initial Rotation", ref rotation)) {
            ps.InitialRotation = rotation.YawPitchRollAsQuaternion();
        }
        
        var speed = ps.Speed;
        if (ImGui.DragFloatRange2("Initial Speed", ref speed.Min, ref speed.Max)) {
            ps.Speed = speed;
        }
        
        var size = ps.Size;
        if (ImGui.DragFloatRange2("Initial Size", ref size.Min, ref size.Max, 0.01f)) {
            ps.Size = size;
        }
        
        ImGui.SeparatorText("Forces");
        
        var acceleration = ps.Acceleration;
        if (ImGui.DragFloat3("Acceleration", ref acceleration)) {
            ps.Acceleration = acceleration;
        }
        
        var spinMin = ps.Spin.Min.YawPitchRollAsVector3();
        var spinMax = ps.Spin.Max.YawPitchRollAsVector3();
        if (ImGui.DragFloat3("Spin Min", ref spinMin)) {
            ps.Spin = ps.Spin with {Min=spinMin.YawPitchRollAsQuaternion()};
        }
        if (ImGui.DragFloat3("Spin Max", ref spinMax)) {
            ps.Spin = ps.Spin with {Max=spinMax.YawPitchRollAsQuaternion()};
        }
        
        var linearDamping = ps.LinearDamping;
        if (ImGui.DragFloat("Linear Damping", ref linearDamping)) {
            ps.LinearDamping = linearDamping;
        }

        var tangent = ps.TangentialAcceleration;
        if (ImGui.DragFloat("Tangential Acceleration", ref tangent)) {
            ps.TangentialAcceleration = tangent;
        }
        
        ImGui.SeparatorText("Display");
        
        var color = ps.Color.ToVector4();
        if (ImGui.ColorEdit4("Color", ref color)) {
            ps.Color = color.ToColor();
        }
        
        var texture = ps.Texture;
        ImGui.Image((nint) texture.Id, new Vector2(64f, 64f));
    }
}