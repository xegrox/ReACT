using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.ViewModels;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace ReACT.Areas.User.Pages
{
    [Authorize(Roles = "Admin, User")]
    public class UserDetailsModel : PageModel
    {
        private UserManager<ApplicationUser> UserManager { get; set; }
        private AuthDbContext authDbContext;

        public String accType { get; set; }

        [BindProperty]
        public ChangePassword CModel { get; set; }

        [BindProperty]
        public EditUser EModel { get; set; }

        [BindProperty]
        public ApplicationUser user { get; set; }
        public UserDetailsModel(UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
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
            if (user.PublicPrivate == true)
            {
                accType = "Private";
            }
            else
            {
                accType = "Public";
            }
            return Page();
            //ApplicationUser user = UserManager.FindByIdAsync(userId).Result;
        }



        public async Task <IActionResult> OnPostChange_PasswordAsync()
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
                        //return RedirectToPage("/UserDetails", new { area = "User" });
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

        public async Task<IActionResult> OnPostDelete_UserAsync()
        {
            var userId = UserManager.GetUserId(User);
            var user = await UserManager.FindByIdAsync(userId);

            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToPage("/Login", new { area = "Home" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return Page();
        }

        public IActionResult OnPostNo_Delete_User()
        {
            return RedirectToPage("/UserDetails");
        }

        public async Task<IActionResult> OnPostEdit_UserAsync()
        {
            var userId = UserManager.GetUserId(User);
            var user = await UserManager.FindByIdAsync(userId);
            if (!ModelState.IsValid)
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
