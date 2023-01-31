using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Helpers;
using ReACT.Models;

namespace ReACT.Areas.User.Pages;

[Authorize]
public class Forest : PageModel
{
    public void OnGet()
    {
        var trees = MockForestDb.Trees;
        var dimens = MockForestDb.Trees.Aggregate((0, 0), (d, tree) => (
            (int) Math.Ceiling(Math.Max(d.Item1, Math.Abs(tree.X * 2) + 2000)),
            (int) Math.Ceiling(Math.Max(d.Item2, Math.Abs(tree.Y * 2) + 200)))
        );
        var plot = new ForestPlot(dimens.Item1, dimens.Item2, 100);
        trees.ForEach(t => plot.InsertTree(t));
        var newTree = plot.InsertRandomTree();
        if (newTree != null)
        {
            MockForestDb.Trees.Add(newTree);
            dimens.Item1 = (int) Math.Ceiling(Math.Max(dimens.Item1, Math.Abs(newTree.X * 2) + 2000));
            dimens.Item2 = (int) Math.Ceiling(Math.Max(dimens.Item2, Math.Abs(newTree.Y * 2) + 2000));
        }
        ViewData["trees"] = plot.GetAll();
        ViewData["width"] = dimens.Item1;
        ViewData["height"] = dimens.Item2;
        var checkpoints = new List<int>();
        foreach (var c in GetCheckpoints())
        {
            checkpoints.Add(c);
            if (c > trees.Count) break;
        }
        ViewData["checkpoints"] = checkpoints;
    }

    public IEnumerable<int> GetCheckpoints()
    {
        var c = 1;
        while (true)
        {
            yield return c;
            switch (c)
            {
                case 1:
                    c = 5;
                    break;
                case 5:
                    c = 10;
                    break;
                default:
                    c += 10;
                    break;
            }
        }
    }
}