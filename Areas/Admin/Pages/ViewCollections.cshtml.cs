using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;
using System.ComponentModel.DataAnnotations;

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

        ApplicationUser? collectionUser = _authDbContext.Users.FirstOrDefault(x => x.Id.Equals(userId));
        user = collectionUser;

        collection = _collectionService.GetCollection(collectionId);

        return Page();
    }

    [BindProperty, Required]
    public int CompanyId { get; set; }

    [BindProperty, Required]
    public int CollectionId { get; set; }
    public Collection collection { get; set; }
    public List<Models.Company> companies { get; set; } = new();

    // Assign company OnPost
    public IActionResult OnPostAssignCompany()
    {
        if (ModelState.IsValid)
        {
            try
            {
                var collection = _collectionService.GetCollection(CollectionId);
                var company = _companyService.GetCompany(CompanyId);
                collection.Company.Id = CompanyId;
                collection.AssignedCompany = company.Name;
                _collectionService.UpdateCollection(collection);

                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Assigned company.");

                return RedirectToPage("/ViewCollections");
            }
            catch
            {
                ModelState.AddModelError("AssignCompanyError", "Company could not be assigned");
                return Page();
            }
        }
        else { return Page(); }
    }


    [BindProperty, Required]
    public string EditDate { get; set; }

    // Edit Collection OnPost
    public IActionResult OnPostEditCollection()
    {
        if (ModelState.IsValid)
        {
            try
            {
                collection.CollectionDate = DateTime.ParseExact(EditDate, "dd-MM-yyy", null);
                collection.Company.Id = CompanyId;
                _collectionService.UpdateCollection(collection);

                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Edited collection.");

                return RedirectToPage("/ViewCollections");
            }
            catch
            {
                ModelState.AddModelError("EditCollectionError", "Collection could not be edited");
                return Page();
            }
        }
        else { return Page(); }
    }

    public ApplicationUser user { get; set; }
}
