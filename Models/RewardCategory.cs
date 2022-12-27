namespace ReACT.Models;

public class RewardCategory
{
    public RewardCategory(int id, string name, string icon)
    {
        Id = id;
        Name = name;
        Icon = icon;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public List<Reward> Rewards { get; set; } = new List<Reward>();
}