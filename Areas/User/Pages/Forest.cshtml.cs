using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ReACT.Models;
using ReACT.Services;

namespace ReACT.Areas.User.Pages;

[Authorize(Roles = "Admin, User"), IgnoreAntiforgeryToken]
public class Forest : PageModel
{
    public async Task OnGet([FromServices] ForestService forest, [FromServices] UserManager<ApplicationUser> userManager)
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

        var user = await userManager.GetUserAsync(User);
        var claim = (await userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "ForestClaimedCheckpoints");
        if (claim != null) ViewData["claimedCheckpoints"] = JsonConvert.DeserializeObject<List<int>>(claim.Value);
    }

    public async Task<IActionResult> OnPost([FromServices] UserManager<ApplicationUser> userManager, int checkpointNo)
    {
        var checkpoint = GetCheckpoints().ElementAt(checkpointNo);
        var level = MockForestDb.Trees.Count;
        if (checkpoint > level) return new BadRequestResult();
        
        var user = await userManager.GetUserAsync(User);
        var claim = (await userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "ForestClaimedCheckpoints");
        var claimedCheckpoints = claim != null ? JsonConvert.DeserializeObject<List<int>>(claim.Value) ?? new List<int>() : new List<int>();
        if (claimedCheckpoints.Contains(checkpoint)) return new BadRequestResult();
        
        user.Total_Points += 200;
        await userManager.UpdateAsync(user);
        claimedCheckpoints.Add(checkpoint);
        if (claim != null) await userManager.RemoveClaimAsync(user, claim);
        await userManager.AddClaimAsync(user, new Claim("ForestClaimedCheckpoints", JsonConvert.SerializeObject(claimedCheckpoints)));
        return new OkResult();
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