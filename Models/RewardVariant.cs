namespace ReACT.Models;

public class RewardVariant
{
    public int Id { get; set; }
    public int RewardId { get; set; }
    public string Name { get; set; }
    public int Points { get; set; }
    public int Popularity { get; set; }
    
    public ICollection<RewardCode> Codes { get; set; }
    public Reward Reward { get; set; }
}