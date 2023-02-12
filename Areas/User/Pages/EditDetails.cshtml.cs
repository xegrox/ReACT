using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.ViewModels;

namespace ReACT.Areas.User.Pages
{
    public class EditDetailsModel : PageModel
    {

        [BindProperty]
        public EditUser EModel { get; set; }
        private UserManager<ApplicationUser> UserManager { get; set; }
        private AuthDbContext authDbContext;
        public ApplicationUser user { get; set; }

        public EditDetailsModel(UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
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
            //if (user.PublicPrivate == true)
            //{
            //    accType = "Private";
            //}
            //else
            //{
            //    accType = "Public";
            //}
            return Page();
        }

        public async Task<IActionResult> OnPostEdit_UserAsync()
        {
            Console.WriteLine(ModelState.Values.SelectMany(v => v.Errors));
            var userId = UserManager.GetUserId(User);
            var user = await UserManager.FindByIdAsync(userId);
            if (ModelState.IsValid)
            {
                user.FirstName = EModel.FirstName;
                user.LastName = EModel.LastName;
                user.Address = EModel.Address;
                user.PublicPrivate = EModel.PublicPrivate;

                var result = await UserManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToPage("/UserDetails");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return Page();
        }

        public IActionResult OnPostNo_Edit_User()
        {
            return RedirectToPage("/UserDetails");
        }
    }
}
