using ZeroElectric.Vinculum;
using System.Numerics;
using NekoRay;
using ZeroElectric.Vinculum.Extensions;

namespace FlappyPegasus.Gui; 

public class ShadowedText : Text {
    public NekoRay.Font ShadowFont;
    public Color ShadowColor;
    public override void Render() {
        ShadowFont.Draw(
            TextString, 
            new Vector2(Transform.Position.X, Transform.Position.Y), 
            Origin, 
            Transform.Rotation.YawPitchRollAsVector3().Z,
            Font.BaseSize*Transform.LocalScale.X,
            Spacing,
            ShadowColor);
        base.Render();
    }
}