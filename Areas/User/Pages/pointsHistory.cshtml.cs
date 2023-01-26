using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;

namespace ReACT.Areas.User.Pages
{
    public class pointsHistoryModel : PageModel
    {
        private readonly CollectionService _collectionService;

        public pointsHistoryModel(CollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        public List<Collection> RecyclableCollectionList { get; set; } = new();

        public IDictionary<int, Collection> userCollections = new Dictionary<int, Collection>();

        public void OnGet()
        {
            RecyclableCollectionList = _collectionService.GetCollections();
            for (int i = 0; i < RecyclableCollectionList.Count; i++)
            {
                if (RecyclableCollectionList[i].UserId == 2)
                {
                    userCollections.Add(i + 1, RecyclableCollectionList[i]);
                }
            }
        }
    }
}
