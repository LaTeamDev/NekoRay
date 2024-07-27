using System.Collections.ObjectModel;
using System.Numerics;
using HotlineSPonyami.Tools.EditorTools;
using NekoLib.Core;
using NekoRay;
using ZeroElectric.Vinculum;
using Texture = NekoRay.Texture;
using Timer = NekoRay.Timer;

namespace HotlineSPonyami.Tools;

public class EditorScene : BaseScene
{
    private int[,] _tiles;
    private int _sizeX, _sizeY; 
    public int SizeX => _sizeX; public int SizeY => _sizeY;
    private DragCamera _camera; public DragCamera Camera => _camera;

    private int _selectedTool = 0; public BaseTool SelectedTool => _tools[_selectedTool];

    private List<KeyValuePair<string, Texture>> _loadedTextures = new List<KeyValuePair<string, Texture>>(); //Потому что мне нужны только string и Texture и я не хочу создавать под это отдельную структуру
    private List<BaseTool> _tools = new List<BaseTool>();
    
    public const int TextureSize = 32;

    public EditorScene(int sizeX, int sizeY)
    {   
        _sizeX = sizeX;
        _sizeY = sizeY;
        _tiles = new int[sizeX,sizeY];
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                _tiles[x, y] = -1;
            }
        }
    }

    public ReadOnlyCollection<BaseTool> GetAllTools()
    {
        return _tools.AsReadOnly();
    }

    public void SelectTool(int id)
    {
        if(id < 0 || id >= _tools.Count) return;
        _selectedTool = id;
    }
    
    public override void Initialize()
    {
        TilePointTool pointTool = new TilePointTool();
        pointTool.Initialize(this);
        _tools.Add(pointTool);
        RectangleFill fill = new RectangleFill();
        fill.Initialize(this);
        _tools.Add(fill);
        new GameObject("Inspector").AddComponent<Inspector>();
        Tools tools = new GameObject("Tools").AddComponent<Tools>();
        _camera = new GameObject("DragCamera").AddComponent<DragCamera>();
        base.Initialize();
        tools.Initialize(this);
    }

    public void GetCurrentCell(out int x, out int y)
    {
        Vector2 mousePos = _camera.ScreenToWorld(Raylib.GetMousePosition());
        mousePos /= (float)TextureSize;
        x = (int)MathF.Floor(mousePos.X);
        y = (int)MathF.Floor(mousePos.Y);
    }

    public void SetTextureToTile(int x, int y, string name)
    {
        if(x < 0 || y < 0 || x >= _sizeX || y >= _sizeY) return;
        if (name == "")
        {
            _tiles[x, y] = -1;
            return;
        }
        int id = -1;
        for (int i = 0; i < _loadedTextures.Count; i++)
        {
            if (_loadedTextures[i].Key == name)
            {
                id = i;
                break;
            }
        }
        if (id != -1)
        {
            _tiles[x, y] = id;
            return;
        }
        
        _loadedTextures.Add(new KeyValuePair<string, Texture>(name, Data.GetTexture("data/textures/unpacked/floors/" + name)));
        _tiles[x, y] = _loadedTextures.Count - 1;
    }
    public Texture GetTextureByTile(int x, int y)
    {
        if(x < 0 || y < 0 || x >= _sizeX || y >= _sizeY) return Data.GetTexture("null");
        int id = _tiles[x, y];
        if(id < 0 || id >= _loadedTextures.Count) return Data.GetTexture("null");
        return _loadedTextures[id].Value;
    }
    
    public override void Update()
    {
        SelectedTool?.OnUpdate();
        
        base.Update();
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.BeginMode2D(_camera.Camera);
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                Rectangle source = new Rectangle(0,0, TextureSize, TextureSize);
                Rectangle destination = new Rectangle(x * TextureSize,y * TextureSize, TextureSize, TextureSize);
                int tile = _tiles[x, y];
                if(tile != -1) GetTextureByTile(x, y).Draw(source, destination, Vector2.One / 2f, 0, Raylib.WHITE);
            }
        }

        for (int x = 0; x <= _sizeX; x++)
            Raylib.DrawLine(x * TextureSize, 0, x * TextureSize, _sizeY * TextureSize, Raylib.GRAY);
        for (int y = 0; y <= _sizeY; y++)
            Raylib.DrawLine(0, y * TextureSize, _sizeX * TextureSize, y * TextureSize, Raylib.GRAY);
        SelectedTool.OnDraw();
        Raylib.EndMode2D();
    }
}