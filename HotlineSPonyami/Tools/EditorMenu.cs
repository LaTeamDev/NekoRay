using ImGuiNET;
using NativeFileDialogSharp;
using NekoLib.Core;

namespace HotlineSPonyami.Tools;

public class EditorMenu : EditorWindow
{
    
    void DrawGui()
    {
        ImGui.BeginMainMenuBar();

        if (ImGui.BeginMenu("File"))
        {
            if (ImGui.MenuItem("Save"))
            {
                DialogResult result = Dialog.FileSave();
                if (result.IsOk)
                {
                    using (BinaryWriter writer = new BinaryWriter(File.Open(result.Path, FileMode.OpenOrCreate)))
                    {
                        Scene.Save(writer);
                    }
                }
            }
            if (ImGui.MenuItem("Open"))
            {
                DialogResult result = Dialog.FileOpen();
                if (result.IsOk)
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(result.Path, FileMode.Open)))
                    {
                        Scene.Load(reader);
                    }
                }
            }
            ImGui.EndMenu();
        }
        
        ImGui.EndMainMenuBar();
    }
}