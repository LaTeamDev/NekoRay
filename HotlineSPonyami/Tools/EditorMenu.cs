using ImGuiNET;
using NativeFileDialogSharp;
using NekoRay;
using ZeroElectric.Vinculum;
using Image = NekoRay.Image;

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
                DialogResult result = Dialog.FileSave("map", Path.Combine(Directory.GetCurrentDirectory(), "data"));
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
                DialogResult result = Dialog.FileOpen("map", Path.Combine(Directory.GetCurrentDirectory(), "data"));
                if (result.IsOk)
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(result.Path, FileMode.Open)))
                    {
                        Scene.Load(reader);
                    }
                }
            }
            if (ImGui.MenuItem("Clear")) {
                Scene.Field.Clear();
            }

            ImGui.MenuItem("Draw grid", "", ref Scene.DrawGrid);
            ImGui.EndMenu();
        }

        if (ImGui.BeginMenu("Image"))
        {
            if (ImGui.MenuItem("Generate Tile Map Texture"))
            {
                Image finalImage = ImageGen.Color(UnpackedTextures.GetAllFloorTextures().Count * 32, 32, Raylib.WHITE);
                for (int i = 0; i < UnpackedTextures.GetAllFloorTextures().Count; i++)
                {
                    Image img = Image.FromTexture(UnpackedTextures.GetAllFloorTextures()[i]);
                    Rectangle src = new Rectangle(0, 0, 32, 32);
                    Rectangle dest = new Rectangle(i * 32, 0, 32, 32);
                    finalImage.Draw(img, src, dest, Raylib.WHITE);
                }
                finalImage.Export("textures/texture_atlas.png");
                Image wallsImage = ImageGen.Color(UnpackedTextures.GetAllWallTextures().Count * 48, 48, Raylib.WHITE);
                for (int i = 0; i < UnpackedTextures.GetAllWallTextures().Count; i++)
                {
                    Image img = Image.FromTexture(UnpackedTextures.GetAllWallTextures()[i]);
                    Rectangle src1 = new Rectangle(0, 0, 48, 48);
                    Rectangle dest1 = new Rectangle(i * 48, 0, 48, 48);
                    wallsImage.Draw(img, src1, dest1, Raylib.WHITE);
                }
                wallsImage.Export("textures/walls_atlas.png");
            }

            if (ImGui.MenuItem("Export Map"))
            {
                DialogResult result = Dialog.FileSave("png", Path.Combine(Directory.GetCurrentDirectory(), "data"));
                if (result.IsOk)
                {
                    Scene.Field.Export(result.Path);
                }
            }
            ImGui.EndMenu();
        }

        ImGui.EndMainMenuBar();
    }
}