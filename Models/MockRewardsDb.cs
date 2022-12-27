namespace ReACT.Models;

public static class MockRewardsDb
{
    public static readonly List<RewardCategory> Categories = new()
    {
        new RewardCategory(1, "Gift Cards", "credit-card"),
        new RewardCategory(2, "Discounts", "tag")
    };
    
    public static readonly List<Reward> Rewards = new()
    {
        new Reward(1, 1, "Google Play", "https://placeimg.com/400/225/arch", ""),
        new Reward(2, 1, "Paypal", "https://placeimg.com/400/225/arch", ""),
        new Reward(3, 2, "Zalora", "https://placeimg.com/400/225/arch", ""),
    };

    public static readonly List<RewardVariant> Variants = new()
    {
        new RewardVariant(1, 1, "$10", 10000),
        new RewardVariant(2, 1, "$20", 19000),
        new RewardVariant(3, 1, "$50", 45000),
        new RewardVariant(4, 2, "$10", 12000),
        new RewardVariant(5, 2, "$20", 20000),
        new RewardVariant(6, 2, "$30", 30000),
        new RewardVariant(7, 3, "$20 OFF", 15000)
    };

    public static readonly List<RewardCode> Codes = new()
    {
        new RewardCode(1, 1, ""),
        new RewardCode(2, 1, ""),
        new RewardCode(3, 2, ""),
        new RewardCode(4, 2, ""),
        new RewardCode(5, 3, ""),
        new RewardCode(6, 3, ""),
        new RewardCode(7, 4, ""),
        new RewardCode(8, 4, ""),
        new RewardCode(9, 4, ""),
        new RewardCode(10, 6, ""),
        new RewardCode(11, 6, ""),
        new RewardCode(12, 7, "")
    };
}