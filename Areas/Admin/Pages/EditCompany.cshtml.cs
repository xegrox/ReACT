using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.ViewModels;
using System.Data;

namespace ReACT.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class EditCompanyModel : PageModel
    {
        [BindProperty]
        public EditCompany EDModel { get; set; }
        private UserManager<ApplicationUser> UserManager { get; set; }
        private AuthDbContext authDbContext;

        [BindProperty]
        public String Id { get; set; }

        public ApplicationUser MyUser { get; set; } = new();

        public EditCompanyModel(UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
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

        public async Task<IActionResult> OnPostEdit_CompanyAsync(string id)
        {

            Console.WriteLine(ModelState.Values.SelectMany(v => v.Errors));
            if (ModelState.IsValid)
            {
                ApplicationUser? user = authDbContext.Users.FirstOrDefault(x => x.Id.Equals(id));
                MyUser = user;

                MyUser.FirstName = EDModel.FirstName;
                MyUser.Address = EDModel.Address;
                MyUser.contactNo = EDModel.ContactNo;

                var result = await UserManager.UpdateAsync(MyUser);

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

        public IActionResult OnPostNo_Edit_Company()
        {
            return RedirectToPage("/ManageCompanies");
        }
    }
}
