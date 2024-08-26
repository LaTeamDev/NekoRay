using System.Text.RegularExpressions;
using BindingFlags = System.Reflection.BindingFlags;

namespace NekoRay.Tools; 

public partial interface IConsoleToken {
    public string Text { get; }

    private static Regex _stringExpression = StringExpression();
    private static Dictionary<string, Type> _registeredTypes = new();

    public static IConsoleToken[] Parse(string text) {
        var list = new List<IConsoleToken>();
        var matches = _stringExpression.Matches(text);
        foreach (Match match in matches)
        {
            foreach (Group group in match.Groups) {
                list.Add(GetTokenFor(group.Value));
            }
        }
        return list.ToArray();
    }

    private static Regex _tokenExpression = TokenExpression();

    public void Render();

    public static IConsoleToken GetTokenFor(string text) {
        if (!text.StartsWith('[')) return new ConsoleTokenString(text);
        var pop = text.StartsWith("[/");
        var list = new List<string>();
        var matches = _tokenExpression.Matches(text);
        foreach (Match match in matches)
        {
            foreach (Group group in match.Groups.Values) {
                list.Add(group.Value);
            }
        }
        list.RemoveAt(0); // fixme
        var type = GetTypeForToken(list[0]);
        list.RemoveAt(0);
        if (type is null) return new ConsoleTokenString(text);
        if (!type.IsAssignableTo(typeof(IPoppableConsoleToken)) && pop)
            throw new Exception($"attempt to pop a nonpoppable token {text}");
        var types = list.Select(_ => typeof(string)).ToArray();
        var constructor = type.GetConstructor(BindingFlags.Public, types);
        if (constructor is null)
            throw new Exception($"no constructor found for token {text}");
        var token = (IConsoleToken) constructor.Invoke(list.ToArray());
        if (pop) {
            ((IPoppableConsoleToken) token).Pop = pop;
        }
        return token;
    }

    static IConsoleToken() {
        Register<ConsoleTokenColor>("color");
    }

    public static void Register(string text, Type t) {
        _registeredTypes[text] = t;
    }

    public static void Register<T>(string text) => Register(text, typeof(T));

    public static Type? GetTypeForToken(string text) =>
        _registeredTypes.TryGetValue(text, out var value) ? value : null;

    [GeneratedRegex(@"((?:[^\[\]\\]|\\.)+)|(\[(?:\w+(?:\([^)]*\))?|\\/\w+)\])")]
    private static partial Regex StringExpression();
    [GeneratedRegex(@"(\w+)(?:\s*\(((?:[^,\)]+)?(?:,\s*[^,\)]+)*)\))?")]
    private static partial Regex TokenExpression();
}