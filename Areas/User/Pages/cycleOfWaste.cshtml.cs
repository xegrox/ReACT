using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ReACT.Areas.User.Pages
{
    [Authorize(Roles = "Admin, User")]
    public class cycleOfWasteModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
