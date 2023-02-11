namespace ReACT.Models
{
    public class CycleOfWaste_prizes
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool VisibleToUser { get; set; }
        public int Points { get; set; }
        public int? RewardVariantId { get; set; }
        public RewardVariant? RewardVariant { get; set; }
    }
}
