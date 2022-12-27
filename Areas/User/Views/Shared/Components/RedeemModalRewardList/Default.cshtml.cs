using ReACT.Models;

namespace ReACT.Areas.User.Pages.Components.RedeemModalRewardList;

public class RedeemModalRewardListViewComponentModel
{
    public RedeemModalRewardListViewComponentModel(string categoryName, IEnumerable<Reward> rewards)
    {
        CategoryName = categoryName;
        Rewards = rewards;
    }
    
    public readonly string CategoryName;
    public readonly IEnumerable<Reward> Rewards;
}