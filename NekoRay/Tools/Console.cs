using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using ImGuiNET;
using NekoLib.Filesystem;

namespace NekoRay.Tools; 

public static class Console {
    [ConVariable("max_messages")]
    public static int MaxMessageCount { get; set; } = 256;

    public static Queue<string> MessageLog { get; } = new();

    static Console() {
        HotReloadService.OnUpdateApplication += _ => RegisterAll();
    }

    public static void Init() {
        RegisterAll();
    }

    public static void Log(string message) {
        while (MessageLog.Count > MaxMessageCount) {
            MessageLog.Dequeue();
        }
        MessageLog.Enqueue(message);
    }
    
    private static Dictionary<string, MethodInfo> _commands = new();
    private static Dictionary<string, PropertyInfo> _convars = new();
    private static Dictionary<string, MethodInfo> _tagHandlers = new();

    public static List<string> CommandList {
        get {
            var candidates = _commands.Keys.ToList();
            candidates.AddRange(_convars.Keys);
            return candidates;
        }
    }
    
    public static void Submit(string commandline) {
        var regex = new Regex("(?<!\\\\);");
        var commands = regex.Split(commandline).Select(com => com.Replace("\\;", ";")).ToArray();
        foreach (var command in commands) {
            var args = command.Split(" ").ToList();
            args.RemoveAll(s => s == "");
            if (args.Count <= 0) continue;
            var commandName = args[0];
            args.RemoveAt(0);
            try {
                if (_convars.ContainsKey(commandName)) {
                    if (args.Count <= 0) {
                        PrintVariable(commandName);
                        continue;
                    }

                    SubmitVariable(commandName, args[0]);
                    continue;
                }

                SubmitCommand(commandName, args.Cast<object>().ToArray());
            }
            catch (Exception e) {
                Serilog.Log.Error(e, "Command failed with error");
            }
        }
    }

    public static void PrintVariable(string variable) {
        if (!_convars.TryGetValue(variable, out var value)) {
            Serilog.Log.Error("Unknown variable {Variable}", variable);
            return;
        }
        if (value.GetMethod is null) {
            Serilog.Log.Error("{Variable} does not have getter", variable);
            return;
        }
        Serilog.Log.Information("{Variable} is set to {Value}", 
            variable, 
            value.GetValue(null));
    }

