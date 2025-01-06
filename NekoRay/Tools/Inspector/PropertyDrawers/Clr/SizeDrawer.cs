using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace NekoRay.Tools;

[CustomDrawer(typeof(SizeF))]
public class SizeFDrawer : SimpleDrawer<SizeF> {
    protected override bool DrawInput(string label, ref SizeF value) {
        var valueVec = (Vector2) value;
        if (!ImGui.InputFloat2(label, ref valueVec)) return false;
        value = (SizeF) valueVec;
        return true;
    }


    protected override bool DrawRange(string label, ref SizeF value, float min, float max) {
        var valueVec = (Vector2) value;
        if (!ImGui.SliderFloat2(label, ref valueVec, min, max)) return false;
        value = (SizeF) valueVec;
        return true;
    }
}

[CustomDrawer(typeof(Size))]
public class SizeDrawer : SimpleDrawer<Size> {
    protected override unsafe bool DrawInput(string label, ref Size value) {
        int[] valueVec = [value.Width, value.Height];
        fixed(void* val = valueVec)
            if (!ImGui.InputInt2(label, ref Unsafe.AsRef<int>(val))) return false;
        value = new Size(valueVec[0], valueVec[1]);
        return true;
    }


    protected override unsafe bool DrawRange(string label, ref Size value, float min, float max) {
        int[] valueVec = [value.Width, value.Height];
        fixed(void* val = valueVec)
            if (!ImGui.InputInt2(label, ref Unsafe.AsRef<int>(val))) return false;
        value = new Size(valueVec[0], valueVec[1]);
        return true;
    }
}