using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;

namespace ReACT.Areas.User.Pages
{
    public class ForumModel : PageModel
    {
        [BindProperty]
        public Models.Thread MyThread { get; set; } = new();

        private readonly MockThreadsDb? _mockThreadsDb;

        public ForumModel(MockThreadsDb mockThreadsDb)
        {
            _mockThreadsDb = mockThreadsDb;
        }

        public List<Models.Thread>? threadList { get; set; } = new();

        public void OnGet()
        {
            threadList = _mockThreadsDb?.GetAll();
        }
    }
}