using System.Reflection.Metadata;
using Serilog;

[assembly: MetadataUpdateHandler(typeof(NekoRay.HotReloadService))]

namespace NekoRay;
public delegate void HotReloadCallback(Type[]? updatedTypes);
internal static class HotReloadService {
    public static event HotReloadCallback? OnClearCache;
    public static event HotReloadCallback? OnUpdateApplication;

    private static void ClearCache(Type[]? updatedTypes) =>
        OnClearCache?.Invoke(updatedTypes);

    private static void UpdateApplication(Type[]? updatedTypes) =>
        OnUpdateApplication?.Invoke(updatedTypes);

    static HotReloadService() {
        OnUpdateApplication += types => {
            Log.Verbose("The application was hot reloaded! Affected types: {Types}", types);
        };
    }
}