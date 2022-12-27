namespace ReACT.Models;

public class RewardVariant
{
    public RewardVariant(int id, int rewardId, string name, int points)
    {
        Id = id;
        RewardId = rewardId;
        Name = name;
        Points = points;
    }

    public int Id { get; set; }
    public int RewardId { get; set; }
    public string Name { get; set; }
    public int Points { get; set; }
    public List<RewardCode> Codes = new List<RewardCode>();
}