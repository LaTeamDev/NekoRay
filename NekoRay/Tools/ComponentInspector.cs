using System.Numerics;
using System.Reflection;
using ImGuiNET;
using NekoLib;
using NekoLib.Core;
using Serilog;

namespace NekoRay.Tools; 

[CustomInspector(typeof(Component))]
public class ComponentInspector : Inspector {
    public override void DrawGui() {
        var target = (Component) Target;
        foreach (var field in Target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)) {
            var name = field.Name + "##" + target.Id;
            if (field.FieldType == typeof(Vector3)) {
                var vec = (Vector3)field.GetValue(Target);
                ImGui.InputFloat3(name, ref vec);
                field.SetValue(Target, vec);
            } else if (field.FieldType == typeof(Vector2)) {
                var vec = (Vector2) field.GetValue(Target);
                ImGui.InputFloat2(name, ref vec);
                field.SetValue(Target, vec);
            }
            else if (field.FieldType == typeof(bool)) {
                var vec = (bool)field.GetValue(Target);
                ImGui.Checkbox(name, ref vec);
                field.SetValue(Target, vec);
            } else if (field.FieldType == typeof(float)) {
                var vec = (float)field.GetValue(Target);
                ImGui.InputFloat(name, ref vec);
                field.SetValue(Target, vec);
            } else if (field.FieldType == typeof(double)) {
                var vec = (double)field.GetValue(Target);
                ImGui.InputDouble(name, ref vec);
                field.SetValue(Target, vec);
            } else if (field.FieldType == typeof(int)) {
                var vec = (int)field.GetValue(Target);
                ImGui.InputInt(name, ref vec);
                field.SetValue(Target, vec);
            } else if (field.FieldType.IsAssignableTo(typeof(Component))) {
                ImGui.TextDisabled(field.Name+": "+field.GetValue(Target));
            }
        }

        var properties = Target.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        foreach (var field in properties) {
            if (field.SetMethod is null) ImGui.BeginDisabled();
            
            if (field.GetMethod is null) {
                ImGui.TextDisabled(field.Name+": No get method");
                return;
            }
            
            var value = field.GetValue(Target);
            
            if (field.PropertyType == typeof(Vector3)) {
                var vec = (Vector3) value;
                ImGui.InputFloat3(field.Name, ref vec);
                value = vec; 
            } else if (field.PropertyType == typeof(bool)) {
                var vec = (bool) value;
                ImGui.Checkbox(field.Name, ref vec);
                value = vec;
            } else if (field.PropertyType == typeof(Vector2)) {
                var vec = (Vector2)value;
                ImGui.InputFloat2(field.Name, ref vec);
                value = vec;
            } else if (field.PropertyType == typeof(float)) {
                var vec = (float)value;
                ImGui.InputFloat(field.Name, ref vec);
                value = vec;
            } else if (field.PropertyType == typeof(double)) {
                var vec = (double)value;
                ImGui.InputDouble(field.Name, ref vec);
                value = vec;
            } else if (field.PropertyType == typeof(int)) {
                var vec = (int)value;
                ImGui.InputInt(field.Name, ref vec);
                value = vec;
            } else if (field.PropertyType.IsAssignableTo(typeof(Component))) {
                ImGui.TextDisabled(field.Name+": "+(field.GetValue(Target)??"null"));
            } else if (field.PropertyType == typeof(Quaternion)) {
                var vec = (Quaternion)value;
                var euler = vec.GetEulerAngles();
                ImGui.InputFloat3(field.Name, ref euler);
                value = Quaternion.CreateFromYawPitchRoll(euler.X, euler.Y, euler.Z);
            }
            
            if (field.SetMethod is null) {ImGui.EndDisabled(); continue;}
            field.SetValue(Target, value);
        }
    }
}