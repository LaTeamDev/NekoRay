using System.Collections.ObjectModel;
using System.Numerics;
using HotlineSPonyami.Tools.EditorTools;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using ZeroElectric.Vinculum;
using Texture = NekoRay.Texture;
using Timer = NekoRay.Timer;

namespace HotlineSPonyami.Tools;

public class EditorScene : BaseScene, IBinarySavable
{
    private DragCamera _camera; public DragCamera Camera => _camera;
    private TileField _filed; public TileField Field => _filed;
    public int _selectedTool = 0; public BaseTool SelectedTool => _tools[_selectedTool];
    private List<BaseTool> _tools = new List<BaseTool>();

    public EditorScene(int sizeX, int sizeY)
    {
        _filed = new TileField(sizeX, sizeY);
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
        FloodFillTool flood = new FloodFillTool();
        flood.Initialize(this);
        _tools.Add(flood);
        new GameObject("Inspector").AddComponent<Inspector>();
        new GameObject("Tools").AddComponent<Tools>().Initialize(this);
        _camera = new GameObject("DragCamera").AddComponent<DragCamera>();
        new GameObject("EditorMenu").AddComponent<EditorMenu>().Initialize(this);
        base.Initialize();
    }

    public void GetCurrentCell(out int x, out int y)
    {
        Vector2 mousePos = _camera.ScreenToWorld(Raylib.GetMousePosition());
        mousePos /= (float)TileField.TextureSize;
        x = (int)MathF.Floor(mousePos.X);
        y = (int)MathF.Floor(mousePos.Y);
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
        _filed?.Draw();
        SelectedTool.OnDraw();
        Raylib.EndMode2D();
    }

    [ConCommand("editor_open")]
    [ConDescription("Opens editor. Usage:(editor_open sizex sizey)")]
    public static void OpenEditor(string sizeX, string sizeY) {
        SceneManager.LoadScene(new EditorScene(int.Parse(sizeX), int.Parse(sizeY)));
    }

    public void Save(BinaryWriter writer)
    {
        _filed.Save(writer);
    }

    public void Load(BinaryReader reader)
    {
        _filed.Load(reader);
    }
}