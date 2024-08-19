namespace NekoRay; 

[AttributeUsage(AttributeTargets.Assembly)]
public class DefaultGameIdAttribute : Attribute {
    public string GameId;

    public DefaultGameIdAttribute(string id) {
        GameId = id;
    }
}