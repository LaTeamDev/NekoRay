using ZeroElectric.Vinculum;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay; 

//TODO
public static class ImageDraw {
    public static unsafe void ClearBackground(this Image image, Color color) {
        var pin = image._image.GcPin();
        Raylib.ImageClearBackground((RayImage*)pin.AddrOfPinnedObject(), color);
        pin.Free();
    }
}