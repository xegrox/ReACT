using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Areas.Admin.Views.Shared.ManageRewards;
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

    public IActionResult OnGetCategoryList()
    {
        return new JsonResult(MockRewardsDb.Categories.Select(category => new
        {
            id = category.Id,
            name = category.Name,
            icon = category.Icon
        }));
    }

    public IActionResult? OnGetEditRewardModalFrame(int rewardId)
    {
        var reward = MockRewardsDb.Rewards.Find(r => r.Id == rewardId);
        if (reward == null) return null;
        reward.Variants = MockRewardsDb.Variants.Where(v => v.RewardId == rewardId).ToList();
        return Partial("ManageRewards/_EditRewardModalPartial", reward);
    }

    public IActionResult OnPostCategory(AddCategoryModalPartialModel model)
    {
        if (ModelState.IsValid)
        {
            var nextId = MockRewardsDb.Categories.Last().Id + 1;
            MockRewardsDb.Categories.Add(new RewardCategory(nextId, model.Name, model.Icon));
            var partial = Partial("ManageRewards/_AddCategoryModalPartial");
            partial.ViewData.ModelState.Clear();
            partial.ViewData["success"] = true;
            return partial;
        }
        else
        {
            var partial = Partial("ManageRewards/_AddCategoryModalPartial");
            return partial;
        }
    }
}