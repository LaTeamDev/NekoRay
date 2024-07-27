using System.Data;
using System.Reflection;
using HotlineSPonyami.Tools;
using Serilog;
using ZeroElectric.Vinculum;

namespace HotlineSPonyami; 

public sealed class Input {
    private Input() {}
    
    [ConCommand("bind")]
    [ConDescription("Bind action on a key")]
    public static void BindCommand(string key, string action) {
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
    
    private static Dictionary<KeyboardKey, string> _kbBinds = new();
    private static Dictionary<MouseButton, string> _mBinds = new();
    private static Dictionary<GamepadButton, string> _gpBinds = new();
    private static Dictionary<string, bool> _down = new();
    private static Dictionary<string, bool> _pressed = new();
    private static Dictionary<string, bool> _released = new();
    private static Dictionary<string, bool> _up = new();
    private static Dictionary<string, bool> _pressedRepeat = new();
    private static HashSet<string> bindList = new();

    public static void Bind(KeyboardKey key, string action) {
        _kbBinds[key] = action;
        bindList.Add(action);
    }
    
    public static void Bind(MouseButton key, string action) {
        _mBinds[key] = action;
        bindList.Add(action);
    }
    
    public static void Bind(GamepadButton key, string action) {
        _gpBinds[key] = action;
        bindList.Add(action);
    }

    [ConCommand("unbind")]
    [ConDescription("Unbind Action")]
    public static void Unbind(string action) {
        _kbBinds.RemoveByValue(action);
        _mBinds.RemoveByValue(action);
        _gpBinds.RemoveByValue(action);
        bindList.Remove(action);
    }

    [ConCommand("unbindall")]
    [ConDescription("Unbind all actions")]
    public static void UnbindAll() {
        foreach (var action in bindList) {
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
    

    public static void Update() {
        foreach (var bind in bindList) {
            _down[bind] = _pressed[bind] = _released[bind] = _up[bind] = _pressedRepeat[bind] = false;
        }
        foreach (var bind in _kbBinds) {
            _down[bind.Value] |= Raylib.IsKeyDown(bind.Key);
            _pressed[bind.Value] |= Raylib.IsKeyPressed(bind.Key);
            _released[bind.Value] |= Raylib.IsKeyReleased(bind.Key);
            _up[bind.Value] |= Raylib.IsKeyUp(bind.Key);
            _pressedRepeat[bind.Value] |= Raylib.IsKeyPressedRepeat(bind.Key);
        }

        foreach (var bind in _mBinds) {
            _down[bind.Value] |= Raylib.IsMouseButtonDown(bind.Key);
            _pressed[bind.Value] |= Raylib.IsMouseButtonPressed(bind.Key);
            _released[bind.Value] |= Raylib.IsMouseButtonReleased(bind.Key);
            _up[bind.Value] |= Raylib.IsMouseButtonUp(bind.Key);
        }
        if (Raylib.IsGamepadAvailable(0)) return;
        foreach (var bind in _gpBinds) {
            _down[bind.Value] |= Raylib.IsGamepadButtonDown(0, bind.Key);
            _pressed[bind.Value] |= Raylib.IsGamepadButtonPressed(0, bind.Key);
            _released[bind.Value] |= Raylib.IsGamepadButtonReleased(0, bind.Key);
            _up[bind.Value] |= Raylib.IsGamepadButtonUp(0, bind.Key);
        }
    }
}