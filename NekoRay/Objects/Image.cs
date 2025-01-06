using NekoLib.Filesystem;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay; 

public class Image : NekoObject {
    internal RayImage _image;
    
    internal Image() { }

    public int Height => _image.height;
    public int Width => _image.width;

    public static unsafe Image Load(string filename) {
        var file = Files.GetFile(filename).ReadBinary();
        fixed (byte* ptr = file) //TODO: investigate is this a valid way to load image (pointer never used again) or it is better to use GCHandle
            return FromMemory(Path.GetExtension(filename), ptr, file.Length);
    }

    public static Image LoadRaw(string filename, int width, int height, PixelFormat format, int headerSize) {
        return new Image {
            _image = Raylib.LoadImageRaw(filename, width, height, format, headerSize)
        };
    }

    public static Image LoadSvg(string filename, int w, int h) {
        return new Image {
            _image = Raylib.LoadImageSvg(filename, w, h)
        };
    }

    public static unsafe Image LoadAnimated(string filename, int* frames) {
        return new Image {
            _image = Raylib.LoadImageAnim(filename, frames)
        };
    }

    public static unsafe Image FromMemory(string fileType, byte* fileData, int dataSize) {
        return new Image {
            _image = Raylib.LoadImageFromMemory(fileType, fileData, dataSize)
        };
    }

    public static Image FromTexture(Texture texture) {
        return new Image {
            _image = Raylib.LoadImageFromTexture(texture._texture)
        };
    }

    public static Image FromScreen() {
        return new Image {
            _image = Raylib.LoadImageFromScreen()
        };
    }
    
    public bool IsReady => Raylib.IsImageReady(_image);

    public override void Dispose() {
        Raylib.UnloadImage(_image);
    }

    public void Export(string fileName) {
        Raylib.ExportImage(_image, fileName);
    }

    public unsafe byte* ExportToMemory(string fileType, out int fileSize) {
        return Raylib.ExportImageToMemory(_image, fileType, out fileSize);
    }

    public Image Copy(Image image) {
        return new Image {
            _image = Raylib.ImageCopy(_image)
        };
    }
    
    public Image Copy(Image image, Rectangle rectangle) {
        return new Image {
            _image = Raylib.ImageFromImage(_image, rectangle)
        };
    }

    public unsafe void SetFormat(PixelFormat format) {
        var pinnedImage = _image.GcPin();
        Raylib.ImageFormat((RayImage*)pinnedImage.AddrOfPinnedObject(), format);
        pinnedImage.Free();
    }
    
    public unsafe void ToPowerOfTwo(Color fill) {
        var pinnedImage = _image.GcPin();
        Raylib.ImageToPOT((RayImage*)pinnedImage.AddrOfPinnedObject(), fill);
        pinnedImage.Free();
    }

    public unsafe void Crop(Rectangle crop) {
        var pinnedImage = _image.GcPin();
        Raylib.ImageCrop((RayImage*)pinnedImage.AddrOfPinnedObject(), crop);
        pinnedImage.Free();
    }

    public unsafe void AlphaCrop(float threshold) {
        var pinnedImage = _image.GcPin();
        Raylib.ImageAlphaCrop((RayImage*)pinnedImage.AddrOfPinnedObject(), threshold);
        pinnedImage.Free();
    }
    
    public unsafe void AlphaClear(Color color, float threshold) {
        var pinnedImage = _image.GcPin();
        Raylib.ImageAlphaClear((RayImage*)pinnedImage.AddrOfPinnedObject(), color, threshold);
        pinnedImage.Free();
    }
    
    public unsafe void AlphaMask(Image alphaMask) {
        var pinnedImage = _image.GcPin();
        Raylib.ImageAlphaMask((RayImage*)pinnedImage.AddrOfPinnedObject(), alphaMask._image);
        pinnedImage.Free();
    }

    public unsafe void AlphaPremultiply() {
        var pinnedImage = _image.GcPin();
        Raylib.ImageAlphaPremultiply((RayImage*)pinnedImage.AddrOfPinnedObject());
        pinnedImage.Free();
    }
    
    public unsafe void BlurGaussian(int blurSize) {
        var pinnedImage = _image.GcPin();
        Raylib.ImageBlurGaussian((RayImage*)pinnedImage.AddrOfPinnedObject(), blurSize);
        pinnedImage.Free();
    }
    
