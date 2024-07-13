using System.Numerics;
using ZeroElectric.Vinculum;

namespace NekoRay; 

public class Shader : IDisposable {
    private RayShader _shader;
    internal Shader() { }

    public Shader(string fragCode, string? vertCode = null) {
        _shader = Raylib.LoadShaderFromMemory(vertCode, fragCode);
    }

    public static Shader FromFiles(string fragPath, string? vertPath = null) {
        return new Shader {
            _shader = Raylib.LoadShader(vertPath, fragPath)
        };
    }

    public bool IsReady => Raylib.IsShaderReady(_shader);

    public int GetLocation(string uniformName) {
        return Raylib.GetShaderLocation(_shader, uniformName);
    }

    public int GetAttributeLocation(string attributeName) {
        return Raylib.GetShaderLocationAttrib(_shader, attributeName);
    }

    public void SetValue<T>(int uniformLocation, ref T value) where T : unmanaged {
        throw new NotImplementedException();
        switch (value) {
            case Byte or Int16 or Int32 or Int64 or Int128:
                Raylib.SetShaderValue(_shader, uniformLocation, value, ShaderUniformDataType.SHADER_UNIFORM_INT);
                break;
            case float:
                Raylib.SetShaderValue(_shader, uniformLocation, value, ShaderUniformDataType.SHADER_UNIFORM_FLOAT);
                break;
            case Vector2:
                Raylib.SetShaderValue(_shader, uniformLocation, value, ShaderUniformDataType.SHADER_UNIFORM_VEC2);
                break;
            case Vector3:
                Raylib.SetShaderValue(_shader, uniformLocation, value, ShaderUniformDataType.SHADER_UNIFORM_VEC3);
                break;
            case Vector4:
                Raylib.SetShaderValue(_shader, uniformLocation, value, ShaderUniformDataType.SHADER_UNIFORM_VEC4);
                break;
        }
    }

    public void Dispose() {
        Raylib.UnloadShader(_shader);
    }
}