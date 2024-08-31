using System.Numerics;
using ImGuiNET;
using NekoRay.Tools;
using rlImGui_cs;
using Serilog;
using Console = NekoRay.Tools.Console;

namespace NekoRay; 

public sealed class Input {
    private Input() {}
    
    [ConCommand("bind")]
    [ConDescription("Bind action on a key")]
    public static void Bind(string key, string action) {
        if (Enum.TryParse<KeyboardKey>(key, out var kbKey)) {
            Bind(kbKey, action);
            return;
        }
        if (Enum.TryParse<MouseButton>(key, out var mKey)) {
            Bind(mKey, action);
            return;
        }
        if (Enum.TryParse<GamepadButton>(key, out var gpKey)) {
            Bind(gpKey, action);
            return;
        }
        Log.Error("Failed to bind {Action} to {Key}", action, key);
    }

    [ConCommand("bindcommand")]
    [ConDescription("Bind command on a key")]
    public static void BindCommand(string key, string command) {
        if (Enum.TryParse<KeyboardKey>(key, out var kbKey)) {
            BindCommand(kbKey, command);
            return;
        }
        Log.Error("Failed to bind {Command} to {Key}", command, key);
    }
    
    private static Dictionary<KeyboardKey, string> _kbBinds = new();
    private static Dictionary<MouseButton, string> _mBinds = new();
    private static Dictionary<GamepadButton, string> _gpBinds = new();
    private static Dictionary<string, bool> _down = new();
    private static Dictionary<string, bool> _pressed = new();
    private static Dictionary<string, bool> _released = new();
    private static Dictionary<string, bool> _up = new();
    private static Dictionary<string, bool> _pressedRepeat = new();
    private static HashSet<string> _bindList = new();
    private static List<Tuple<KeyboardKey, string>> _commandList = new();

    public static void Bind(KeyboardKey key, string action) {
        _kbBinds[key] = action;
        _bindList.Add(action);
    }
    
    public static void Bind(MouseButton key, string action) {
        _mBinds[key] = action;
        _bindList.Add(action);
    }
    
    public static void Bind(GamepadButton key, string action) {
        _gpBinds[key] = action;
        _bindList.Add(action);
    }
    
    public static void BindCommand(KeyboardKey key, string command) {
        _commandList.Add(new Tuple<KeyboardKey, string>(key, command));
    }

    [ConCommand("unbind")]
    [ConDescription("Unbind Action")]
    public static void Unbind(string action) {
        _kbBinds.RemoveByValue(action);
        _mBinds.RemoveByValue(action);
        _gpBinds.RemoveByValue(action);
        _bindList.Remove(action);
    }

    [ConCommand("unbindall")]
    [ConDescription("Unbind all actions")]
    public static void UnbindAll() {
        foreach (var action in _bindList) {
            _kbBinds.RemoveByValue(action);
            _mBinds.RemoveByValue(action);
            _gpBinds.RemoveByValue(action);
        }
    }

    public static void Unbind(KeyboardKey key) {
        _kbBinds.Remove(key);
    }
    public static void Unbind(MouseButton button) {
        _mBinds.Remove(button);
    }
    public static void Unbind(GamepadButton key) {
        _gpBinds.Remove(key);
    }
    
    [ConCommand("unbindcommand")]
    [ConDescription("Unbind command")]
    public static void UnbindCommand(string command) {
        foreach (var a in _commandList.Where(tuple => tuple.Item2 == command)) {
            _commandList.Remove(a);
        }
    }

    public static bool IsDown(string action) {
        if (!_down.TryGetValue(action, out var value)) return false;
        return value;
    }

    public static bool IsPressed(string action) {
        if (!_pressed.TryGetValue(action, out var value)) return false;
        return value;
    }

    public static bool IsReleased(string action) {
        if (!_released.TryGetValue(action, out var value)) return false;
        return value;
    }

    public static bool IsUp(string action) {
        if (!_up.TryGetValue(action, out var value)) return false;
        return value;
    }

    public static bool IsRepeat(string action) {
        if (!_pressedRepeat.TryGetValue(action, out var value)) return false;
        return value;
    }

    internal static Vector2 _lastMousePos;
    public static Vector2 MousePosition {
        get => _lastMousePos;
        internal set => _lastMousePos = value;
    }
    
    internal static void UpdateMouseBinds() {
        foreach (var bind in _mBinds) {
            _down[bind.Value] |= Raylib.IsMouseButtonDown(bind.Key);
            _pressed[bind.Value] |= Raylib.IsMouseButtonPressed(bind.Key);
            _released[bind.Value] |= Raylib.IsMouseButtonReleased(bind.Key);
            _up[bind.Value] |= Raylib.IsMouseButtonUp(bind.Key);
        }
    }

    internal static void UpdateKeyboardBinds() {
        foreach (var bind in _kbBinds) {
            _down[bind.Value] |= Raylib.IsKeyDown(bind.Key);
            _pressed[bind.Value] |= Raylib.IsKeyPressed(bind.Key);
            _released[bind.Value] |= Raylib.IsKeyReleased(bind.Key);
            _up[bind.Value] |= Raylib.IsKeyUp(bind.Key);
            _pressedRepeat[bind.Value] |= Raylib.IsKeyPressedRepeat(bind.Key);
        }
    }

    internal static void UpdateGamepadBinds() {
        foreach (var bind in _gpBinds) {
            _down[bind.Value] |= Raylib.IsGamepadButtonDown(0, bind.Key);
            _pressed[bind.Value] |= Raylib.IsGamepadButtonPressed(0, bind.Key);
            _released[bind.Value] |= Raylib.IsGamepadButtonReleased(0, bind.Key);
            _up[bind.Value] |= Raylib.IsGamepadButtonUp(0, bind.Key);
        }
    }

    public static bool ForceUpdate = false;


    public static void Update() {
        var io = ImGui.GetIO();
        foreach (var commandBind in _commandList) {
            if (Raylib.IsKeyPressed(commandBind.Item1)) Console.Submit(commandBind.Item2);
        }
        
        foreach (var bind in _bindList) {
            _down[bind] = _pressed[bind] = _released[bind] = _up[bind] = _pressedRepeat[bind] = false;
        }
        
        if (!io.WantCaptureKeyboard || ForceUpdate)
            UpdateKeyboardBinds();

        if (!io.WantCaptureMouse || ForceUpdate) {
            UpdateMouseBinds();
        }
        
        if (!io.WantCaptureMouse)
            MousePosition = Raylib.GetMousePosition();
        
        if (Raylib.IsGamepadAvailable(0)) return;
        UpdateGamepadBinds();
    }
}