using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;

namespace ReACT.Areas.Admin.Pages
{
    public class CollectionDetailsModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager { get; }
        private AuthDbContext _authDbContext;
        private readonly CollectionService _collectionService;
        public CollectionDetailsModel(UserManager<ApplicationUser> userManager, AuthDbContext authDbContext, CollectionService collectionService)
        {
            _userManager = userManager;
            _authDbContext = authDbContext;
            _collectionService = collectionService;
        }

        public ApplicationUser user { get; set; }
        public Collection collection { get; set; }

        public IActionResult OnGet(int collectionId)
        {
            string userId = _userManager.GetUserId(User);
            ApplicationUser? currentUser = _authDbContext.Users.FirstOrDefault(x => x.Id.Equals(userId));
            user = currentUser;

            if (currentUser != null)
            {
                collection = _collectionService.GetCollection(collectionId);
                return Page();
            } else
            {
                return Redirect("/ViewCollections");
            }
        }
    }
}
