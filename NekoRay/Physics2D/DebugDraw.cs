using System.Numerics;
using NekoRay.Tools;

namespace NekoRay.Physics2D; 

public class DebugDraw : Box2D.DebugDraw {
    private static DebugDraw? _instance;
    public static DebugDraw Instance {
        get {
            if (_instance is null) _instance = new DebugDraw();
            return _instance;
        }
    }
    [ConVariable("phys_draw")]
    [ConTags("cheat")]
    public static bool ConvarDraw { get; set; }

    [ConVariable("phys_drawbounds")]
    [ConTags("cheat")]
    public static bool ConvarUseDrawingBounds {
        get => Instance.UseDrawingBounds;
        set => Instance.UseDrawingBounds = value;
    }

    [ConVariable("phys_drawshapes")]
    [ConTags("cheat")]
    public static bool ConvarDrawShapes {
        get => Instance.DrawShapes;
        set => Instance.DrawShapes = value;
    }

    [ConVariable("phys_drawcontacts")]
    [ConTags("cheat")]
    public static bool ConvarDrawContacts {
        get => Instance.DrawContacts;
        set => Instance.DrawContacts = value;
    }

    [ConVariable("phys_drawjoints")]
    [ConTags("cheat")]
    public static bool ConvarDrawJoints {
        get => Instance.DrawJoints;
        set => Instance.DrawJoints = value;
    }

    [ConVariable("phys_drawmass")]
    [ConTags("cheat")]
    public static bool ConvarDrawMass {
        get => Instance.DrawMass;
        set => Instance.DrawMass = value;
    }

    [ConVariable("phys_drawcontactimpulses")]
    [ConTags("cheat")]
    public static bool ConvarDrawContactImpulses {
        get => Instance.DrawContactImpulses;
        set => Instance.DrawContactImpulses = value;
    }

    [ConVariable("phys_drawcontactnormals")]
    [ConTags("cheat")]
    public static bool ConvarDrawContactNormals {
        get => Instance.DrawContactNormals;
        set => Instance.DrawContactNormals = value;
    }

    [ConVariable("phys_drawfrictionimpulses")]
    [ConTags("cheat")]
    public static bool ConvarDrawFrictionImpulses {
        get => Instance.DrawFrictionImpulses;
        set => Instance.DrawFrictionImpulses = value;
    }

    [ConVariable("phys_drawgraphcolors")]
    [ConTags("cheat")]
    public static bool ConvarDrawGraphColors {
        get => Instance.DrawGraphColors;
        set => Instance.DrawGraphColors = value;
    }

    [ConVariable("phys_drawjointextras")]
    [ConTags("cheat")]
    public static bool ConvarDrawJointExtras {
        get => Instance.DrawJointExtras;
        set => Instance.DrawJointExtras = value;
    }

    [ConVariable("phys_drawaabbs")]
    [ConTags("cheat")]
    public static bool ConvarDrawAABBs {
        get => Instance.DrawAABBs;
        set => Instance.DrawAABBs = value;
    }

    private DebugDraw() {
        DrawString = DebugDrawDrawString;
        DrawCircle = DebugDrawDrawCircle;
        DrawCapsule = DebugDrawDrawCapsule;
        DrawPoint = DebugDrawDrawPoint;
        DrawPolygon = DebugDrawDrawPolygon;
        DrawSegment = DebugDrawDrawSegment;
        DrawTransform = DebugDrawDrawTransform;
        DrawSolidCapsule = DebugDrawDrawSolidCapsule;
        DrawSolidCircle = DebugDrawDrawSolidCircle;
        DrawSolidPolygon = DebugDrawDrawSolidPolygon;
        UseDrawingBounds = false;
        DrawShapes = true;
        DrawContacts = true;
        DrawJoints = true;
        DrawMass = true;
        DrawContactImpulses = true;
        DrawContactNormals = true;
        DrawFrictionImpulses = true;
        DrawGraphColors = true;
        DrawJointExtras = true;
        DrawAABBs = true;
    }

    private static unsafe SolidPolygon DebugDrawDrawSolidPolygon =
        (transform, pos, count, color, sth) => {
            //idk how to draw polygons in raylib honestly
            Raylib.DrawLineStrip(pos, count, color.ToRaylib());
        };

    private static unsafe SolidCircle DebugDrawDrawSolidCircle =
        (transform, radius, color, sth) => {
            Raylib.DrawCircleV(transform.Position, radius, color.ToRaylib());
        };

    private static unsafe SolidCapsule DebugDrawDrawSolidCapsule =
        (start, end, radius, color, sth) => {
            Raylib.DrawCircleV(start, radius, color.ToRaylib());
            Raylib.DrawCircleV(start, radius, color.ToRaylib());
            Raylib.DrawLineEx(start, end, radius, color.ToRaylib());
        };

    private static unsafe Transform DebugDrawDrawTransform = (transform, sth) => { };

    private static unsafe Segment DebugDrawDrawSegment = (start, end, color, sth) => {
        Raylib.DrawLineV(start, end, color.ToRaylib());
    };

    private static unsafe Polygon DebugDrawDrawPolygon = (pos, count, color, sth) => {
        Raylib.DrawLineStrip(pos, count, color.ToRaylib());
    };

    private static unsafe Point DebugDrawDrawPoint = (pos, radius, color, sth) => {
        Raylib.DrawRectangleV(pos - Vector2.One * radius / 2, Vector2.One * radius, color.ToRaylib());
    };

    private static unsafe Capsule DebugDrawDrawCapsule =
        (start, end, radius, color, sth) => {
            Raylib.DrawCircleLinesV(start, radius, color.ToRaylib());
            Raylib.DrawCircleLinesV(start, radius, color.ToRaylib());
            var normal = Vector2.Normalize(end - start);
            var perp = new Vector2(normal.Y, -normal.X);
            Raylib.DrawLineV(start + perp * radius, end + perp * radius, color.ToRaylib());
            Raylib.DrawLineV(start - perp * radius, end - perp * radius, color.ToRaylib());
        };

    private static unsafe Circle DebugDrawDrawCircle = (pos, radius, color, sth) => {
        Raylib.DrawCircleLinesV(pos, radius, color.ToRaylib());
    };

    private static unsafe String DebugDrawDrawString = (Vector2 pos, string str, void* sth) => {
        Raylib.DrawText(str, pos.X, pos.Y, 10f, Raylib.WHITE);
    };
}