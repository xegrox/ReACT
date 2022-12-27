namespace ReACT.Models;

public class Reward
{
    public Reward(int id, int categoryId, string name, string imageUrl, string redeemUrl)
    {
        Id = id;
        CategoryId = categoryId;
        Name = name;
        ImageUrl = imageUrl;
        RedeemUrl = redeemUrl;
    }

    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string RedeemUrl { get; set; }
    public List<RewardVariant> Variants { get; set; } = new();
}