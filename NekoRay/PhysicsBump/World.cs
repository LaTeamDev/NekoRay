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
public class World {
    private Dictionary<object, Rectangle> Rects = new();
    private Dictionary<int, Dictionary<int, Cell>> Rows = new();
    private HashSet<Cell> NonEmptyCells = new();
    private float CellSize;
    private Dictionary<string, PhysicsResponses.PhysicsResponse> Responses = new();

    public World(float cellSize)
    {
        this.CellSize = cellSize;
    }

    private class Cell
    {
        public int ItemCount;
        public int x, y;
        public Dictionary<object, bool> Items;
    }

    // Implement the sorting methods
    private static int SortByWeight(Collision a, Collision b)
    {
        return a.Ti.CompareTo(b.Ti);
    }

    private static int SortByTiAndDistance(Collision a, Collision b)
    {
        if (a.Ti == b.Ti)
        {
            // Implement rect_getSquareDistance method
            var ad = a.ItemRect.GetSquareDistance(a.OtherRect);
            var bd = b.ItemRect.GetSquareDistance(b.OtherRect);
            return ad.CompareTo(bd);
        }
        return a.Ti.CompareTo(b.Ti);
    }

    private void AddItemToCell(object item, Point pos)
    {
        if (!Rows.ContainsKey(pos.Y))
            Rows[pos.Y] = new Dictionary<int, Cell>();

        var row = Rows[pos.Y];
        if (!row.ContainsKey(pos.X))
            row[pos.X] = new Cell { ItemCount = 0, x = pos.X, y = pos.Y, Items = new Dictionary<object, bool>() };

        var cell = row[pos.X];
        NonEmptyCells.Add(cell);
        if (cell.Items.ContainsKey(item)) return;
        cell.Items[item] = true;
        cell.ItemCount++;
    }

    private bool RemoveItemFromCell(object item, Point pos)
    {
        if (!Rows.ContainsKey(pos.Y) || !Rows[pos.Y].ContainsKey(pos.X) || !Rows[pos.Y][pos.X].Items.ContainsKey(item))
            return false;

        var cell = Rows[pos.Y][pos.X];
        cell.Items.Remove(item);
        cell.ItemCount--;
        if (cell.ItemCount == 0)
            NonEmptyCells.Remove(cell);
        return true;
    }

    // Implement other private methods like getDictItemsInCellRect, getCellsTouchedBySegment, etc.

    public void AddResponse(PhysicsResponses.PhysicsResponse response, string name)
    {
        Responses[name] = response;
    }

    // Implement other public methods like Project, CountCells, HasItem, GetItems, etc.

    public void Add(object item, Rectangle rect)
    {
        if (Rects.ContainsKey(item))
            throw new Exception($"Item {item} added to the world twice.");

        Rects[item] = rect;

        var cellRect = rect.ToCellRect(CellSize);
        for (var cy = cellRect.X; cy < cellRect.Y + cellRect.Height; cy++)
        {
            for (var cx = cellRect.X; cx < cellRect.X + cellRect.Width; cx++)
            {
                AddItemToCell(item, new Point((int)cx, (int)cy));
            }
        }
    }

