using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Services;

namespace ReACT.Areas.User.Pages
{
    [Authorize]
    public class threadModel : PageModel
    {
        private readonly ForumService _forumService;

        public threadModel(ForumService forumService)
        {
            _forumService = forumService;
        }

        public Models.Thread MyThread { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            Models.Thread? thread = _forumService.GetThread(id);

            if (thread != null)
            {
                MyThread = thread;

                return Page();
            }
            else
            {
                return Redirect("/Forum");
            }
        }
    }
}