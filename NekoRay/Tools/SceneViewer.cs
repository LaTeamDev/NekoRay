using NekoLib.Core;
using ImGuiNET;
using NekoLib.Scenes;

namespace NekoRay.Tools; 

public class SceneViewer : ToolBehaviour {
    private ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.OpenOnDoubleClick | ImGuiTreeNodeFlags.SpanAvailWidth;
    
    void DrawGui() {
        if (ImGui.Begin("Hierarchy")) {
            foreach (var scene in SceneManager.Scenes) {
                if (scene == SceneManager.ActiveScene) ImGui.SetNextItemOpen(true, ImGuiCond.Once);
                DrawSceneGui(scene);
            }
        }
        ImGui.End();
    }

    [ConCommand("scene_viewer")]
    [ConTags("cheat")]
    public static void OpenSceneViewer() => ToolsShared.ToggleTool<SceneViewer>();

    void DrawSceneGui(IScene scene) {
        if (ImGui.TreeNode(scene.Name)) {
            DrawGameObjectHierarchyRootGui(scene);
            ImGui.TreePop();
        }
    }

    void DrawGameObjectHierarchyRootGui(IScene scene) {
        var currentGameObjects = new GameObject[scene.GameObjects.Count];
        scene.GameObjects.CopyTo(currentGameObjects);
        foreach (var gameObject in currentGameObjects) {
            if (gameObject.Transform.Parent is not null) continue;
            var node = ImGui.TreeNodeEx(gameObject.Name+"##"+gameObject.Id, flags);
            if (ImGui.IsItemClicked() && !ImGui.IsItemToggledOpen())
                Inspect.Instance.SelectedObject = gameObject;
            if (node) {
                DrawGameObjectHierarchyGui(gameObject);
                ImGui.TreePop();
            }
        }
    }
    
    void DrawGameObjectHierarchyGui(GameObject gameObject) {
        foreach (var transform in gameObject.Transform) {
            var node = ImGui.TreeNodeEx(transform.GameObject.Name+"##"+transform.GameObject.Id, flags);
            if (ImGui.IsItemClicked() && !ImGui.IsItemToggledOpen())
                Inspect.Instance.SelectedObject = transform.GameObject;
            if (node) {
                DrawGameObjectHierarchyGui(transform.GameObject);
                ImGui.TreePop();
            }
        }
    }
}