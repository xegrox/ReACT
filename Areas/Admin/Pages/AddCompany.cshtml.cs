using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.ViewModels;
using System;
using System.Data;

namespace ReACT.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class AddCompanyModel : PageModel
    {
        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }
        private readonly RoleManager<IdentityRole> roleManager;

        [BindProperty]
        public AddCompany ADModel { get; set; }

        public AddCompanyModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAdd_CompanyAsync()
        {
            Console.WriteLine(ModelState.Values.SelectMany(v => v.Errors));
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = ADModel.Email,
                    Email = ADModel.Email,
                    FirstName = ADModel.FirstName,
                    Address = ADModel.Address,
                    contactNo = ADModel.ContactNo,
                    PublicPrivate = true
                };
                //Create the Company role if NOT exist
                IdentityRole roleCompany = await roleManager.FindByIdAsync("Company");
                if (roleCompany == null)
                {
                    IdentityResult result2 = await roleManager.CreateAsync(new IdentityRole("Company"));
                    if (!result2.Succeeded)
                    {
                        ModelState.AddModelError("", "Create role Company failed");
                    }
                }

                var result = await userManager.CreateAsync(user, ADModel.Password);
                if (result.Succeeded)
                {
                    result = await userManager.AddToRoleAsync(user, "Company");
                    //await signInManager.SignInAsync(user, false);
                    return RedirectToPage("/ManageCompanies");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }

        public IActionResult OnPostNo_Add_Company()
        {
            return RedirectToPage("/ManageCompanies");
        }
    }
}
