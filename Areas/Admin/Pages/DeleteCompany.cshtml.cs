using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;

namespace ReACT.Areas.Admin.Pages
{
    public class DeleteCompanyModel : PageModel
    {
        private UserManager<ApplicationUser> UserManager { get; set; }
        private AuthDbContext authDbContext;

        [BindProperty]
        public String Id { get; set; }
        public ApplicationUser MyUser { get; set; } = new();

        public DeleteCompanyModel(UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
        {
            UserManager = userManager;
            this.authDbContext = authDbContext;
        }

        public IActionResult OnGet(string id)
        {
            ApplicationUser? user = authDbContext.Users.FirstOrDefault(x => x.Id.Equals(id));
            MyUser = user;
            Id = user.Id;

            return Page();
        }

        public async Task<IActionResult> OnPostDelete_CompanyAsync(string id)
        {
            ApplicationUser? user = authDbContext.Users.FirstOrDefault(x => x.Id.Equals(id));
            MyUser = user;

            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToPage("/ManageCompanies");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return Page();
        }

        public IActionResult OnPostNo_Delete_Company()
        {
            return RedirectToPage("/ManageCompanies");
        }
    }
}