using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ReACT.Areas.Admin.Pages;

public class ViewCollectionModel : PageModel
{
    private UserManager<ApplicationUser> _userManager { get; }
    private AuthDbContext _authDbContext;
    private readonly CollectionService _collectionService;
    private readonly CompanyService _companyService;

    public ViewCollectionModel(CollectionService collectionService, CompanyService companyService, UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
    {
        _collectionService = collectionService;
        _companyService = companyService;
        _userManager = userManager;
        _authDbContext = authDbContext;
    }

    public List<Collection> CollectionList { get; set; } = new();
    public IActionResult OnGet(bool completed, int collectionId, string userId)
    {
        if (completed)
        {
            CollectionList = _collectionService.GetCompletedCollections();
            companies = _companyService.GetCompanies();
            ViewData["completed"] = 2;
        } else
        {
            CollectionList = _collectionService.GetPendingCollections();
            companies = _companyService.GetCompanies();
            ViewData["completed"] = 1;
        }

        return Page();
    }

    public int CompanyId { get; set; }
    public int CollectionId { get; set; }
    public Collection collection { get; set; }
    public List<Models.Company> companies { get; set; } = new();

    // Assign company OnPost
    public IActionResult OnPostAssignCompany(int CollectionId, int CompanyId)
    {
        if (ModelState.IsValid)
        {
            var collection = _collectionService.GetCollection(CollectionId);
            var company = _companyService.GetCompany(CompanyId);
            //collection.Company.Id = CompanyId;
            collection.AssignedCompany = company.Name;
            _collectionService.UpdateCollection(collection);

            return RedirectToPage("/ViewCollections");
        }
        else { return RedirectToPage("/ViewCollections"); }
    }


    public string EditDate { get; set; }
    public DateTime ParsedDate { get; set; }

    // Edit Collection OnPost
    public IActionResult OnPostEditCollection(string EditDate, int CollectionId, int CompanyId)
    {
        if (ModelState.IsValid)
        {
            var collection = _collectionService.GetCollection(CollectionId);

            ParsedDate = DateTime.ParseExact(EditDate, "yyyy-MM-dd", CultureInfo.CurrentCulture);
            collection.CollectionDate = ParsedDate.Date;
            //collection.Company.Id = CompanyId;
            _collectionService.UpdateCollection(collection);


            return RedirectToPage("/ViewCollections");
        }
        else { return RedirectToPage("/ViewCollections"); }
    }

    public ApplicationUser user { get; set; }
}
