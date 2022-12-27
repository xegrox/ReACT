using Microsoft.AspNetCore.Mvc;
using ReACT.Areas.User.Pages.Components.RedeemModalRewardList;
using ReACT.Models;

namespace ReACT.Areas.User.ViewComponents.RedeemModal;

public class RedeemModalRewardListViewComponent : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(int categoryId)
    {
        var wrap = (Action f) => { f(); return true; };
        var categoryName = MockRewardsDb.Categories.Find(category => category.Id == categoryId)!.Name;
        var rewards = MockRewardsDb.Rewards.Where(reward => reward.CategoryId == categoryId && wrap(() =>
        {
            reward.Variants = MockRewardsDb.Variants.Where(variant => variant.RewardId == reward.Id).ToList();
        }));
        return Task.FromResult<IViewComponentResult>(View(new RedeemModalRewardListViewComponentModel(
            categoryName,
            rewards
        )));
    }
}