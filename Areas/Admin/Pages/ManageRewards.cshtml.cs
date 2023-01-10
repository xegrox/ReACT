using FuzzySharp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Helpers;
using ReACT.Models;

namespace ReACT.Areas.Admin.Pages;

public class ManageRewards : PageModel
{
    public IActionResult OnGet(int? categoryId, bool all, string? search)
    {
        IEnumerable<Reward> rewards;
        if (all)
        {
            rewards = search == null ? MockRewardsDb.Rewards.AsEnumerable()
                : MockRewardsDb.Rewards.Where(r => r.Name.FuzzyMatch(search));
        }
        else
        {
            categoryId ??= MockRewardsDb.Categories.FirstOrDefault()?.Id;
            if (categoryId == null) return RedirectToPage("ManageRewards", new { all = true });
            var categoryExists = MockRewardsDb.Categories.Any(c => c.Id == categoryId);
            if (!categoryExists) return RedirectToPage();
            rewards = MockRewardsDb.Rewards.Where(r => r.CategoryId == categoryId && (search == null || r.Name.FuzzyMatch(search)));
            ViewData["activeCategoryId"] = categoryId;
        }
        
        foreach (var reward in rewards)
        {
            reward.Variants = MockRewardsDb.Variants.Where(v => v.RewardId == reward.Id).ToList();
        }
        
        ViewData["search"] = search;
        ViewData["rewards"] = rewards;
        ViewData["categories"] = MockRewardsDb.Categories;
        return Page();
    }
}