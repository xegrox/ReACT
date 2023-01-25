using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;

namespace ReACT.Areas.User.Pages
{
    public class UserDetailsModel : PageModel
    {
        private UserManager<ApplicationUser> UserManager { get;}
        private AuthDbContext authDbContext;

        [BindProperty]
        public ApplicationUser user { get; set; }
        public UserDetailsModel(UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
        {
            UserManager = userManager;
            this.authDbContext = authDbContext;
        }


        public IActionResult OnGet()
        {
            //var user =  UserManager.FindByIdAsync(ApplicationUser.GetUserId());
            string userId = UserManager.GetUserId(User);
            ApplicationUser? currentUser = authDbContext.Users.FirstOrDefault(x => x.Id.Equals(userId));
            user = currentUser;
            return Page();
            //ApplicationUser user = UserManager.FindByIdAsync(userId).Result;
        }
    }
}
