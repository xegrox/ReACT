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

        [BindProperty, Required, StringLength(50, MinimumLength = 5, ErrorMessage = "The thread title must be between 5 and 50 characters.")]
        public string Title { get; set; }
        [BindProperty, Required, MaxLength(300, ErrorMessage = "The thread description must be less than 300 characters.")]
        public string Content { get; set; }

        public List<Models.Thread>? threadList { get; set; } = new();

        public IActionResult OnGet()
        {
            threadList = _forumService.GetAll();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                Models.Thread thread = new Models.Thread { Id = 4, Title = Title, Content = Content, ImageURL = "" };
                _forumService.AddThread(thread);

                return Redirect("/User/Forum");
            }

            return Page();
        }
    }
}