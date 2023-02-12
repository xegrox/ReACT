using System.Runtime.Intrinsics.X86;

namespace ReACT.Models
{
    public class CycleOfWaste
    {
        public int Id { get; set; }
        
        public int Points_spent { get; set; }

        public int Points_earned { get; set; }

        public DateTime ActivityDate { get; set; } = DateTime.Now;

        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }
    }
}
