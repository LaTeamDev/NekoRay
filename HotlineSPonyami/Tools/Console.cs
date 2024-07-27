using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ImGuiNET;
using NekoLib.Core;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using ZeroElectric.Vinculum;
using Shader = NekoRay.Shader;
using Texture = NekoRay.Texture;

namespace HotlineSPonyami.Tools; 

public class Console : Behaviour {
    public static int MaxMessageCount = 256;

    private static Queue<string> _messageLog = new();

    static Console() {
        Register<Console>();
    }

    public static void Log(string message) {
        while (_messageLog.Count > MaxMessageCount) {
            _messageLog.Dequeue();
        }
        _messageLog.Enqueue(message);
    }

    private string _inputBuffer = "";
    private static Dictionary<string, MethodInfo> _commands = new();

    void DrawGui() {
        var opened = Enabled;
        if (ImGui.Begin("Console", ref opened)) {
            ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0,0,0, 0.1f));
            if (ImGui.BeginChild("MessageLog", new Vector2(ImGui.GetWindowWidth(), 
                    ImGui.GetWindowHeight()-ImGui.GetFrameHeightWithSpacing()-ImGui.GetTextLineHeightWithSpacing()-18f))) {
                foreach (var message in _messageLog) {
                    ImGui.TextWrapped(message);
                }
            }
            ImGui.EndChild();
            ImGui.PopStyleColor();
            ImGui.BeginGroup();
            var inputTextFlags =
                ImGuiInputTextFlags.EnterReturnsTrue |
                ImGuiInputTextFlags.EscapeClearsAll;
            //TODO: callback for completition
            if (ImGui.InputText("", ref _inputBuffer, 255, inputTextFlags)) {
                SubmitBuffer();
            }
            ImGui.SameLine();
            if (ImGui.Button("Send")) {
                SubmitBuffer();
            }
            ImGui.EndGroup();
        }
        ImGui.End();
        if (Enabled != opened) {
            Enabled = opened;
        }
    }

    void SubmitBuffer() {
        try {
            Submit(_inputBuffer);
        }
        catch (Exception e) {
            Serilog.Log.Error("Command failed with error {Exception}", e.ToString());
        }
        _inputBuffer = "";
    }

    public static void Submit(string commandline) {
        _messageLog.Enqueue("> "+ commandline);
        var regex = new Regex("(?<!\\\\);");
        var commands = regex.Split(commandline).Select(com => com.Replace("\\;", ";")).ToArray();
        foreach (var command in commands) {
            var args = command.Split(" ").ToList();
            args.RemoveAll(s => s == "");
            if (args.Count <= 0) continue;
            var commandName = args[0];
            args.RemoveAt(0);
            SubmitCommand(commandName, args.Cast<object>().ToArray());
        }
    }

    public static void SubmitCommand(string command, params object?[]? args) {
        if (!_commands.ContainsKey(command)) {
            Serilog.Log.Error("Unknown command {CommandName}", command);
            return;
        }
        if (args is null) _commands[command].Invoke(null, args);
        var param = _commands[command].GetParameters();
        for (var index = 0; index < param.Length; index++) {
            var parameter = param[index];
            if (args.Length <= index) {
                Serilog.Log.Error("Parameter count mismatch in {CommandName}", command);
                return;
            }
            if (parameter.ParameterType == args[index].GetType()) continue;
            if (args[index].GetType() != typeof(string)) continue;
            var parseMethod = parameter.ParameterType.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static);
            if (parseMethod is null)
                throw new ArgumentException("the type is wrong and i cant parse it", parameter.Name);
            args[index] = parseMethod.Invoke(null, new object?[] { args[index]});
        }

        _commands[command].Invoke(null, args);
    }

    [ConCommand("echo")]
    [ConDescription("prints text in the console")]
    //TODO: Support params
    public static void Echo(string meow) {
        Serilog.Log.Information(meow);
    }
    
    [ConCommand("test_cmd")]
    [ConDescription("meow")]
    //TODO: Support params
    public static void TestCommand(string meow, float wow = 12f, int hehe = 33) {
        Serilog.Log.Information(meow);
        Serilog.Log.Information("{0}", wow);
        Serilog.Log.Information("{0}", hehe);
    }
    
    [ConCommand("help")]
    [ConDescription("List all available commands and it's description")]
    public static void Help() {
        var list = "Available commands: ";
        foreach (var command in _commands) {
            list += "\n" + command.Key + " - ";
            var desc = command.Value.GetCustomAttribute<ConDescriptionAttribute>();
            if (desc is not null) 
                list += desc.Description;
            else
                list += "No description.";
            var declType = command.Value.DeclaringType;
            if (declType is not null)
                list += " (From "+ declType.FullName+")";
        }
        Serilog.Log.Information(list);
    }
    
    [ConCommand("clear")]
    [ConDescription("Clears Console")]
    public static void Clear() {
        _messageLog.Clear();
    }

    public static void Register<T>() {
        var methods = typeof(T).GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(info => info.GetCustomAttribute<ConCommandAttribute>() is not null);
        foreach (var method in methods) {
            var conComName = method.GetCustomAttribute<ConCommandAttribute>()!.Name;
            _commands.Add(conComName, method);
        }
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class ConCommandAttribute : Attribute {
    private string _name;
    public string Name => _name;

    public ConCommandAttribute(string name) {
        _name = name;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class ConDescriptionAttribute : Attribute {
    private string _description;
    public string Description => _description;

    public ConDescriptionAttribute(string name) {
        _description = name;
    }
}
public class ConsoleSink : ILogEventSink
{
    private readonly IFormatProvider _formatProvider;

    public ConsoleSink(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public void Emit(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(_formatProvider);
        Console.Log(message);
    }
}
public static class ConsoleSinkExtensions
{
    public static LoggerConfiguration GameConsole(
        this LoggerSinkConfiguration loggerConfiguration,
        IFormatProvider formatProvider = null)
    {
        return loggerConfiguration.Sink(new ConsoleSink(formatProvider));
    }
}