using System.Numerics;
using NekoRay;
using ZeroElectric.Vinculum;

namespace TowerDefence.Gameplay.Buildings;

public class BuildingInventory
{
    private List<BuildingTemplate> _templates = new List<BuildingTemplate>();
    private int _currentTemplate = 0;
    private Sprite? _currentSprite = null;
    private string _lastSprite = "";
    
    private Sprite? CurrentSprite
    {
        get
        {
            if (CurrentTemplate == null) return null;
            if (_lastSprite == CurrentTemplate?.Texture) return _currentSprite;
            _lastSprite = CurrentTemplate?.Texture;
            _currentSprite = Data.GetSprite(_lastSprite);
            return _currentSprite;
        }
    }
    
    public BuildingTemplate? CurrentTemplate
    {
        get
        {
            if (_templates.Count <= 0) return null;
            if (_currentTemplate >= _templates.Count || _currentTemplate < 0) _currentTemplate = 0;
            return _templates[_currentTemplate];
        }
    }

    public void ChoseBuilding(int id)
    {
        _currentTemplate = id;
    }

    public void Add(BuildingTemplate template)
    {
        _templates.Add(template);
    }

    public BaseBuilding? Place(Vector2 place)
    {
        if (CurrentTemplate == null) return null;
        BaseBuilding building = CurrentTemplate?.Create();
        building.Transform.Position = new Vector3(place.X, place.Y, 0);
        building.Initialize();
        _templates.RemoveAt(_currentTemplate);
        _lastSprite = "";
        _currentTemplate = 0;
        return building;
    }

    public void DrawTemplate(Vector2 position)
    {
        if(CurrentTemplate == null || CurrentSprite == null) return;
        Sprite sprite = CurrentSprite;
        var origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        sprite.Draw(position, null, origin, 0, new Color(0, 255, 0, 128));
        //Raylib.DrawRectangleV(position, Vector2.One * 50, Raylib.RAYWHITE);
    }
}