    public static void SubmitVariable(string variable, object? arg) {
        if (!_convars.TryGetValue(variable, out var convar)) {
            Serilog.Log.Error("Unknown variable {Variable}", variable);
            return;
        }
        
        List<string> commandTags = new();
        if (Attribute.IsDefined(convar, typeof(ConTagsAttribute))) {
            commandTags = convar.GetCustomAttribute<ConTagsAttribute>()!.Tags;
        }

        foreach (var tag in commandTags) {
            if (!_tagHandlers.TryGetValue(tag, out var tagHandler)) continue;
            if (!(bool) (tagHandler.Invoke(null, null) ?? true)) {
                return;
            }
        }
        
        if (convar.SetMethod is null) {
            Serilog.Log.Error("{Variable} does not have setter", variable);
            return;
        }
        arg = ConvertValue(arg, convar.PropertyType);
        convar.SetValue(null, arg);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object? ConvertValue(object? obj, Type type) {
        ArgumentNullException.ThrowIfNull(type);
        
        if (!type.IsNullable()) return Convert.ChangeType(obj, type);
        
        if (obj is null) return null;
        
        type = new NullableConverter(type).UnderlyingType;
        return Convert.ChangeType(obj, type);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T? ConvertValue<T>(object? obj, Type type) => (T?)ConvertValue(obj, type);

    public static void SubmitCommand(string command, params object?[]? args) {
        if (!_commands.TryGetValue(command, out var conCommand)) {
            Serilog.Log.Error("Unknown command {CommandName}", command);
            return;
        }

        List<string> commandTags = new();
        if (Attribute.IsDefined(conCommand, typeof(ConTagsAttribute))) {
            commandTags = conCommand.GetCustomAttribute<ConTagsAttribute>()!.Tags;
        }

        foreach (var tag in commandTags) {
            if (!_tagHandlers.TryGetValue(tag, out var tagHandler)) continue;
            if (!(bool) (tagHandler.Invoke(null, null) ?? true)) {
                return;
            }
        }
        var argList = (args??Array.Empty<object?>()).ToList();
        var param = conCommand.GetParameters();
        for (var index = 0; index < param.Length; index++) {
            var parameter = param[index];
            if (Attribute.IsDefined(parameter, typeof(ParamArrayAttribute))) {
                var newArgList = argList[..index];
                var paramsArg = argList[index..].ToArray();
                var typedParamsArg = Array.CreateInstance(parameter.ParameterType.GetElementType(), paramsArg.Length);
                Array.Copy(paramsArg, typedParamsArg, paramsArg.Length);
                newArgList.Add(typedParamsArg);
                argList = newArgList;
                break;
            }
            if (index >= argList.Count) {
                if (parameter.IsOptional) {
                    argList.Add(parameter.DefaultValue);
                    continue;
                }
                Serilog.Log.Error("Parameter count mismatch in {CommandName}. Missing {Parameter}", command, parameter);
                return;
            }
            if (argList.Count > param.Length && !Attribute.IsDefined(param[^1], typeof(ParamArrayAttribute))) {
                Serilog.Log.Error("Parameter count mismatch in {CommandName}. Too Many Arguments", command);
                return;
            }
            argList[index] = ConvertValue(argList[index], parameter.ParameterType);
        }

        conCommand.Invoke(null, argList.ToArray());
    }

    [ConCommand("echo")]
    [ConDescription("prints text in the console")]
    public static void Echo(params string[] meow) {
        Serilog.Log.Information(string.Join(' ', meow));
    }
    
    [ConCommand("test_cmd")]
    [ConDescription("meow")]
    [ConTags("test")]
    [Conditional("DEBUG")]
    public static void TestCommand(string meow, float wow = 12f, int hehe = 33, params string[]? columnthree) {
        Serilog.Log.Information(meow);
        Serilog.Log.Information("{0}", wow);
        Serilog.Log.Information("{0}", hehe);
        if (columnthree is not null) {
            Serilog.Log.Information("params is working uwu {Params}", columnthree);
        }
    }
    
    [ConCommand("dotnet_version")]
    [ConDescription("Print running .NET version")]
    public static void DotnetVersion() {
        Serilog.Log.Information("Running " 
                                + RuntimeInformation.FrameworkDescription 
                                + " ("
                                + RuntimeInformation.RuntimeIdentifier
                                + ") On " 
                                + RuntimeInformation.OSDescription 
                                + " "
                                + RuntimeInformation.ProcessArchitecture);
    }
    
    [ConVariable("test_variable")]
    [ConTags("test")]
    public static bool TestVariable { get; set; }
    
    [ConCommand("help")]
    [ConDescription("List all available commands and it's description")]
    public static void Help() {
        var list = "Available commands: ";
        foreach (var command in _commands) {
            list += "\n" + command.Key + "(";
            var parameters = command.Value.GetParameters();
            for (var index = 0; index < parameters.Length; index++) {
                var parameter = parameters[index];
                var notLast = (index + 1 < parameters.Length);
                list += $"{parameter.ParameterType} {parameter.Name}";
                if (parameter.IsOptional) list += $" = {parameter.DefaultValue}";
                if (notLast) list += ", ";
            }
            
            list += ")\n\t";
            var desc = command.Value.GetCustomAttribute<ConDescriptionAttribute>();
            if (desc is not null) 
                list += desc.Description;
            else
                list += "No description.";
            var declType = command.Value.DeclaringType;
            if (declType is not null)
                list += " (From "+ declType.FullName+")";
        }

        list += "\nAvailable convars: ";
        foreach (var convars in _convars) {
            list += "\n" + convars.Key + " = ";
            try {
                list += convars.Value.GetValue(null);
            }
            catch (Exception e) { }
            list += "\n\t";
            var desc = convars.Value.GetCustomAttribute<ConDescriptionAttribute>();
            if (desc is not null) 
                list += desc.Description;
            else
                list += "No description.";
            var declType = convars.Value.DeclaringType;
            if (declType is not null)
                list += " (From "+ declType.FullName+")";
        }
        Serilog.Log.Information(list);
    }
    
    [ConCommand("clear")]
    [ConDescription("Clears Console")]
    public static void Clear() {
        MessageLog.Clear();
    }

    [ConCommand("exec")]
    [ConDescription("run commands from thefile")]
    public static void ExecFile(string path) {
        var virtualPath = Path.Combine("cfg", path + ".cfg");
        if (!Files.FileExists(virtualPath)) {
            Serilog.Log.Error("No file {Path} found", path);
            return;
        }

        Submit(Files.GetFile(virtualPath).Read().Replace("\r\n", ";").Replace("\n", ";"));
    }

    public static bool CheatsWasEnabled { get; private set; }
    private static bool _cheatsEnabled;

    [ConVariable("sv_cheats")]
    [ConDescription("Enable cheats")]
    public static bool CheatsEnabled {
        get => _cheatsEnabled;
        set {
            CheatsWasEnabled = value || CheatsWasEnabled;
            _cheatsEnabled = value;
        }
    }
    
    [ConVariable("test_cheatvar")]
    [ConTags("cheat", "test")]
    public static bool TestCheatVar { get; set; }

    [ConCommand("test_cheat")]
    [ConTags("cheat", "test")]
    [Conditional("DEBUG")]
    public static void TestCheat() {
        Echo("cheat :3");
    }

    [ConTagHandler("cheat")]
    public static bool HandleCheat() => CheatsEnabled;

    [ConCommand("quit")]
    public static void Quit() {
        Program.Quit();
    }

    public static List<string> History { get; } = new();

    [ConCommand("history")]
    public static void PrintHistory() {
        Serilog.Log.Information("History:\n"+string.Join('\n', History));
    }

    private static void RegisterAll() {
        _commands = new();
        _convars = new();
        _tagHandlers = new();
        
        var bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        
        var assemblyTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(domainAssembly => domainAssembly.GetTypes());

        var allMethods = assemblyTypes.SelectMany(type => 
            type
                .GetMethods(bindingFlags)
                .Where(
                    info => Attribute.IsDefined(info, typeof(ConCommandAttribute))
                )
            );
        foreach (var method in allMethods) {
            var conComName = method.GetCustomAttribute<ConCommandAttribute>()!.Name;
            _commands.Add(conComName, method);
        }
        
        var allHandlers = assemblyTypes.SelectMany(type => 
            type
                .GetMethods(bindingFlags)
                .Where(
                    info => Attribute.IsDefined(info, typeof(ConTagHandlerAttribute))
                )
        );
        foreach (var method in allHandlers) {
            var tag = method.GetCustomAttribute<ConTagHandlerAttribute>()!.Tag;
            _tagHandlers.Add(tag, method);
        }
        
        var allProperties = assemblyTypes.SelectMany(type => 
            type
                .GetProperties(bindingFlags)
                .Where(
                    info => Attribute.IsDefined(info, typeof(ConVariableAttribute))
                )
        );
        foreach (var property in allProperties) {
            var tag = property.GetCustomAttribute<ConVariableAttribute>()!.Name;
            _convars.Add(tag, property);
        }
        
        Serilog.Log.Verbose("Console commands successfully registered");
    }

    ///TODO: this will crash if something was registered under the same name
    [Obsolete("All commands are now registered automatically and refreshed on hot reload")]
    public static void Register(Type type) {
        var bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

    var methods = type.GetMethods(bindingFlags)
            .Where(info => Attribute.IsDefined(info, typeof(ConCommandAttribute)));
        foreach (var method in methods) {
            var conComName = method.GetCustomAttribute<ConCommandAttribute>()!.Name;
            _commands[conComName] =  method;
        }
        
        var properties = type.GetProperties(bindingFlags)
            .Where(info => Attribute.IsDefined(info, typeof(ConVariableAttribute)));
        foreach (var property in properties) {
            var conVarName = property.GetCustomAttribute<ConVariableAttribute>()!.Name;
            _convars[conVarName] = property;
        }
        
        var tagHandlers = type.GetMethods(bindingFlags)
            .Where(info => Attribute.IsDefined(info, typeof(ConTagHandlerAttribute)));
        foreach (var tagHandler in tagHandlers) {
            var contag = tagHandler.GetCustomAttribute<ConTagHandlerAttribute>()!.Tag;
            _tagHandlers[contag] = tagHandler;
        }
    }
    [Obsolete("All commands are now registered automatically and refreshed on hot reload")]
    public static void Register<T>() => Register(typeof(T));
}