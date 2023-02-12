using Microsoft.Build.Framework;

namespace ReACT.Models
{
    public class RecyclingType
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PointsPerKg { get; set; }
    }
}
