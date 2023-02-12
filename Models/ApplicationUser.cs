using Microsoft.AspNetCore.Identity;

namespace ReACT.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Address { get; set; }
        public string? Profile { get; set; }
        public bool? PublicPrivate { get; set; }
        public int Total_Points { get; set; } = 0;
        public int chances_Default { get; set; } = 3;
        public int chances_Free { get; set; } = 0;
        public int postComments_counter { get; set; } = 0;
        public int chance_CommentTask { get; set; } = 0;
        public bool used_CommentTask { get; set; } = false;
        public int chance_TreeTask { get; set; } = 0;
        public bool used_TreeTask { get; set; } = false;


        public List<Collection>? Collections { get; set; }
        public List<Thread>? Threads { get; set; }
        public List<CycleOfWaste>? CycleOfWaste_records { get; set; }
    }
}

