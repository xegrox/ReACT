using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.ViewModels;

namespace ReACT.Areas.Home.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }


        private readonly SignInManager<ApplicationUser> signInManager;
        private UserManager<ApplicationUser> userManager { get; }
        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
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
                var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,LModel.RememberMe, false);
                if (identityResult.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(LModel.Email);
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles.Contains("User"))
                    {
                        return RedirectToPage("/Dashboard", new { area = "User" });
                    }
                    else if (roles.Contains("Company"))
                    {
                        return RedirectToPage("/CompanyDetails", new { area = "Company" });
                    }
                }
                ModelState.AddModelError("", "Username or Password incorrect");
            }
            return Page();
        }
    }
}
