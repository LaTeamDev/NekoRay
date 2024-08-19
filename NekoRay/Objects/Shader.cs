using System.Numerics;

namespace NekoRay; 

public class Shader : NekoObject {
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

    private Dictionary<string, int> _shaderAttributeLocationCache = new();
    private Dictionary<string, int> _shaderUniformLocationCache = new();
    public int GetLocation(string uniformName) {
        //if (_shaderUniformLocationCache.TryGetValue(uniformName, out var value))
        //    return value;
        return _shaderUniformLocationCache[uniformName] = Raylib.GetShaderLocation(_shader, uniformName);
    }


    public int GetAttributeLocation(string attributeName) {
        //if (_shaderAttributeLocationCache.TryGetValue(attributeName, out var value))
        //    return value;
        return _shaderAttributeLocationCache[attributeName] = Raylib.GetShaderLocationAttrib(_shader, attributeName);
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
    public void SetVector2(string name, Vector2 value) =>
        Raylib.SetShaderValue(_shader, GetLocation(name), value, ShaderUniformDataType.SHADER_UNIFORM_VEC2);
    public void SetInt(string name, int value) =>
        Raylib.SetShaderValue(_shader, GetLocation(name), value, ShaderUniformDataType.SHADER_UNIFORM_INT);
    public void SetFloat(string name, float value) =>
        Raylib.SetShaderValue(_shader, GetLocation(name), value, ShaderUniformDataType.SHADER_UNIFORM_FLOAT);
    public void SetTexture(string name, Texture value) =>
        Raylib.SetShaderValueTexture(_shader, GetLocation(name), value._texture);
    
    
    private static Stack<Shader> _shaderStack = new ();

    public AttachMode Attach() {
        _shaderStack.Push(this);
        Raylib.BeginShaderMode(_shader);
        return new AttachMode(Detach);
    }
    

    private void Detach() {
        if (!_shaderStack.Contains(this))
            throw new Exception("Huh??? The texture you want to pop isn't even in stack?? wth");
        Raylib.EndShaderMode();
        if (!_shaderStack.TryPop(out var shader)) {
            return;
        }
        if (shader != this) {
            throw new Exception("you tried to detach in wrong order");
        } 
        if (_shaderStack.TryPeek(out var anotherShader))
            Raylib.BeginShaderMode(anotherShader._shader);
    }

    public override void Dispose() {
        Raylib.UnloadShader(_shader);
    }
}