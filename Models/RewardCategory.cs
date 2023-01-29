namespace ReACT.Models;

public class RewardCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    
    public ICollection<Reward> Rewards { get; set; }
}