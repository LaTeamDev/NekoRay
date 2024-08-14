using System.Numerics;
using Box2D.Interop;

namespace NekoRay.Physics2D; 

public class DebugDraw : Box2D.DebugDraw {
    private static DebugDraw? _instance;
    public static DebugDraw Instance {
        get {
            if (_instance is null) _instance = new DebugDraw();
            return _instance;
        }
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