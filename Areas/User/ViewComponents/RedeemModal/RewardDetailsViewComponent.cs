using Microsoft.AspNetCore.Mvc;
using ReACT.Areas.User.Views.Shared.Components.RedeemModalRewardDetails;
using ReACT.Models;

namespace ReACT.Areas.User.ViewComponents.RedeemModal;

public class RedeemModalRewardDetailsViewComponent : ViewComponent
{
    public Task<IViewComponentResult?> InvokeAsync(int rewardId)
    {
        var reward = MockRewardsDb.Rewards.Find(reward => reward.Id == rewardId);
        if (reward == null) return Task.FromResult<IViewComponentResult?>(null);
        reward.Variants = MockRewardsDb.Variants.Where(variant => variant.RewardId == rewardId).ToList();
        var categoryName = MockRewardsDb.Categories.Find(category => category.Id == reward.CategoryId)!.Name;
        return Task.FromResult<IViewComponentResult?>(View(new RedeemModalRewardDetailsModel(categoryName, reward)));
    }
}