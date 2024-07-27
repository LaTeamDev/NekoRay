using ZeroElectric.Vinculum;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay; 

//TODO
public static class ImageDraw {
    public static unsafe void ClearBackground(this Image image, Color color) {
        var pin = image._image.GcPin();
        Raylib.ImageClearBackground((ZeroElectric.Vinculum.Image*)pin.AddrOfPinnedObject(), color);
        pin.Free();
    }

    public static unsafe void Draw(this Image destination, Image src, Rectangle srcRect, Rectangle dstRect,
        Color tint)
    {
        var pin = destination._image.GcPin();
        Raylib.ImageDraw((ZeroElectric.Vinculum.Image*)pin.AddrOfPinnedObject(), src._image, srcRect, dstRect, tint);
        pin.Free();
    }

    public static unsafe void DrawPixel(this Image destination, int posX, int posY, Color color)
    {
        var pin = destination._image.GcPin();
        Raylib.ImageDrawPixel((ZeroElectric.Vinculum.Image*)pin.AddrOfPinnedObject(), posX, posY, color);
        pin.Free();
    }
}