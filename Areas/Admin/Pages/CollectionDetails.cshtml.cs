using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;

namespace ReACT.Areas.Admin.Pages
{
    public class CollectionDetailsModel : PageModel
    {
        private readonly MockCollectionsDb _mockCollectionsDb;
        private readonly MockUsersDb _mockUsersDb;

        public CollectionDetailsModel(MockCollectionsDb mockCollectionsDb, MockUsersDb mockUsersDb)
        {
            _mockCollectionsDb = mockCollectionsDb;
            _mockUsersDb = mockUsersDb;
        }

        public UserInfo user { get; set; }
        public Collection collection { get; set; }

        public IActionResult OnGet(int userId, int collectionId)
        {
            UserInfo? userInfo = _mockUsersDb.GetUser(userId);
            if (userInfo != null)
            {
                user = userInfo;
                collection = _mockCollectionsDb.GetCollection(collectionId);
                return Page();
            } else
            {
                return Redirect("/ViewCollections");
            }
        }
    }
}
