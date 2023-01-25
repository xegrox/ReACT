using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ReACT.Areas.User.Pages;

[Authorize]
public class Dashboard : PageModel
{
    public void OnGet()
    {
        
    }
}