using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.ViewModels;

namespace ReACT.Areas.Home.Pages
{
    public class AdminLoginModel : PageModel
    {
        [BindProperty]
        public AdminLogin AModel { get; set; }

        private readonly SignInManager<ApplicationUser> signInManager;

        private ApplicationUser user;
        private UserManager<ApplicationUser> userManager { get; }
        public AdminLoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var identityResult = await signInManager.PasswordSignInAsync(AModel.Email, AModel.Password, AModel.RememberMe, false);
                if (identityResult.Succeeded)
                {
                        return RedirectToPage("/ManageUsers", new { area = "Admin" });
                }
                ModelState.AddModelError("", "Username or Password incorrect");
            }
            return Page();
        }
    }
}
