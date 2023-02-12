using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using System.Data;

namespace ReACT.Areas.Admin.Pages;

[Authorize(Roles = "Admin")]
public class ManageUsers : PageModel
{
    private UserManager<ApplicationUser> userManager { get; }

    [BindProperty]
    public List<ApplicationUser> Users { get; set; }
    public String accType { get; set; }

    public ManageUsers(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<PageResult> OnGetAsync(string roleName)
    {
        Users = (await userManager.GetUsersInRoleAsync("User")).ToList();
        foreach (var user in Users)
        {
            if (user.PublicPrivate == true)
            {
                accType = "Private";
            }
            else
            {
                accType = "Public";
            }
        }
        return Page();
    }

}