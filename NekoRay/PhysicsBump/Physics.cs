/*
 *  THIS IS A PORT OF LOVE2D LIBRARY BUMP.LUA BY KIKITO LICENSED UNDER
 *  MIT LICENSE
 *
 *  Copyright (c) 2014 Enrique García Cota
 *
 *  Permission is hereby granted, free of charge, to any person obtaining a
 *  copy of this software and associated documentation files (the
 *  "Software"), to deal in the Software without restriction, including
 *  without limitation the rights to use, copy, modify, merge, publish,
 *  distribute, sublicense, and/or sell copies of the Software, and to
 *  permit persons to whom the Software is furnished to do so, subject to
 *  the following conditions:
 *
 *  The above copyright notice and this permission notice shall be included
 *  in all copies or substantial portions of the Software.
 *
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 *  OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 *  MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 *  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
 *  CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 *  TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 *  SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NekoRay.PhysicsBump;

[Obsolete]
public static class Physics {
    private static double DELTA = 1e-10; //floating-point margin of error
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float Nearest(float x, float a, float b) {
        return Math.Abs(a - x) < Math.Abs(b - x) ? a : b;
    }
    
    public static Vector2 GetNearestCorner(this Rectangle rect, Vector2 point) {
        return new Vector2(Nearest(point.X, rect.X, rect.X+rect.Width), Nearest(point.Y, rect.Y, rect.Y + rect.Height));
    }

    public static (float ti1, float ti2, float nx1, float ny1, float nx2, float ny2)? GetSegmentIntersectionIndices(this Rectangle rect, float x1, float y1, float x2, float y2,
        float ti1 = 0f, float ti2 = 1f) {
        var (dx, dy) = (x2 - x1, y2 - y1);
        float nx, ny;
        var (nx1, ny1, nx2, ny2) = (0f, 0f, 0f, 0f);
        float p, q, r;

        for (var side = 1; side < 4; side++) {
            (nx, ny, p, q) = side switch {
                1 => (-1, 0, -dx, x1 - rect.X),
                2 => (1, 0, dx, rect.X + rect.Width - x1),
                3 => (0, -1, -dy, y1 - rect.Y),
                _ => (0, 1, dy, rect.Y + rect.Height - y1)
            };

            if (p == 0)
                if (q <= 0)
                    return null;
            r = q / p;
            if (p < 0) {
                if (r > ti2)
                    return null;
                if (r > ti1)
                    (ti1, nx1, ny1) = (r, nx, ny);
            }
            else {
                if (r > ti1)
                    return null;
                if (r > ti2)
                    (ti2, nx2, ny2) = (r, nx, ny);
            }
        }
        return (ti1, ti2, nx1, ny1, nx2, ny2);
    }

    public static Rectangle GetDiff(this Rectangle rect, Rectangle rect2) {
        return new Rectangle(rect2.X - rect.X - rect.Width,
            rect2.Y - rect.Y - rect.Height,
            rect.Width + rect2.Width,
            rect.Height + rect2.Height);
    }

    public static bool ContainsPoint(this Rectangle rect, Vector2 point) {
        return point.X - rect.X > DELTA && 
               point.Y - rect.Y > DELTA &&
               rect.X + rect.Width - point.X > DELTA  && 
               rect.Y + rect.Height - point.Y > DELTA;
    }

    public static bool IsIntersecting(this Rectangle rect, Rectangle rect2) {
        return rect.X < rect2.X + rect2.Width &&
               rect2.X < rect.X + rect.Width &&
               rect.Y < rect2.Y + rect2.Height &&
               rect2.Y < rect.Y + rect.Height;
    }

    public static float GetSquareDistance(this Rectangle rect, Rectangle rect2) {
        var dx = rect.X - rect2.X + (rect.Width - rect2.Width) / 2;
        var dy = rect.Y - rect2.Y + (rect.Height - rect2.Height) / 2;
        return dx * dx + dy * dy;
    }

    public static Collision? DetectCollision(this Rectangle rect, Rectangle rect2, Vector2? goal) {
        goal ??= new Vector2(rect.X, rect.Y);
        var result = new Collision();
        result.Move = new(goal.Value.X - rect.X, goal.Value.Y - rect.Y);
        var diff = rect.GetDiff(rect2);
        float? ti = null;
        if (diff.ContainsPoint(Vector2.Zero)) { // item was intersecting other
            var point = rect.GetNearestCorner(Vector2.Zero);
            var (wi, hi) = (Math.Min(rect.Width, Math.Abs(point.X)), Math.Min(rect.Height, Math.Abs(point.Y))); //area of intersection
            ti = -wi * hi; //ti is the negative area of intersection
            result.Overlaps = true;
        }
        else {
            var sii =
                rect.GetSegmentIntersectionIndices(0, 0, result.Move.X, result.Move.Y, -float.MaxValue, float.MaxValue);
            // item tunnels into other
            if (sii is not null &&
                sii.Value.ti1 < 1 &&
                Math.Abs(sii.Value.ti1 - sii.Value.ti2) >= DELTA && // special case for rect going through another rect's corner
                (0 < sii.Value.ti1 + DELTA ||
                 0 == sii.Value.ti1 && sii.Value.ti2 > 0)) {
                ti = sii.Value.ti1;
                result.Normal = new(sii.Value.nx1, sii.Value.ny1);
                result.Overlaps = false;
            }
        }
        if (ti is null) return null;

        if (result.Overlaps) {
            if (result.Move.X == 0 && result.Move.Y == 0) {
                var point = diff.GetNearestCorner(Vector2.Zero);
                if (Math.Abs(point.X) < Math.Abs(point.Y)) 
                    point.Y = 0;
                else 
                    point.X = 0;
                result.Normal = new(Math.Sign(point.X), Math.Sign(point.Y));
                result.Touch = new(rect.X + point.X, rect.Y + point.Y);
            }
            else {
                // intersecting and moving - move in the opposite direction
                var sii = diff.GetSegmentIntersectionIndices(0, 0, result.Move.X, result.Move.Y, -float.MaxValue, 1);
                if (sii is null) return null;
                result.Touch = new(rect.X + result.Move.X * sii.Value.ti1, rect.Y + result.Move.Y * sii.Value.ti1);
            }
        }
        else {
            result.Touch = new(rect.X + result.Move.X * ti.Value, rect.Y + result.Move.Y * ti.Value);
        }

        result.ItemRect = rect;
        result.OtherRect = rect2;
        result.Ti = ti.Value;
        return result;
    }
}