using System.Data;
using System.Numerics;
using NekoLib.Core;
using NekoRay;

namespace LuaScriptingTest;

public class SpamMove : Behaviour {
    void Update() {
        Transform.Position = new Vector3(0f, (float)Math.Sin(Time.CurrentTime), 0f);
    }
}