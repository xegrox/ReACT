namespace ReACT.Models
{
    public class PointsHistory
    {
        public int id { get; set; }
        public string userId { get; set; }
        public DateTime activity_datetime { get; set; } = DateTime.Now;
        public int points_spent { get; set; }
        public string activity_description { get; set; }
        public int points_gained { get; set; }
    }
}
