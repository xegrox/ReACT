using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.ViewModels;
using System.Data;

namespace ReACT.Areas.User.Pages
{
    [Authorize(Roles = "Admin, User")]
    public class ChangePasswordModel : PageModel
    {
        private UserManager<ApplicationUser> UserManager { get; set; }

        [BindProperty]
        public ChangePassword CModel { get; set; }

        public ChangePasswordModel(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostChange_PasswordAsync()
        {
            if (ModelState.IsValid)
            {
                var userId = UserManager.GetUserId(User);
                var user = await UserManager.FindByIdAsync(userId);

                if (user != null)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(user, CModel.CurrentPassword, CModel.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToPage("/UserDetails");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
            }

            return Page();
        }

        public IActionResult OnPostNo_Change_Password()
        {
            return RedirectToPage("/UserDetails");
        }
    }
}
