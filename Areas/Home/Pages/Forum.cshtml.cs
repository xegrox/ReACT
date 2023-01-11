using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ReACT.Areas.Home.Pages;

public class Forum : PageModel
{
    public IActionResult OnGet()
    {
        return Redirect("../User/Forum");
    }
}