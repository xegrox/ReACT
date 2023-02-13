using ReACT.Helpers;
using ReACT.Models;

namespace ReACT.Services;

public class ForestService
{

    private readonly AuthDbContext _context;

    public ForestService(AuthDbContext context)
    {
        _context = context;
    }

    public void InsertRandomTree(double recycledKg)
    {
        var trees = _context.ForestTrees.ToList();
        // Create plot bigger than current size to accomodate for new trees
        var dimens = trees.Aggregate((0, 0), (d, tree) => (
            (int) Math.Ceiling(Math.Max(d.Item1, Math.Abs(tree.X * 2) + 4000)),
            (int) Math.Ceiling(Math.Max(d.Item2, Math.Abs(tree.Y * 2) + 4000)))
        );
        var plot = new ForestPlot(dimens.Item1, dimens.Item2, 100);
        trees.ForEach(t => plot.InsertTree(t));
        var newTree = plot.InsertRandomTree(recycledKg);
        if (newTree == null) return;
        _context.ForestTrees.Add(newTree);
        _context.SaveChanges();
    }
}