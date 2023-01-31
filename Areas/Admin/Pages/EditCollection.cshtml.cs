using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

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
        public string Date { get; set; }
        public DateTime ParsedDate { get; set; }

        [BindProperty, Required]
        public string Company { get; set; }
        public Collection collection { get; set; }
        public List<Models.Company> companies { get; set; } = new();
        public IActionResult OnGet(int collectionId)
        {
            collection = _collectionService.GetCollection(collectionId);
            companies = _companyService.GetCompanies();
            return Page();
        }

        public IActionResult OnPost(int collectionId)
        {
            if (ModelState.IsValid)
            {
                ParsedDate = DateTime.ParseExact(Date, "yyyy-MM-dd", CultureInfo.CurrentCulture);

                collection = _collectionService.GetCollection(collectionId);
                collection.CollectionDate = ParsedDate.Date;
                collection.AssignedCompany = Company;

                _collectionService.UpdateCollection(collection);
                return Redirect("/Admin/ViewCollections");
            }
            return Page();
        }
    }
}
