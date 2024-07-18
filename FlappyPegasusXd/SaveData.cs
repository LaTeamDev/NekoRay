using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using NekoRay;
using Tomlyn;

namespace FlappyPegasus; 

public class SaveData {


    public static SaveData data;
    [JsonPropertyName("coinCount")] 
    public int _coinCount { get; set; }

    [JsonPropertyName("bestScore")]
    public int _bestScore { get; set; }
    [JsonPropertyName("deathCount")] 
    public int _deathCount { get; set; }

    public static int CoinCount {
        get => data._coinCount;
        set => data._coinCount = value;
    }

    public static int BestScore {
        get => data._bestScore;
        set => data._bestScore = value;
    }

    public static int DeathCount {
        get => data._deathCount;
        set => data._deathCount = value;
    }

    public static string GetSaveLocation() {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FlappyPegasusXd", "save.json");
    }

    public static void Load() {
        Directory.CreateDirectory(Path.GetDirectoryName(GetSaveLocation()));
        if (!File.Exists(GetSaveLocation())) File.Create(GetSaveLocation()).Dispose();
        try {
            data = JsonSerializer.Deserialize<SaveData>(File.ReadAllText(GetSaveLocation()));
        }
        catch (JsonException) {
            data = new SaveData();
        }
    }

    public static void Save() {
        Directory.CreateDirectory(Path.GetDirectoryName(GetSaveLocation()));
        using var file = File.CreateText(GetSaveLocation());
        file.Write(JsonSerializer.Serialize(data));
    }
}