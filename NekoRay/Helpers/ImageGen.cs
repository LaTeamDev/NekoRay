using ZeroElectric.Vinculum.Extensions;

namespace NekoRay; 

public static class ImageGen {
    public static Image Color(int width, int height, Color color) {
        return new Image {
            _image = Raylib.GenImageColor(width, height, color)
        };
    }

    public static Image GradientLinear(int width, int height, int direction, Color start, Color end) {
        return new Image {
            _image = Raylib.GenImageGradientLinear(width, height, direction, start, end)
        };
    }

    public static Image GradientRadial(int width, int height, int density, Color inner, Color outer) {
        return new Image {
            _image = Raylib.GenImageGradientRadial(width, height, density, inner, outer)
        };
    }
    
    public static Image GradientSquare(int width, int height, int direction, Color start, Color end) {
        return new Image {
            _image = Raylib.GenImageGradientSquare(width, height, direction, start, end)
        };
    }
    
    public static Image Checked(int width, int height, int checksX, int checksY, Color col1, Color col2) {
        return new Image {
            _image = Raylib.GenImageChecked(width, height, checksX, checksY, col1, col2)
        };
    }
    
    public static Image WhiteNoise(int width, int height, float factor) {
        return new Image {
            _image = Raylib.GenImageWhiteNoise(width, height, factor)
        };
    }
    
    public static Image PerlinNoise(int width, int height, int checksX, int checksY, float scale) {
        return new Image {
            _image = Raylib.GenImagePerlinNoise(width, height, checksX, checksY, scale)
        };
    }
    
    public static Image Cellular(int width, int height, int tilesize) {
        return new Image {
            _image = Raylib.GenImageCellular(width, height, tilesize)
        };
    }
    
    public unsafe static Image Text(int width, int height, string text) {
        var textPin = text.GcPin();
        var img = new Image {
            _image = Raylib.GenImageText(width, height, (sbyte*)textPin.AddrOfPinnedObject())
        };
        textPin.Free();
        return img;
    }
    
    public static Image Text(string text, int size, Color color) {
        return new Image {
            _image = Raylib.ImageText(text, size, color)
        };
    }
    
    public static Image Text(string text, Font font, float size, float spacing, Color color) {
        return new Image {
            _image = Raylib.ImageTextEx(font._font, text, size, spacing, color)
        };
    }
}