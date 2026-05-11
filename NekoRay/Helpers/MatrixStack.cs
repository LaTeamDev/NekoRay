using System.Numerics;

namespace NekoRay;

public static class MatrixStack {
    public static Stack<Matrix4x4> Stack = new();
    public static Matrix4x4 Current => Stack.Count>0?Stack.Peek():Matrix4x4.Identity;
    public static Vector3 Position = Vector3.Zero;
    public static Quaternion Rotation = Quaternion.Identity;
    public static Vector3 Scale = Vector3.One;

    public static void Push(Matrix4x4 matrix) {
        Stack.Push(Current*matrix);
        if (!Matrix4x4.Decompose(matrix, out Scale, out Rotation, out Position))
            throw new InvalidDataException();
    }
    public static void Pop() {
        Stack.Pop();
    }
}