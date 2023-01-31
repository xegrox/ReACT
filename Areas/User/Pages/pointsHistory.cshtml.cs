using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;

namespace ReACT.Areas.User.Pages
{
    public class pointsHistoryModel : PageModel
    {
        //private readonly CollectionService _collectionService;
        //private UserManager<ApplicationUser> _userManager { get; }
        //private readonly CollectionService _collectionService;
        //private AuthDbContext _context;

        //public pointsHistoryModel(CollectionService collectionService, UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
        //{
        //    _collectionService = collectionService;
        //    UserManager = userManager;
        //    _context = authDbContext; 
        //}

        //public List<Collection> RecyclableCollectionList { get; set; } = new();

        //public IDictionary<int, Collection> userCollections = new Dictionary<int, Collection>();

        //public void OnGet()
        //{
        //    string userId = UserManager.GetUserId(User);
        //    RecyclableCollectionList = _collectionService.GetCollections();
        //    for (int i = 0; i < RecyclableCollectionList.Count; i++)
        //    {
        //        if (RecyclableCollectionList[i].UserId == userId)
        //        {
        //            userCollections.Add(i + 1, RecyclableCollectionList[i]);
        //        }
        //    }
        //}
        private readonly CollectionService _collectionService;
        private readonly UserManager<ApplicationUser> _userManager;

        public pointsHistoryModel(CollectionService collectionService, UserManager<ApplicationUser> userManager)
        {
            _collectionService = collectionService;
            _userManager = userManager;
        }

        public List<Collection> RecyclableCollectionList { get; set; } = new();

        public IDictionary<int, Collection> userCollections = new Dictionary<int, Collection>();

        public void OnGet()
        {
            RecyclableCollectionList = _collectionService.GetCollections();
            for (int i = 0; i < RecyclableCollectionList.Count; i++)
            {
                if (RecyclableCollectionList[i].UserId == _userManager.GetUserId(User))
                {
                    userCollections.Add(i + 1, RecyclableCollectionList[i]);
                }
            }
        }
    }
}
