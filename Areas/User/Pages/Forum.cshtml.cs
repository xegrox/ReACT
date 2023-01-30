using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;
using System.ComponentModel.DataAnnotations;

namespace ReACT.Areas.User.Pages
{
    public class ForumModel : PageModel
    {
        private readonly ForumService _forumService;

        public ForumModel(ForumService forumService)
        {
            _forumService = forumService;
        }

        [BindProperty]
        public Models.Thread MyThread { get; set; } = new();

        private readonly MockThreadsDb? _mockThreadsDb;

        public ForumModel(MockThreadsDb mockThreadsDb)
        {
            _mockThreadsDb = mockThreadsDb;
        }

        [BindProperty, Required, StringLength(50, MinimumLength = 5, ErrorMessage = "The thread title must be between 5 and 50 characters.")]
        public string Title { get; set; }
        [BindProperty, Required, MaxLength(300, ErrorMessage = "The thread description must be less than 300 characters.")]
        public string Content { get; set; }

        public List<Models.Thread>? threadList { get; set; } = new();

        public void OnGet()
        {
            threadList = _forumService.GetAll();
        }

        public IActionResult OnPost()
        {
            //note: remember to add a check for duplicate title
            //if (ModelState.IsValid)
            if (true)
            {
                Models.Thread thread = new Models.Thread { Id = 4, Title = MyThread.Title, Content = MyThread.Content, ImageURL = "" };
                _forumService.AddThread(thread);

                return Redirect("/User/Forum");
            }

            return Page();
        }
    }
}