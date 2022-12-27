using ReACT.Models;

namespace ReACT.Areas.User.Views.Shared.Components.RedeemModalRewardDetails;

public class RedeemModalRewardDetailsModel
{
    public RedeemModalRewardDetailsModel(string categoryName, Reward reward)
    {
        CategoryName = categoryName;
        Reward = reward;
    }
    
    public readonly string CategoryName;
    public readonly Reward Reward;
}