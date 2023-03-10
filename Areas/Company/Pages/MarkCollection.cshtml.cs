using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;
using System.Data;

namespace ReACT.Areas.Company.Pages
{
    [Authorize(Roles = "Company")]
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
            collection = _collectionService.GetCollection(CollectionId);
            collection.Status = "Completed";
            _collectionService.UpdateCollection(collection);
            return Redirect("/Company/ViewCollections");
        }
    }
}
