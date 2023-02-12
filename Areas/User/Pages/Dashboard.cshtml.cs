using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ReACT.Areas.User.Pages;

[Authorize(Roles = "Admin, User")]

public class Dashboard : PageModel
{
    public void OnGet()
    {
        
    }
}