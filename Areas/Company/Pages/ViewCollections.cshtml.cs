using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ReACT.Areas.Company.Pages
{
    public class ViewCollectionsModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager { get; }
        private AuthDbContext _authDbContext;
        private readonly CollectionService _collectionService;
        private readonly CompanyService _companyService;

        public ViewCollectionsModel(CollectionService collectionService, CompanyService companyService, UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
        {
            _collectionService = collectionService;
            _companyService = companyService;
            _userManager = userManager;
            _authDbContext = authDbContext;
        }
        public Models.Company Company { get; set; }
        public List<Collection> CollectionList { get; set; } = new();
        public IActionResult OnGet(bool completed)
        {
            if (completed)
            {
                CollectionList = _collectionService.GetCollectionsByCompany("Company Test");
                Company = _companyService.GetCompany(1);
                ViewData["completed"] = 2;
            }
            else
            {
                CollectionList = _collectionService.GetIncompleteCollectionsByCompany("Company Test");
                Company = _companyService.GetCompany(1);
                ViewData["completed"] = 1;
            }

            return Page();
        }
    }
}
