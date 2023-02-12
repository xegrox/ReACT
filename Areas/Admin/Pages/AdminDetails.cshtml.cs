using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using System.Data;

namespace ReACT.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminDetailsModel : PageModel
    {
        private UserManager<ApplicationUser> UserManager { get; set; }
        private AuthDbContext authDbContext;


        public ApplicationUser user { get; set; }
        public AdminDetailsModel(UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
        {
            UserManager = userManager;
            this.authDbContext = authDbContext;
        }
        public IActionResult OnGet()
        {
            string userId = UserManager.GetUserId(User);
            ApplicationUser? currentUser = authDbContext.Users.FirstOrDefault(x => x.Id.Equals(userId));
            user = currentUser;
            return Page();        
        }
    }
}
