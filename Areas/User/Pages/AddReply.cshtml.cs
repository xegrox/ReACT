using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ReACT.Areas.User.Pages
{
    [Authorize(Roles = "Admin, User")]
    public class ReplyModel : PageModel
    {
        private readonly MessageService _messageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _authDbContext;

        public ReplyModel(MessageService messageService, UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
        {
            _messageService = messageService;
            _userManager = userManager;
            _authDbContext = authDbContext;
        }

        [BindProperty, Required, MaxLength(300, ErrorMessage = "Your reply cannot exceed 250 characters.")]
        public string Content { get; set; }

        public Message? MyMessage { get; set; }

        public IActionResult OnGet(int id)
        {
            MyMessage = _messageService.GetMessage(id);

            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var messageId = Convert.ToInt32(HttpContext.Request.Query["id"]);
                var currentUserId = _userManager.GetUserId(User);
                var currentUserName = _authDbContext.Users.Where(u => u.Id == currentUserId).Select(u => u.FirstName).SingleOrDefault() + " " + _authDbContext.Users.Where(u => u.Id == currentUserId).Select(u => u.LastName).SingleOrDefault();
                var threadId = _messageService.GetMessage(messageId).threadId;

                Message message = new Message { Content = Content, replyTo = messageId, UserId = currentUserId, UserName = currentUserName, threadId = threadId };
                _messageService.AddMessage(message);

                var currentUser = _authDbContext.Users.Where(u => u.Id == currentUserId).FirstOrDefault();
                if (currentUser.postComments_counter < 5)
                {
                    currentUser.postComments_counter += 1;
                }
                if (currentUser.postComments_counter == 5 && currentUser.chance_CommentTask == 0)
                {
                    currentUser.chance_CommentTask = 1;
                    currentUser.chances_Free += 1;
                }
                _authDbContext.SaveChanges();

                var redirectURL = "/forumThread?id=" + messageId.ToString();
                return Redirect(redirectURL);
            }

            return Page();
        }
    }
}
