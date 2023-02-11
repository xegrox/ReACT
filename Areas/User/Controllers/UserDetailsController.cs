using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReACT.Models;

namespace ReACT.Areas.User.Controllers;

[Authorize]
[Area("User")]
public class UserDetailsController : Controller
{
    public async Task<IActionResult> Points([FromServices] UserManager<ApplicationUser> userManager)
    {
        var user = await userManager.GetUserAsync(User);
        return new JsonResult(user.Total_Points);
    }
}