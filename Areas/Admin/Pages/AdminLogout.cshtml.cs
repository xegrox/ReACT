using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using System.Data;

namespace ReACT.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminLogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        public AdminLogoutModel(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("/AdminLogin", new { area = "Home" });
        }
        public async Task<IActionResult> OnPostDontLogoutAsync()
        {
            return RedirectToPage("/ManageUsers", new { area = "Admin" });
        }

    }
}
