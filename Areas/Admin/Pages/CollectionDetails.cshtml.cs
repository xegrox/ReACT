using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;

namespace ReACT.Areas.Admin.Pages
{
    public class CollectionDetailsModel : PageModel
    {
        private readonly UserService _userService;
        private readonly CollectionService _collectionService;
        public CollectionDetailsModel(UserService userService, CollectionService collectionService)
        {
            _userService = userService;
            _collectionService = collectionService;
        }

        public Users user { get; set; }
        public Collection collection { get; set; }

        public IActionResult OnGet(int userId, int collectionId)
        {
            Users? userInfo = _userService.GetUser(userId);
            if (userInfo != null)
            {
                user = userInfo;
                collection = _collectionService.GetCollection(collectionId);
                return Page();
            } else
            {
                return Redirect("/ViewCollections");
            }
        }
    }
}
