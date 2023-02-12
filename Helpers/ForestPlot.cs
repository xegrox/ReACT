using ReACT.Models;

namespace ReACT.Helpers;

// Based on Bridson's Poisson Disk Algorithm
public class ForestPlot
{
    public ForestPlot(double width, double height, double treeRadius)
    {
        _treeRadius = treeRadius;
        var cellSize = Math.Floor(treeRadius / Math.Sqrt(2));
        _treeGrid = new PointGrid<ForestTree>(width, height, cellSize);
    }

    private readonly double _treeRadius;
    private readonly PointGrid<ForestTree> _treeGrid;
    private readonly List<ForestTree> _points = new();
    private readonly List<ForestTree> _active = new();

    public IEnumerable<ForestTree> GetAll()
    {
        return _points.AsEnumerable();
    }
    
    public bool InsertTree(ForestTree tree)
    {
        var (cellX, cellY) = _treeGrid.GetCellPos(tree.X, tree.Y);
        if (!_treeGrid.IsCellEmpty(cellX, cellY)) return false;
        _treeGrid.SetCell(cellX, cellY, tree);
        _points.Add(tree);
        if (tree.Active) _active.Add(tree);
        return true;
    }
    
    public ForestTree? InsertRandomTree(double recycledKg)
    {
        if (_points.Count == 0)
        {
            var tree = CreateTree(0, 0, recycledKg);
            InsertTree(tree);
            return tree;
        }
        while (_active.Count > 0)
        {
            var randIndex = new Random().Next(_active.Count);
            var randTree = _active[randIndex];
            for (var tries = 0; tries < 30; tries++)
            {
                var theta = new Random().Next(360);
                var radius = new Random().NextDouble() * _treeRadius + _treeRadius;
                var x = randTree.X + radius * Math.Cos(Math.PI * theta / 180);
                var y = randTree.Y + radius * Math.Sin(Math.PI * theta / 180);
                if (!IsValidPoint(x, y)) continue;
                var newTree = CreateTree(x, y, recycledKg);
                InsertTree(newTree);
                return newTree;
            }
            
            randTree.Active = false;
            _active.Remove(randTree);
        }
        return null;
    }

    private bool IsValidPoint(double x, double y) 
    {
        if (Math.Abs(x) >= _treeGrid.Width/2 || Math.Abs(y) >= _treeGrid.Height/2) return false;
        var (cellX, cellY) = _treeGrid.GetCellPos(x, y);
        var i0 = Math.Max(cellX - 1, 0);
        var i1 = Math.Min(cellX + 1, _treeGrid.CellCountWidth - 1);
        var j0 = Math.Max(cellY - 1, 0);
        var j1 = Math.Min(cellY + 1, _treeGrid.CellCountHeight - 1);

        for (var i = i0; i <= i1; i++)
        {
            for (var j = j0; j <= j1; j++)
            {
                var tree = _treeGrid.GetCell(i, j);
                if (tree != null && CalcDist(x, y, tree.X, tree.Y) < _treeRadius) return false;
            }
        }

        return true;
    }

    private static double CalcDist(double x0, double y0, double x1, double y1)
    {
        return Math.Sqrt(Math.Pow(x0 - x1, 2) + Math.Pow(y0 - y1, 2));
    }

    private static ForestTree CreateTree(double x, double y, double recycledKg) {
        var values = Enum.GetValues<ForestTreeVariant>();
        return new ForestTree
        {
            X = x,
            Y = y,
            Active = true,
            Date = DateTime.Now,
            RecycledKg = recycledKg,
            Variant = values[new Random().Next(values.Length)]
        };
    }
}