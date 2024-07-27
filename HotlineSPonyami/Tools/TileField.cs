using System.Numerics;
using NekoRay;
using ZeroElectric.Vinculum;
using Image = NekoRay.Image;

namespace HotlineSPonyami.Tools;

public class TileField : IBinarySavable
{
    private byte[,] _tiles;
    private int _sizeX, _sizeY; 
    public int SizeX => _sizeX; public int SizeY => _sizeY;

    public const int TextureSize = 32;
    
    public TileField(int sizeX, int sizeY)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
        _tiles = new byte[sizeX,sizeY];
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                _tiles[x, y] = 0;
            }
        }
    }

    public void SetTile(int x, int y, byte id)
    {
        if(x < 0 || y < 0 || x >= _sizeX || y >= _sizeY) return;
        _tiles[x, y] = id;
    }

    public byte GetTile(int x, int y)
    {
        if(x < 0 || y < 0 || x >= _sizeX || y >= _sizeY) return 0;
        return _tiles[x, y];
    }


    public void Draw()
    {
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                Rectangle source = new Rectangle(0,0, TextureSize, TextureSize);
                Rectangle destination = new Rectangle(x * TextureSize,y * TextureSize, TextureSize, TextureSize);
                byte tile = _tiles[x, y];
                if(tile > 0) UnpackedTextures.GetFloorTextureById(GetTile(x, y)).Draw(source, destination, Vector2.One / 2f, 0, Raylib.WHITE);
            }
        }

        for (int x = 0; x <= _sizeX; x++)
            Raylib.DrawLine(x * TextureSize, 0, x * TextureSize, _sizeY * TextureSize, Raylib.GRAY);
        for (int y = 0; y <= _sizeY; y++)
            Raylib.DrawLine(0, y * TextureSize, _sizeX * TextureSize, y * TextureSize, Raylib.GRAY);
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(_sizeX);
        writer.Write(_sizeY);
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                writer.Write(_tiles[x, y]);
            }
        }
    }

    public void Load(BinaryReader reader)
    {
        _sizeX = reader.ReadInt32();
        _sizeY = reader.ReadInt32();
        _tiles = new byte[_sizeX, _sizeY];
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                _tiles[x, y] = reader.ReadByte();
            }
        }
    }

    public Image Export()
    {
        Image finalMap = ImageGen.Color(SizeX, SizeY, Raylib.BLACK);
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                finalMap.DrawPixel(x, y, new Color(_tiles[x, y], (byte)0, (byte)0, (byte)255));
            }
        }
        return finalMap;
    }
}