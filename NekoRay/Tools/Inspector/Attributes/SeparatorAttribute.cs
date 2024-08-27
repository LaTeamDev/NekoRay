namespace NekoRay.Tools;

public class SeparatorAttribute(string text) : Attribute {
    public readonly string Text = text;
}