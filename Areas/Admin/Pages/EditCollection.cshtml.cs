using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using System.ComponentModel.DataAnnotations;

namespace ReACT.Areas.Admin.Pages
{
    public class EditCollectionModel : PageModel
    {
        private readonly MockCollectionsDb _mockCollectionsDb;

        public EditCollectionModel(MockCollectionsDb mockCollectionsDb)
        {
            _mockCollectionsDb = mockCollectionsDb;
        }

        [BindProperty, Required]
        public string Date { get; set; }
        [BindProperty, Required]
        public string Company { get; set; }
        public Collection collection { get; set; }
        public IActionResult OnGet(int collectionId)
        {
            collection = _mockCollectionsDb.GetCollection(collectionId);
            return Page();
        }

        public IActionResult OnPost(int collectionId)
        {
            if (ModelState.IsValid)
            {
                var Collection = MockCollectionsDb.Collections.FirstOrDefault(x => x.Id.Equals(collectionId));
                Collection.CollectionDate = Date;
                Collection.AssignedCompany = Company;
                return Redirect("/Admin/ViewCollections");
            }
            return Page();
        }
    }
}
