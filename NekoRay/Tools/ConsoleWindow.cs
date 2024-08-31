using System.Numerics;
using System.Runtime.InteropServices;
using ImGuiNET;

namespace NekoRay.Tools;

public class ConsoleWindow : ToolBehaviour {
    internal static ConsoleWindow? Instance;

    public ConsoleWindow() {
        Instance = this;
    }
    
    private string _inputBuffer = "";
    
    private static void CompletionCallback(ImGuiInputTextCallbackDataPtr data) {
        // Locate beginning of current word
        var str = Marshal.PtrToStringUTF8(data.Buf)??"";
        var last = Math.Max(str.LastIndexOf(' '), str.LastIndexOf(';'))+1;
        var size = str.Length - last;

        // Build a list of candidates
        var candidates = Console.CommandList.Where(s => s.StartsWith(str??"")).ToList();

        if (candidates.Count == 0) return;
        //if (candidates.Count <= 1)
        //{
        // Single match. Delete the beginning of the word and replace it entirely so we've got nice casing.
        data.DeleteChars(last, size);
        data.InsertChars(data.CursorPos, candidates[0]);
        //data.InsertChars(data.CursorPos, " ");
        return;
        //}
        // Multiple matches. Complete as much as we can..
        //TODO:
    }
    
    //todo: fix
    private static void HistoryCallback(ImGuiInputTextCallbackDataPtr data) {
        _historyBuffer ??= Marshal.PtrToStringUTF8(data.Buf);
        //Log($"{_historyBuffer}\nthe history is \n{string.Join('\n', _history)}");
        if (Console.History.Count == 0) return;
        var prevHistoryPos = _historyPos;
        if (data.EventKey == ImGuiKey.UpArrow) {
            for (var i = Console.History.Count - 1; i >= 0; i--) {
                _historyPos--;
                if (_historyPos <= -1)
                    _historyPos = Console.History.Count - 1;
                if (Console.History[i].StartsWith(_historyBuffer)) break;
            }

        }
        else if (data.EventKey == ImGuiKey.DownArrow) {
            for (var i = 0; i < Console.History.Count; i++) {
                _historyPos++;
                if (_historyPos >= Console.History.Count)
                    _historyPos = 0;
                if (Console.History[i].StartsWith(_historyBuffer)) break;
            }
        }
        
        if (prevHistoryPos != _historyPos)
        {
            var historyStr = (_historyPos >= 0) ? Console.History[_historyPos] : "";
            data.DeleteChars(0, data.BufTextLen);
            data.InsertChars(0, historyStr);
        }
    }
    
    private static unsafe int TextEditCallback(ImGuiInputTextCallbackData* data)
    {
        switch (data->EventFlag) {
            case ImGuiInputTextFlags.CallbackCompletion:
                CompletionCallback(data); 
                break;
            case ImGuiInputTextFlags.CallbackHistory: 
                HistoryCallback(data);
                break;
            case ImGuiInputTextFlags.CallbackEdit:
                _historyPos = -1;
                _historyBuffer = null;
                break;
        }
        return 0;
    }

    unsafe void DrawGui() {
        var opened = Enabled;
        if (ImGui.Begin("Console", ref opened)) {
            ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0,0,0, 0.1f));
            if (ImGui.BeginChild("MessageLog", new Vector2(ImGui.GetWindowWidth(), 
                    ImGui.GetWindowHeight()-ImGui.GetFrameHeightWithSpacing()-ImGui.GetTextLineHeightWithSpacing()-18f))) {
                foreach (var message in Console.MessageLog) {
                    ImGui.TextWrapped(message);
                }
            }
            ImGui.EndChild();
            ImGui.PopStyleColor();
            ImGui.BeginGroup();
            var inputTextFlags =
                ImGuiInputTextFlags.EnterReturnsTrue |
                ImGuiInputTextFlags.EscapeClearsAll | 
                ImGuiInputTextFlags.CallbackHistory | 
                ImGuiInputTextFlags.CallbackCompletion |
                ImGuiInputTextFlags.CallbackEdit;
            if (ImGui.InputText("", ref _inputBuffer, 255, inputTextFlags, TextEditCallback)) {
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
            _historyPos = 0;
            Console.History.Add(_inputBuffer);
            Console.Log("> "+ _inputBuffer);
            Console.Submit(_inputBuffer);
        }
        catch (Exception e) {
            Serilog.Log.Error(e, "Command failed with error");
        }
        _inputBuffer = "";
    }
    
    private static int _historyPos = -1;
    private static string? _historyBuffer = null;

    [ConCommand("toggleconsole")]
    public static ConsoleWindow ToggleConsole() => ToolsShared.ToggleTool<ConsoleWindow>();
}