using CommandLine;
using SoLoud;

namespace NekoRay;

public class CliOptions {
    internal static CliOptions? _instance;
    public static CliOptions Instance => _instance;
    
    [Option("tools", Default=false, HelpText = "Enable dev environment")] //TODO: rename to dev?
    public bool DevMode { get; set; }
    
    [Option('g', "game", HelpText = "name of the mod you want to load")]
    public string? Game { get; set; }
    
    [Option("console", HelpText = "open console on start")]
    public bool ConsoleOnStart { get; set; }
    
    [Option('a',"audio-backend", HelpText = "audio backend to use", Default = SoLoudBackend.Auto)]
    public SoLoudBackend AudioBackend { get; set; }
}