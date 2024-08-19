# NekoRay
a silly game engine built on Raylib and NekoLib

of course it is not finished and only a subset of raylib features is present

## How to make a game
this engine uses source like code mounting. so, to make a game create a **library project**
reference NekoRay.dll/proj and extend `GameBase` (`Load()` will be your entry point).
to make engine recognize compiled dll you need to follow following structure
```
.
├─ bin
├─ <gameid> # this is an id of your game
│  ├─ bin
│  │  └─ client.dll # this is where your compiled dll along it's dependecies should be
│  └─ conf.toml 
├─ NekoRay.exe
└─ NekoRay.dll
```

conf.toml sample content
```toml
name = "Hotline Ponyami"

[meta]
developers = ["VanderCat", "Mr.Yabloko"]
website = "https://github.com/LaTeamDev/NekoRay"

[filesystem]
bin = ["{{this}}/bin/"]
mount = ["{{this}}", "{{this}}/custom/*"]
```
a minimal conf.toml would look like
```toml
[filesystem]
bin = ["{{this}}/bin/"]
mount = ["{{this}}"]
```

to run the game run 
```
./NekoRay -game <gameid> #where <gameid> is the name of your game folder
```

you can also set the default gameid on build
```shell
dotnet build NekoRay /p:NekoRayDefaultGameId=<gameid>
```

## Tips and Tricks
### Input
if you ever need rebindable controls you can use Input class
```csharp
Input.Bind(KeyboardKey.KEY_SPACE, "Jump");
if (Input.IsPressed("Jump")) Log.Debug("Jump");
```

### Console
there is a somewhat functional console subsystem capable of running commands. It is
enabled by default but hidden unless you specify `--console` argument or bind command `toggleconsole` on
a key

#### Config files
You can use this to handle user settings or make a system to load specific settings in bulk e. g. for debug
```
exec <NAME> # where name is cfg/<NAME>.cfg
```
also autoexec.cfg is launched automaticly (great place to handle user binds)

#### Custom console command
```csharp
public class Commands { //NOTE: it does not need to be it's own class, but it NEEDS to be a non static one
    [ConsoleCommand("console_name")]
    [ConsoleDescription("Description text to show in help")]
    public static void SampleCommand(string arg1, int arg2) {
    }
}

//register as early as you can, e.g. GameBase.Initialize();
Console.Register<Commands>();
```

#### Useful commands
##### bind action using console
```
bind <KEY> <ACTION>
```
##### bind console command on a key
```
bindtoggle <KEYBOARD_KEY> <COMMAND>
```
#### list all commands
```
help
```
#### Changing loop function
if you think im dumb and should not touch the code ever again e.g. "my timestep
is not fixed" just straight up override `Run()` (love2d style) and supply your
own loop function.

#### Crash Handling
there is also a stolen crash handler approach from love2d, if your code crash
it will switch game loop to error loop (`ErrorHandler()` method). You can override it too.

#### Scenes
you can extend BaseScene and override
`Initialize()` to create scene in. However, you still can create your own scene implementation from scratch
using `IScene`.

#### ECS
As this engine relies on `NekoLib` there is Unity-like EC
```csharp
// consider this code is valid
var Player = new GameObject("Player");
Player.AddComponent<Rigidbody2d>; // does not exist (yet) in engine
Player.AddComponent<SpriteRenderer>;
Player.AddComponent<PlayerController>;
```
if you use SceneBase your good to go, it will work as long as
base.Initialize() called after. If youre writing your own scene/do not use them you need
to update gameobjects somehow

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

---

a semiwhat working game can be found in `FlappyPegasusXd` project.

have fun

## License
The engine is licensed under zlib license.