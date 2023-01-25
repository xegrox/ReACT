using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;
using System.ComponentModel.DataAnnotations;

namespace ReACT.Areas.Admin.Pages
{
    public class EditCollectionModel : PageModel
    {
        private readonly CollectionService _collectionService;
        private readonly CompanyService _companyService;

        public EditCollectionModel(CollectionService collectionService, CompanyService companyService)
        {
            _collectionService = collectionService;
            _companyService = companyService;
        }

        [BindProperty, Required]
        public DateTime Date { get; set; }
        [BindProperty, Required]
        public int CompanyId { get; set; }
        public Collection collection { get; set; }
        public List<Companies> companies { get; set; } = new();
        public IActionResult OnGet(int collectionId)
        {
            collection = _collectionService.GetCollection(collectionId);
            companies = _companyService.GetCompanies();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                collection.CollectionDate = Date;
                collection.CompanyId = CompanyId;

                _collectionService.UpdateCollection(collection);
                return Redirect("/Admin/ViewCollections");
            }
            return Page();
        }
    }
}
