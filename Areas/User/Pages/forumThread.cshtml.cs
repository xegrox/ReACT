using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;

namespace ReACT.Areas.User.Pages
{
    public class threadModel : PageModel
    {
        private readonly MockThreadsDb? _mockThreadsDb;

        public threadModel(MockThreadsDb mockThreadsDb)
        {
            _mockThreadsDb = mockThreadsDb;
        }

        public Models.Thread MyThread { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            Models.Thread? thread = _mockThreadsDb?.GetThread(id);

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