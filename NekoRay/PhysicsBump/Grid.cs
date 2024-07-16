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

using System.Drawing;
using System.Numerics;
using Rectangle = ZeroElectric.Vinculum.Rectangle;

namespace NekoRay.PhysicsBump; 

[Obsolete]
public static class Grid {
    public static Vector2 ToWorld(float cellSize, Point pos) {
        return new((pos.X - 1) * cellSize, (pos.X - 1) * cellSize);
    }

    public static Point ToCell(float cellSize, Vector2 pos) {
        return new((int)MathF.Floor(pos.X / cellSize) + 1, (int)MathF.Floor(pos.Y / cellSize) + 1);
    }
    
    // grid_traverse* functions are based on "A Fast Voxel Traversal Algorithm for Ray Tracing",
    // by John Amanides and Andrew Woo - http://www.cse.yorku.ca/~amana/research/grid.pdf
    // It has been modified to include both cells when the ray "touches a grid corner",
    // and with a different exit condition
    
    public static (int stepX, double dx, double tx) TraverseInitStep(float cellSize, float ct, float t1, float t2)
    {
        var v = t2 - t1;
        if (v > 0)
        {
            return (1, cellSize / v, ((ct + v) * cellSize - t1) / v);
        }
        if (v < 0)
        {
            return (-1, -cellSize / v, ((ct + v - 1) * cellSize - t1) / v);
        }
        return (0, double.PositiveInfinity, double.PositiveInfinity);
    }

    public delegate void TraverseFunc(Point vec);
    
    public static void Traverse(float cellSize, Vector2 pos1, Vector2 pos2, TraverseFunc f)
    {
        var cell1 = ToCell(cellSize, pos1);
        var cell2 = ToCell(cellSize, pos2);
        var (stepX, dx, tx) = TraverseInitStep(cellSize, cell1.X, pos1.X, pos2.X);
        var (stepY, dy, ty) = TraverseInitStep(cellSize, cell2.Y, pos1.Y, pos2.Y);
        var cell = cell1;

        f(cell);

        while (Math.Abs(cell.X - cell2.X) + Math.Abs(cell.Y - cell2.Y) > 1)
        {
            if (tx < ty)
            {
                tx += dx;
                cell.X += stepX;
                f(cell);
            }
            else
            {
                if (tx == ty)
                    f(cell with {X = cell.X + stepX});
                ty += dy;
                cell.Y += stepY;
                f(cell);
            }
        }

        if (cell.X != cell2.X || cell.Y != cell2.Y)
            f(cell);
    }

    public static Rectangle ToCellRect(this Rectangle rect, float cellSize)
    {
        var c = ToCell(cellSize, new Vector2(rect.X, rect.Y));
        int cr = (int)Math.Ceiling((rect.X + rect.Width) / cellSize);
        int cb = (int)Math.Ceiling((rect.y + rect.Height) / cellSize);
        return new(c.X, c.Y, cr - c.X + 1, cb - c.Y + 1);
    }
}