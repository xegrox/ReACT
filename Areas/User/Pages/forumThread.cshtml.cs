using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;

namespace ReACT.Areas.User.Pages
{
    [Authorize]
    public class ThreadModel : PageModel
    {
        private readonly ForumService _forumService;
        private readonly MessageService _messageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _authDbContext;

        public ThreadModel(ForumService forumService, MessageService messageService, UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
        {
            _forumService = forumService;
            _messageService = messageService;
            _userManager = userManager;
            _authDbContext = authDbContext;
        }

        [BindProperty, Required, MaxLength(300, ErrorMessage = "Your message cannot exceed 250 characters.")]
        public string Content { get; set; }

        public Models.Thread MyThread { get; set; } = new();
        public List<Message> messageList { get; set; } = new();

        public ApplicationUser? currentUser { get; set; }

        public string GetPostTime(DateTime dateTime)
        {
            return _forumService.CalcTime(dateTime);
        }

        public IActionResult OnGet(int id)
        {
            Models.Thread? thread = _forumService.GetThread(id);
            //Filter messages according to threadId
            List<Message> messages = _authDbContext.Messages.Where(m => m.threadId == id).ToList();

            if (thread != null)
            {
                MyThread = thread;
                messageList = messages;

                return Page();
            }
            else
            {
                return Redirect("/Forum");
            }
        }

        //OnPost for add new message
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var threadId = Convert.ToInt32(HttpContext.Request.Query["id"]);

                var currentUserId = _userManager.GetUserId(User);
                var currentUserName = _authDbContext.Users.Where(u => u.Id == currentUserId).Select(u => u.FirstName).SingleOrDefault() + " " + _authDbContext.Users.Where(u => u.Id == currentUserId).Select(u => u.LastName).SingleOrDefault();

                Message message = new Message { Content = Content, UserId = currentUserId, UserName = currentUserName, threadId = threadId };
                _messageService.AddMessage(message);

                return Redirect(Request.GetEncodedUrl());
            }

            return Page();
        }

    }
}