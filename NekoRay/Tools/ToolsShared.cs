namespace NekoRay.Tools;

public static class ToolsShared {
    public static void RunInPersistant(Action<IScene> action) {
        var scene = SceneManager.GetScene<PersistantScene>();
        using (scene.UseTemporarily()) {
            action(scene);
        }
    }
    
    public static T ToggleTool<T>() where T : ToolBehaviour, new() {
        T? toolBehaviour = null;
        RunInPersistant(scene => {
            var toolName = typeof(T).FullName;
            ArgumentException.ThrowIfNullOrWhiteSpace(toolName, nameof(toolName));
            var gameObject = scene.GetGameObjectByName(toolName);
            if (gameObject is not null) {
                gameObject.ActiveSelf = !gameObject.ActiveSelf;
                toolBehaviour = gameObject.GetComponent<T>();
                return;
            }
            gameObject = new GameObject(toolName);
            toolBehaviour = gameObject.AddComponent<T>();
        });
        if (toolBehaviour is null) throw new NullReferenceException("Could not create or get tool");
        return toolBehaviour;
    }
}