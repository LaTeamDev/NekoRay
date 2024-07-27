﻿using NekoLib.Core;
using NekoRay;
using ZeroElectric.Vinculum;

namespace HotlineSPonyami.Gameplay; 

public class ReticleController : Behaviour {
    void Update() {
        var position = BaseCamera.Main.ScreenToWorld(Raylib.GetMousePosition());
        Transform.Position = Transform.Position with {X = position.X, Y = position.Y};
    }
}