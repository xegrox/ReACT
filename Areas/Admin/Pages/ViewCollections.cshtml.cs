using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ReACT.Areas.Admin.Pages;

[Authorize(Roles = "Admin")]
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
    public async Task<IActionResult> OnGet(bool completed)
    {
        var usersWithRole = await _userManager.GetUsersInRoleAsync("Company");
        companies = usersWithRole.OfType<ApplicationUser>().ToList();

        if (completed)
        {
            CollectionList = _collectionService.GetCompletedCollections();
            ViewData["completed"] = 2;
        } else
        {
            CollectionList = _collectionService.GetPendingCollections();
            ViewData["completed"] = 1;
        }

        return Page();
    }

    public string CompanyId { get; set; }
    public int CollectionId { get; set; }
    public Collection collection { get; set; }
    public List<ApplicationUser> companies { get; set; }

    // Assign company OnPost
    public async Task<IActionResult> OnPostAssignCompany(int CollectionId, string CompanyId)
    {
        if (ModelState.IsValid)
        {
            var collection = _collectionService.GetCollection(CollectionId);
            var company = await _userManager.FindByIdAsync(CompanyId);
            collection.AssignedCompany = company.FirstName;
            _collectionService.UpdateCollection(collection);

            return RedirectToPage("/ViewCollections");
        }
        else { return RedirectToPage("/ViewCollections"); }
    }


    public string EditDate { get; set; }
    public DateTime ParsedDate { get; set; }

    // Edit Collection OnPost
    public async Task<IActionResult> OnPostEditCollection(string EditDate, int CollectionId, string? CompanyId)
    {
        if (ModelState.IsValid)
        {
            var collection = _collectionService.GetCollection(CollectionId);

            ParsedDate = DateTime.ParseExact(EditDate, "yyyy-MM-dd", CultureInfo.CurrentCulture);
            collection.CollectionDate = ParsedDate.Date;
            if (collection.AssignedCompany != "N/A")
            {
                var company = await _userManager.FindByIdAsync(CompanyId);
                collection.AssignedCompany = company.FirstName;
            }
            _collectionService.UpdateCollection(collection);


            return RedirectToPage("/ViewCollections");
        }
        else { return RedirectToPage("/ViewCollections"); }
    }

    public ApplicationUser user { get; set; }
}
