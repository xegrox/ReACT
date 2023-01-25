using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ReACT.Areas.User.Pages
{
    [Authorize]
    public class scoreboardRankModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
