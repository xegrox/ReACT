using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;
using System.Runtime.Intrinsics.X86;

namespace ReACT.Areas.Company.Pages;

public class AllocatePoints : PageModel
{
    private readonly CollectionService _collectionService;
    private readonly AuthDbContext _authDbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RecyclingTypeService _recyclingTypeService;
    private readonly CompanyService _companyService;

    public AllocatePoints(CollectionService collectionService, AuthDbContext authDbContext, UserManager<ApplicationUser> userManager, RecyclingTypeService recyclingTypeService, CompanyService companyService)
    {
        _collectionService = collectionService;
        _authDbContext = authDbContext;
        _userManager = userManager;
        _recyclingTypeService = recyclingTypeService;
        _companyService = companyService;
    }

    [BindProperty]
    public Collection oneCollection { get; set; } = new();

    public List<Collection> RecyclableCollectionList { get; set; } = new();

    public List<ApplicationUser> UsersList { get; set; } = new();

    public List<RecyclingType> RecyclingTypeList { get; set; } = new();

    public void OnGet()
    {
        RecyclableCollectionList = _collectionService.GetCollectionsByCompany("Company Test");
        foreach (var collection in RecyclableCollectionList)
        {
            var oneUser = _authDbContext.Users.FirstOrDefault(x => x.Id == collection.UserId);
            if (oneUser != null)
            {
                UsersList.Add(oneUser);
            }
        }
    }

    public async Task<ActionResult> OnPostAdd_Collection()
    {
        ApplicationUser? selectedUser = _authDbContext.Users.FirstOrDefault(x => x.Id.Equals(oneCollection.UserId));
        if (selectedUser != null)
        {
            var pointsPerKG = 0;

            // get PointsPerKG conversion rate
            RecyclingTypeList = _recyclingTypeService.GetAllTypes();
            foreach (var recyclingType in RecyclingTypeList)
            {
                if (recyclingType.Name == "Collection")
                {
                    pointsPerKG = recyclingType.PointsPerKg;
                    break;
                }
            }

            // get company
            var companyList = _companyService.GetCompany(1);
            if (companyList != null)
            {
                // check if user has scheduled a recyclable collection
                var check_collection = _authDbContext.Collections.FirstOrDefault(x => x.UserId.Equals(selectedUser.Id) && x.Status.Equals("Not Completed"));
                if (check_collection == null) {
                    ModelState.AddModelError("", "This user has not scheduled a collection.");
                    return Page();
                }

                // update collection status
                oneCollection.UserId = selectedUser.Id;
                oneCollection.CollectionDate = DateTime.Now;
                oneCollection.AssignedCompany = companyList.Name;
                oneCollection.Status = "Completed";
                oneCollection.PointsAllocated = Convert.ToInt32(oneCollection.TotalWeight * Convert.ToDecimal(pointsPerKG));
                oneCollection.Company = companyList;
                selectedUser.Total_Points += oneCollection.PointsAllocated;
                _collectionService.UpdateCollection(oneCollection);

                // add points history
                PointsHistory addRow = new PointsHistory
                {
                    userId = selectedUser.Id,
                    points_spent = 0,
                    activity_description = oneCollection.TotalWeight + "kg of recyclables collected by " + companyList.Name,
                    points_gained = oneCollection.PointsAllocated
                };
                _authDbContext.PointsHistory.Add(addRow);
                await _authDbContext.SaveChangesAsync();

                // return
                return RedirectToPage("/ScanRecyclable");
            }
            else
            {
                // cannot find company
                ModelState.AddModelError("", "Company does not exist.");
                return Page();
            }

        }
        else
        {
            // cannot find user with userId
            ModelState.AddModelError("", "Barcode string does not exist.");
            return Page();
        }
    }

    public IActionResult OnPostNo_Add_Collection()
    {
        return RedirectToPage("/ScanRecyclable");
    }
}