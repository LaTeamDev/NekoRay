# NekoRay
a silly game engine made in two days (lmao) to battle test NekoLib built on Raylib

of course it is not finished and only a subset of raylib features is present

## How to make a game
this engine uses source like code mounting. so, to make a game create a **library project**
reference NekoRay.dll/proj and extend `GameBase` (`Load()` will be your entry point).
to make engine recognize compiled dll you need to create `whatever.conf.toml`
and type identity = "dllname" to engine to load the compiled dll

to run the game run 
```
./NekoRay -game whatever #where whatever is the name of conf.toml
```
you can supply default conf, e.g. to run game just to double click (e.g. for publishing) 
under the name "default.conf.toml"


btw the conf.toml file is also your settings like width height of monitor etc

by default there will be hooked up scene management from nekolib but
if you dont want to use it just replace `Update()` and `Draw()`.

if you think im dumb and should not touch the code ever again e.g. "my timestep
is not fixed" just straight up override `Run()` (love2d style) and supply your
own loop function.

there is also a stolen error handler approach from love2d, if your code crash
it will switch game loop to error loop (`ErrorHandler()` method). You can override it too.

in case you stick with scene management you can extend BaseScene and override
`Initialize()` to create  in. However, you still can create your own scene from scratch
using `IScene`.

As this engine relies on `NekoLib` there is Unity-like ECS.
```csharp
// consider this code is valid
var Player = new GameObject("Player");
Player.AddComponent<Rigidbody2d>; // does not exist (yet) in engine
Player.AddComponent<SpriteRenderer>;
Player.AddComponent<PlayerController>;
```
if you use SceneBase your good to go, it will work as long as
base.Initialize() called after. If youre writing your own scene/do not use them you need
to update gameobject somehow

```csharp
Player.Update();
Player.Draw();
```

keep in mind if using your ownScene you need to manually draw render texture from camera e.g.
```csharp
public virtual void DrawCameraTexture() {
    if (BaseCamera.Main is null) return;
    if (this != BaseCamera.Main.GameObject.Scene) return;
    
    var texture = BaseCamera.Main.RenderTexture.Texture;
    var rect = new Rectangle(0, 0, texture.Width, -texture.Height);
    var rectDest = new Rectangle(0, 0, texture.Width, texture.Height);
    texture.Draw(rect, rectDest, Vector2.Zero, 0f, Raylib.WHITE);
}
```

a semiwhat working game can be found in `FlappyPegasusXd` project.

have fun

## License
The engine is licensed under zlib license.