using FuzzySharp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Helpers;
using ReACT.Models;

namespace ReACT.Areas.Admin.Pages;

public class ManageRewards : PageModel
{
    public IActionResult OnGet(int? categoryId, bool all, string? search, int pageIndex = 1)
    {
        if (pageIndex < 1) return RedirectToPage(new {categoryId, all, search});
        const int rewardsPerPage = 5;
        List<Reward> rewards;
        if (all)
        {
            rewards = search == null ? MockRewardsDb.Rewards
                : MockRewardsDb.Rewards.Where(r => r.Name.FuzzyMatch(search)).ToList();
        }
        else
        {
            categoryId ??= MockRewardsDb.Categories.FirstOrDefault()?.Id;
            if (categoryId == null) return RedirectToPage("ManageRewards", new { all = true });
            var categoryExists = MockRewardsDb.Categories.Any(c => c.Id == categoryId);
            if (!categoryExists) return RedirectToPage();
            rewards = MockRewardsDb.Rewards.Where(r => r.CategoryId == categoryId && (search == null || r.Name.FuzzyMatch(search))).ToList();
            ViewData["activeCategoryId"] = categoryId;
        }

        var totalRewardsCount = rewards.Count;
        if (rewardsPerPage * (pageIndex - 1) + 1 > totalRewardsCount) return RedirectToPage(new {categoryId, all, search});
        rewards = rewards.Skip(rewardsPerPage * (pageIndex-1)).Take(rewardsPerPage).ToList();
        foreach (var reward in rewards)
        {
            reward.Variants = MockRewardsDb.Variants.Where(v => v.RewardId == reward.Id).ToList();
        }

        ViewData["rewardsPerPage"] = rewardsPerPage;
        ViewData["pageIndex"] = pageIndex;
        ViewData["pageCount"] = (totalRewardsCount - 1) / rewardsPerPage + 1;
        ViewData["search"] = search;
        ViewData["rewards"] = rewards;
        ViewData["categories"] = MockRewardsDb.Categories;
        return Page();
    }
}