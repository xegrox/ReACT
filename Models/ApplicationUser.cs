using Microsoft.AspNetCore.Identity;

namespace ReACT.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? Profile { get; set; }
        public bool PublicPrivate { get; set; }
        public CycleOfWaste? cowid { get; set; }
        public int Total_Points { get; set; } = 0;
        public int postComments_counter { get; set; } = 0;
        public int totalChances_counter { get; set; } = 3;


        public List<Collection>? Collections { get; set; }

    }
}

