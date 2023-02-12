namespace ReACT.Models;

public class Reward
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string RedeemUrl { get; set; }
    public int Popularity { get; set; }
    
    public ICollection<RewardVariant> Variants { get; set; }
    public RewardCategory Category { get; set; }
}