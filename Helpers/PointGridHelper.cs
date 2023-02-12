namespace ReACT.Helpers;

public class PointGrid<T>
{
    public PointGrid(double width, double height, double cellSize)
    {
        Width = width;
        Height = height;
        _cellSize = cellSize;
        
        CellCountWidth = Convert.ToInt32(Math.Ceiling(width / _cellSize)) + 1;
        CellCountHeight = Convert.ToInt32(Math.Ceiling(height / _cellSize)) + 1;
        _grid = new T?[CellCountWidth][];
        for (var i = 0; i < _grid.Length; i++)
        {
            _grid[i] = new T?[CellCountHeight];
        }
    }

    public double Width { get; }
    public double Height { get; }
    public int CellCountWidth { get; }
    public int CellCountHeight { get; }
    
    private readonly T?[][] _grid;
    private readonly double _cellSize;

    public (int, int) GetCellPos(double x, double y)
    {
        var gridX = Convert.ToInt32(Math.Floor((Width/2 + x) / _cellSize));
        var gridY = Convert.ToInt32(Math.Floor((Height/2 + y) / _cellSize));
        return (gridX, gridY);
    }

    public void SetCell(int cellX, int cellY, T value)
    {
        _grid[cellX][cellY] = value;
    }

    public T? GetCell(int cellX, int cellY) => _grid[cellX][cellY];

    public bool IsCellEmpty(int cellX, int cellY) => _grid[cellX][cellY] == null;
}