namespace ReACT.Models
{
    public class CycleOfWaste_prizes
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Points_Worth { get; set; }
        public bool VisibleToUser { get; set; }
        public int? RewardId { get; set; }
        public Reward? Reward { get; set; }
    }
}
