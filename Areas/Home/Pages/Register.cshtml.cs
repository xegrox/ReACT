using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.ViewModels;
using System.Security.Claims;

namespace ReACT.Areas.Home.Pages
{
    public class RegisterModel : PageModel
    {
        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }
        private readonly RoleManager<IdentityRole> roleManager;



        [BindProperty]
        public Register RModel { get; set; }
        public RegisterModel(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager; 
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = RModel.Email,
                    Email = RModel.Email,
                    FirstName = RModel.FirstName,
                    LastName = RModel.LastName,
                    Address = RModel.Address,
                    PublicPrivate = RModel.PublicPrivate,
                };

                //Create the Admin role if NOT exist
                //Create the User role if NOT exist
                IdentityRole roleAdmin = await roleManager.FindByIdAsync("Admin");
                IdentityRole roleUser = await roleManager.FindByIdAsync("User");
                if (roleAdmin == null)
                {
                    IdentityResult result2 = await roleManager.CreateAsync(new IdentityRole("Admin"));
                    if (!result2.Succeeded)
                    {
                        ModelState.AddModelError("", "Create role Admin failed");
                    }
                }
                if (roleUser == null)
                {
                    IdentityResult result3 = await roleManager.CreateAsync(new IdentityRole("User"));
                    if (!result3.Succeeded)
                    {
                        ModelState.AddModelError("", "Create role User failed");
                    }
                }

                var result = await userManager.CreateAsync(user, RModel.Password);
                if (result.Succeeded)
                {
                    if (user.Email == "Admin@ReAct.com")
                    {
                        result = await userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        result = await userManager.AddToRoleAsync(user, "User");
                    }
                    await signInManager.SignInAsync(user, false);
                    return RedirectToPage("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }
    }
}