    public unsafe void Resize(int width, int height) {
        var pinnedImage = _image.GcPin();
        Raylib.ImageResize((RayImage*)pinnedImage.AddrOfPinnedObject(), width, height);
        pinnedImage.Free();
    }
    
    public unsafe void ResizeNearest(int width, int height) {
        var pinnedImage = _image.GcPin();
        Raylib.ImageResizeNN((RayImage*)pinnedImage.AddrOfPinnedObject(), width, height);
        pinnedImage.Free();
    }
    
    public unsafe void ResizeCanvas(int width, int height, int offsetX, int offestY, Color color) {
        var pinnedImage = _image.GcPin();
        Raylib.ImageResizeCanvas((RayImage*)pinnedImage.AddrOfPinnedObject(), width, height, offsetX, offestY, color);
        pinnedImage.Free();
    }
    
    public unsafe void GenerateMipmaps() {
        var pinnedImage = _image.GcPin();
        Raylib.ImageMipmaps((RayImage*)pinnedImage.AddrOfPinnedObject());
        pinnedImage.Free();
    }
    
    public unsafe void Dither(int rBpp, int gBpp, int bBpp, int aBpp) {
        var pinnedImage = _image.GcPin();
        Raylib.ImageDither((RayImage*)pinnedImage.AddrOfPinnedObject(), rBpp, gBpp, bBpp, aBpp);
        pinnedImage.Free();
    }
    
    public unsafe void FlipVertical() {
        var pinnedImage = _image.GcPin();
        Raylib.ImageFlipVertical((RayImage*)pinnedImage.AddrOfPinnedObject());
        pinnedImage.Free();
    }
    
    public unsafe void FlipHorizontal() {
        var pinnedImage = _image.GcPin();
        Raylib.ImageFlipHorizontal((RayImage*)pinnedImage.AddrOfPinnedObject());
        pinnedImage.Free();
    }
    
    public unsafe void Rotate(int degrees) {
        var pinnedImage = _image.GcPin();
        Raylib.ImageRotate((RayImage*)pinnedImage.AddrOfPinnedObject(), degrees);
        pinnedImage.Free();
    }
    
    public unsafe void RotateCW() {
        var pinnedImage = _image.GcPin();
        Raylib.ImageRotateCW((RayImage*)pinnedImage.AddrOfPinnedObject());
        pinnedImage.Free();
    }
    
    public unsafe void RotateCCW() {
        var pinnedImage = _image.GcPin();
        Raylib.ImageRotateCCW((RayImage*)pinnedImage.AddrOfPinnedObject());
        pinnedImage.Free();
    }
    
    public unsafe void Tint(Color color) {
        var pinnedImage = _image.GcPin();
        Raylib.ImageColorTint((RayImage*)pinnedImage.AddrOfPinnedObject(), color);
        pinnedImage.Free();
    }
    
    public unsafe void Invert() {
        var pinnedImage = _image.GcPin();
        Raylib.ImageColorInvert((RayImage*)pinnedImage.AddrOfPinnedObject());
        pinnedImage.Free();
    }
    
    public unsafe void Grayscale() {
        var pinnedImage = _image.GcPin();
        Raylib.ImageColorGrayscale((RayImage*)pinnedImage.AddrOfPinnedObject());
        pinnedImage.Free();
    }
    
    public unsafe void Contrast(float contrast) {
        var pinnedImage = _image.GcPin();
        Raylib.ImageColorContrast((RayImage*)pinnedImage.AddrOfPinnedObject(), contrast);
        pinnedImage.Free();
    }
    
    public unsafe void Brightness(int brightnes) {
        var pinnedImage = _image.GcPin();
        Raylib.ImageColorBrightness((RayImage*)pinnedImage.AddrOfPinnedObject(), brightnes);
        pinnedImage.Free();
    }
    
    /* TODO:
    public unsafe Color* GetColors() {
        var colors = Raylib.LoadImageColors(_image);
        return colors;
    }
    
    public unsafe Color* GetPalette(int maxPaletteSize, out int colorCount) {
        int colCount;
        var colors = Raylib.LoadImagePalette(_image, maxPaletteSize, &colCount);
        colorCount = colCount;
        return colors;
    }
    */
    public Rectangle GetAlphaBorder(float threshold) {
        return Raylib.GetImageAlphaBorder(_image, threshold);
    }

    public Color GetColorAt(int x, int y) {
        return Raylib.GetImageColor(_image, x, y);
    }

    public Texture ToTexture() {
        return new Texture {
            _texture = Raylib.LoadTextureFromImage(_image)
        };
    }
}