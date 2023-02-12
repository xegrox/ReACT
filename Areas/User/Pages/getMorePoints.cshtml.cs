using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace ReACT.Areas.User.Pages
{
    [Authorize(Roles = "Admin, User")]
    public class getMorePointsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
