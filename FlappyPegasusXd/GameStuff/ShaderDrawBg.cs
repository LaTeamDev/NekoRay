﻿using System.Numerics;
using NekoLib.Core;
using NekoRay;
using ZeroElectric.Vinculum;
using Shader = NekoRay.Shader;
using Texture = NekoRay.Texture;

namespace FlappyPegasus.GameStuff; 

public class ShaderDrawBg : Behaviour {
    private Shader Shader;
    public Texture? Texture;
    public float Speed = 1f;
    
    
    void Awake() {
        Shader = Data.GetShader("shader/movetexture.frag");
        Shader.SetVector2("direction", -Vector2.UnitX);
    }

     void Render() {
        if (Texture is null) return;
        Matrix4x4.Decompose(Transform.GlobalMatrix, out var scale, out _, out _);
        var position = new Vector2(Transform.Position.X, Transform.Position.Y);
        var renderScale = Raylib.GetRenderHeight() / 288f;
        Shader.SetFloat("speed", Speed);
        Shader.SetFloat("time", (float)Time.CurrentTime);
        using (Shader.Attach()) {
            Texture.Draw(
                new Rectangle(0, 0,   Raylib.GetRenderWidth()/renderScale, Texture.Height), 
                new Rectangle(position.X, position.Y, Raylib.GetRenderWidth()/renderScale, Texture.Height), 
                new Vector2(0, 0),
                0f,
                Raylib.WHITE);
        }
     }
}