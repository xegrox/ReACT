using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ReACT.Areas.User.Pages
{
    [Authorize]
    public class ForumModel : PageModel
    {
        private readonly ForumService _forumService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _authDbContext;

        public ForumModel(ForumService forumService, UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
        {
            _forumService = forumService;
            _userManager = userManager;
            _authDbContext = authDbContext;
        }

        [BindProperty, Required, StringLength(50, MinimumLength = 5, ErrorMessage = "The thread title must be between 5 and 50 characters.")]
        public string Title { get; set; }
        [BindProperty, Required, MaxLength(300, ErrorMessage = "The thread description must be less than 300 characters.")]
        public string Content { get; set; }

        public ApplicationUser? currentUserId { get; }

        public List<Models.Thread>? threadList { get; set; } = new();

        public string GetPostTime(DateTime dateTime)
        {
            return _forumService.CalcTime(dateTime);
        }

        public IActionResult OnGet()
        {
            threadList = _forumService.GetAll();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var currentUserId = _userManager.GetUserId(User);
                var currentUserName = _authDbContext.Users.Where(u => u.Id == currentUserId).Select(u => u.FirstName).SingleOrDefault() + " " + _authDbContext.Users.Where(u => u.Id == currentUserId).Select(u => u.LastName).SingleOrDefault();

                Models.Thread thread = new Models.Thread { Title = Title, Content = Content, ImageURL = "", UserId = currentUserId, UserName = currentUserName };
                _forumService.AddThread(thread);

                return Redirect("/User/Forum");
            }

            return Page();
        }
    }
}