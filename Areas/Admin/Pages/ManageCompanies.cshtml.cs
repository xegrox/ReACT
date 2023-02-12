using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using System.Data;

namespace ReACT.Areas.Admin.Pages;
[Authorize(Roles = "Admin")]

public class ManageCompanies : PageModel
{
    private UserManager<ApplicationUser> userManager { get; }
    private SignInManager<ApplicationUser> signInManager { get; }
    private readonly RoleManager<IdentityRole> roleManager;

    [BindProperty]
    public List<ApplicationUser> Users { get; set; }

    public ManageCompanies(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager){
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
    }
    public async Task<PageResult> OnGetAsync(string roleName)
    {
        Users = (await userManager.GetUsersInRoleAsync("Company")).ToList();
        return Page();
    }
}