using System.Reflection;
using ImGuiNET;

namespace NekoRay.Tools;

public abstract class SimpleDrawer<T> : Drawer {
    protected abstract bool DrawInput(string label, ref T value);

    protected virtual bool DrawRange(string label, ref T value, float min, float max) =>
        DrawInput(label, ref value);

    public override void DrawGui(MemberInfo info, object? obj)
    {
        base.DrawGui(info, obj);
        switch (info.MemberType) {
            case MemberTypes.Property:
                DrawGui((PropertyInfo)info, obj);
                break;
            case MemberTypes.Field:
                DrawGui((FieldInfo)info, obj);
                break;
            default: throw new Exception("I can't draw anything besides properties and fields, derive from Drawer<T>");
        }
    }

    public virtual void DrawNull(MemberInfo info) {
        ImGui.TextDisabled($"{info.Name}: null");
    }
    
    public virtual void DrawGui(FieldInfo info, object? obj)
    {
        if (obj == null) DrawNull(info);

        var value = (T)info.GetValue(obj);
        var range = info.GetCustomAttribute<RangeAttribute>();
        if (range is null) {
            if (DrawInput(info.Name, ref value))
            {
                info.SetValue(obj, value);
            }
            return;
        }
        
        if (DrawRange(info.Name, ref value, range.Min, range.Max))
        {
            info.SetValue(obj, value);
        }
    }
    //this is basically the same implementation and i'm sure it is possible to fix it
    public virtual void DrawGui(PropertyInfo info, object? obj) {
        var disabled = info.GetSetMethod() is null;
        if (disabled) ImGui.BeginDisabled();
        if (obj == null) DrawNull(info);

        var value = (T)info.GetValue(obj);
        var range = info.GetCustomAttribute<RangeAttribute>();
        if (range is null) {
            if (DrawInput(info.Name, ref value))
            {
                info.SetValue(obj, value);
            }
            if (disabled) ImGui.EndDisabled();
            return;
        }
        
        if (DrawRange(info.Name, ref value, range.Min, range.Max))
        {
            info.SetValue(obj, value);
        }
        if (disabled) ImGui.EndDisabled();
    }
}