    public void Remove(object item)
    {
        var rect = GetRect(item);

        Rects.Remove(item);
        var cellRect = rect.ToCellRect(CellSize);
        for (var cy = cellRect.X; cy < cellRect.Y + cellRect.Height; cy++)
        {
            for (var cx = cellRect.X; cx < cellRect.X + cellRect.Width; cx++)
            {
                RemoveItemFromCell(item, new Point((int)cx, (int)cy));
            }
        }
    }
/*
    public void Update(object item, Rectangle upd)
    {
        if (!Rects.ContainsKey(item))
            throw new Exception($"Item {item} must be added to the world before updating. Use World.Add(item, rect to add it first.");

        var rect = Rects[item];
        if (rect.x != upd.x || rect.y != upd.y || rect.width != upd.width || rect.height != upd.height)
        {
            var rect1 = upd.ToCellRect(CellSize);
            var rect2 = upd.ToCellRect(CellSize);

            if (rect1.x != rect2.x || rect1.y != rect2.y || rect2.width != rect2.width || rect1.height != rect2.height)
            {
                var (cr1, cb1) = (rect1.x + rect1.width - 1, rect1.y + rect1.height - 1);
                var (cr2, cb2) = (rect2.x + rect2.width - 1, rect2.y + rect2.height - 1);
                var cyOut = Math.Max(rect1.y, rect2.y) - 1;

                while (cyOut <= Math.Min(cb1, cb2) + 1)
                {
                    cyOut++;
                    var cxOut = Math.Max(rect1.x, rect2.x) - 1;
                    while (cxOut <= Math.Min(cr1, cr2) + 1)
                    {
                        cxOut++;
                        if (cxOut < rect1.x || cxOut > cr1 || cyOut < rect1.y || cyOut > cb1)
                        {
                            AddItemToCell(item, new Point((int)cxOut, (int)cyOut));
                        }
                        else if (cxOut < rect2.x || cxOut > cr2 || cyOut < rect2.y || cyOut > cb2)
                        {
                            RemoveItemFromCell(item, new Point((int)cxOut, (int)cyOut));
                        }
                    }
                }
            }

            rect.x = upd.x;
            rect.y = upd.y;
            rect.width = upd.width;
            rect.height = upd.height;
        }
    }

    public (Vector2 goal, Collision[] cols, int len) Move(object item, Vector2 goal, Func<object, object, string> filter)
    {
        // Temporary placeholders for unimplemented methods
        Func<object, float, float, float, float, (float, float, Collision[], int)> checkFilter = (item, x, y, w, h) => (0, 0, new Collision[0], 0);
        Func<float, float, float, float, float, float> GetTouchedCells = (x1, y1, x2, y2, cellSize) => 0;

        var (actualX, actualY, collisions, len) = CheckFilter(item, goalX, goalY, rects[item].w, rects[item].h);

        if (len > 0)
        {
            var (x, y, w, h) = GetRect(item);
            float projected_coords = GetTouchedCells(x, y, goalX, goalY, cellSize);

            Array.Sort(collisions, 0, len, new Comparison<Collision>(SortByTiAndDistance));

            projected_coords = GetTouchedCells(goalX, goalY, actualX, actualY, cellSize);
        }

        Update(item, actualX, actualY, rects[item].w, rects[item].h);

        return (actualX, actualY, collisions, len);
    }

    public (float, float, Collision[], int) Check(object item, float goalX, float goalY, Func<object, object, string> filter)
    {
        var (x, y, w, h) = GetRect(item);

        var collisions = new List<Collision>();

        var (cl, ct, cw, ch) = GridToCellRect(cellSize, goalX, goalY, w, h);

        var dictItemsInCellRect = GetDictItemsInCellRect(cl, ct, cw, ch);

        foreach (var other in dictItemsInCellRect.Keys)
        {
            if (other != item)
            {
                var responseType = filter(item, other);

                if (responseType != null)
                {
                    var (ox, oy, ow, oh) = GetRect(other);
                    var collision = RectDetectCollision(x, y, w, h, ox, oy, ow, oh, goalX, goalY);

                    if (collision != null)
                    {
                        collision.other = other;
                        collision.item = item;
                        collision.type = responseType;
                        collisions.Add(collision);
                    }
                }
            }
        }

        collisions.Sort(SortByWeight);

        var (goalX_, goalY_, cols, len) = ResolveCollisions(collisions, goalX, goalY, w, h, responses, filter);

        return (goalX_, goalY_, cols, len);
    }*/
    private Rectangle GetRect(object item)
    {
        if (!Rects.TryGetValue(item, out var rect))
            throw new Exception($"Item {item} must be added to the world before getting its rect. Use World.Add(item, rect) to add it first.");

        return rect;
    }

}