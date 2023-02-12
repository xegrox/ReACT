using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.ViewModels;

namespace ReACT.Areas.Admin.Pages
{
    public class AddUserModel : PageModel
    {
        [BindProperty]
        public AddUser AUModel { get; set; }
        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }
        private readonly RoleManager<IdentityRole> roleManager;

        public AddUserModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAdd_UserAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = AUModel.Email,
                    Email = AUModel.Email,
                    FirstName = AUModel.FirstName,
                    LastName = AUModel.LastName,
                    Address = AUModel.Address,
                    PublicPrivate = AUModel.PublicPrivate,
                };

                //Create the User role if NOT exist
                IdentityRole roleUser = await roleManager.FindByIdAsync("User");
                if (roleUser == null)
                {
                    IdentityResult result3 = await roleManager.CreateAsync(new IdentityRole("User"));
                    if (!result3.Succeeded)
                    {
                        ModelState.AddModelError("", "Create role User failed");
                    }
                }

                var result = await userManager.CreateAsync(user, AUModel.Password);
                if (result.Succeeded)
                {
                    result = await userManager.AddToRoleAsync(user, "User");
                    return RedirectToPage("/ManageUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }

        public IActionResult OnPostNo_Add_User()
        {
            return RedirectToPage("/ManageUsers");
        }
    }
}
