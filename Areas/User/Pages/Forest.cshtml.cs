using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ReACT.Areas.User.Pages;

[Authorize]
public class Forest : PageModel
{
    public void OnGet()
    {
        
    }
}