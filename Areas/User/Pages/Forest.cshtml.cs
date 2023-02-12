using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Helpers;
using ReACT.Models;
using ReACT.Services;

namespace ReACT.Areas.User.Pages;

[Authorize(Roles = "Admin, User")]

public class Forest : PageModel
{
    public void OnGet([FromServices] ForestService forest)
    {
        forest.InsertRandomTree(20);
        var trees = MockForestDb.Trees;
        var dimens = MockForestDb.Trees.Aggregate((0, 0), (d, tree) => (
            (int) Math.Ceiling(Math.Max(d.Item1, Math.Abs(tree.X * 2) + 4000)),
            (int) Math.Ceiling(Math.Max(d.Item2, Math.Abs(tree.Y * 2) + 4000)))
        );
        ViewData["trees"] = trees;
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