using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;

namespace ReACT.Areas.Admin.Pages;

public class ManageRewards : PageModel
{
    public IActionResult OnGet(int? categoryId, bool all)
    {
        IEnumerable<Reward> rewards;
        if (all)
        {
            rewards = MockRewardsDb.Rewards.AsEnumerable();
        }
        else
        {
            categoryId ??= MockRewardsDb.Categories.FirstOrDefault()?.Id;
            if (categoryId == null) return RedirectToPage("ManageRewards", new { all = true });
            var categoryExists = MockRewardsDb.Categories.Any(c => c.Id == categoryId);
            if (!categoryExists) return RedirectToPage();
            rewards = MockRewardsDb.Rewards.Where(r => r.CategoryId == categoryId);
            ViewData["activeCategoryId"] = categoryId;
        }
        
        foreach (var reward in rewards)
        {
            reward.Variants = MockRewardsDb.Variants.Where(v => v.RewardId == reward.Id).ToList();
        }

        ViewData["rewards"] = rewards;
        ViewData["categories"] = MockRewardsDb.Categories;
        return Page();
    }
}