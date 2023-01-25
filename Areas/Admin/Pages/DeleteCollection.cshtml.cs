using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;

namespace ReACT.Areas.Admin.Pages
{
    public class DeleteCollectionModel : PageModel
    {
        private readonly CollectionService _collectionService;
        public DeleteCollectionModel(CollectionService collectionService)
        {
            _collectionService = collectionService;
        }
        public Collection collection { get; set; }
        public IActionResult OnGet(int CollectionId)
        {
            _collectionService.DeleteCollection(CollectionId);
            return Redirect("/Admin/ViewCollections");
        }
    }
}
