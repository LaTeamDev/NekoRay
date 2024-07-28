using System.Numerics;
using NekoRay;
using ZeroElectric.Vinculum;
using Image = NekoRay.Image;

namespace HotlineSPonyami.Tools;

public struct Tile
{
    public byte FloorId;
    public byte Down;
    public byte Left;
    public byte Pile;
}

public class TileField : IBinarySavable
{
    private Tile[,] _tiles;
    private int _sizeX, _sizeY; 
    public int SizeX => _sizeX; public int SizeY => _sizeY;

    public const int TextureSize = 32;
    
    public TileField(int sizeX, int sizeY)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
        _tiles = new Tile[sizeX,sizeY];
        Clear();
    }

    public void SetTileFloor(int x, int y, byte id)
    {
        if(x < 0 || y < 0 || x >= _sizeX || y >= _sizeY) return;
        _tiles[x, y].FloorId = id;
    }

    public byte GetTileFloor(int x, int y)
    {
        if(x < 0 || y < 0 || x >= _sizeX || y >= _sizeY) return 0;
        return _tiles[x, y].FloorId;
    }

    public void UpdatePiles()
    {
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                
            }
        }
    }
    
    public void SetLine(int startX, int startY, int endX, int endY, byte wallId)
    {
        if (startX != endX)
        {
            startY--;
            for (int x = startX; x < endX; x++)
            {
                if(x < 0 || startY < 0 || x >= _sizeX || startY >= _sizeY) continue;
                _tiles[x, startY].Down = wallId;
            }
            return;
        }
        for (int y = startY; y < endY; y++)
        {
            if(startX < 0 || y < 0 || startX >= _sizeX || y >= _sizeY) continue;
            _tiles[startX, y].Left = wallId;
        }
    }


    public void Draw(bool drawGrid)
    {
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                Rectangle source = new Rectangle(0,0, TextureSize, TextureSize);
                Rectangle destination = new Rectangle(x * TextureSize,y * TextureSize, TextureSize, TextureSize);
                byte tile = _tiles[x, y].FloorId;
                if(tile > 0) UnpackedTextures.GetFloorTextureById(GetTileFloor(x, y)).Draw(source, destination, Vector2.One / 2f, 0, Raylib.WHITE);

                if (_tiles[x, y].Left != 0)
                {
                    Rectangle leftSource = new Rectangle(0, 0, 7, 32);
                    Rectangle leftDestination = new Rectangle(x * 32, y * 32, 7, 32);
                    UnpackedTextures.GetWallTextureById(_tiles[x, y].Left).Draw(leftSource, leftDestination, Vector2.Zero, 0, Raylib.WHITE);
                }

                if (_tiles[x, y].Down != 0)
                {
                    Rectangle downSource = new Rectangle(16, 48 - 7, 32, 7);
                    Rectangle downDestination = new Rectangle(x * 32, y * 32 + 32 - 7, 32, 7);
                    UnpackedTextures.GetWallTextureById(_tiles[x, y].Down).Draw(downSource, downDestination, Vector2.Zero, 0, Raylib.WHITE);
                }

                if (_tiles[x, y].Pile !=0)
                {
                    Rectangle pileSource = new Rectangle(0, 48 - 7, 7, 7);
                    Rectangle pileDestination = new Rectangle(x * 32, y * 32 + 32 - 7, 7, 7);
                    UnpackedTextures.GetWallTextureById(_tiles[x, y].Pile).Draw(pileSource, pileDestination, Vector2.Zero, 0, Raylib.WHITE);
                }
            }
        }

        if (!drawGrid) return;
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
                writer.Write(_tiles[x, y].FloorId);
                writer.Write(_tiles[x, y].Down);
                writer.Write(_tiles[x, y].Left);
                writer.Write(_tiles[x, y].Pile);
            }
        }
    }

    public void Load(BinaryReader reader)
    {
        _sizeX = reader.ReadInt32();
        _sizeY = reader.ReadInt32();
        _tiles = new Tile[_sizeX, _sizeY];
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                _tiles[x, y] = new Tile();
                _tiles[x, y].FloorId = reader.ReadByte();
                _tiles[x, y].Down = reader.ReadByte();
                _tiles[x, y].Left = reader.ReadByte();
                _tiles[x, y].Pile = reader.ReadByte();
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
                //TODO: FIXME
                finalMap.DrawPixel(x, y, new Color(_tiles[x, y].FloorId, _tiles[x, y].Left, (byte)0, (byte)255));
            }
        }
        return finalMap;
    }

    public void Clear() {
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                _tiles[x, y] = new Tile
                {
                    FloorId = 0,
                    Down = 0,
                    Left = 0,
                    Pile = 0
                };
            }
        }
    }
    
    public Tile this[int x, int y]
    {
        get => _tiles[x,y];
        //set => _tiles[x,y] = value;
    }
}