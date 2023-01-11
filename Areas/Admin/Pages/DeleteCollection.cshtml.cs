using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;

namespace ReACT.Areas.Admin.Pages
{
    public class DeleteCollectionModel : PageModel
    {
        private readonly MockCollectionsDb _mockCollectionsDb;

        public DeleteCollectionModel(MockCollectionsDb mockCollectionsDb)
        {
            _mockCollectionsDb = mockCollectionsDb;
        }
        public Collection collection { get; set; }
        public IActionResult OnGet(int CollectionId)
        {
            collection = _mockCollectionsDb.GetCollection(CollectionId);
            MockCollectionsDb.Collections.Remove(collection);
            return Redirect("/Admin/ViewCollections");
        }
    }
}
