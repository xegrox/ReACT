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

    public ApplicationUser UsersList { get; set; } = new();

    public List<RecyclingType> RecyclingTypeList { get; set; } = new();

    public void OnGet()
    {
        RecyclableCollectionList = _collectionService.GetCollections();
        foreach (var collection in RecyclableCollectionList)
        {
            var oneUser = _authDbContext.Users.FirstOrDefault(x => x.Id == collection.UserId);
            if (oneUser != null)
            {
                UsersList = oneUser;
            }
        }
    }

    public IActionResult OnPostAdd_Collection()
    {
        ApplicationUser? selectedUser = _authDbContext.Users.FirstOrDefault(x => x.Id.Equals(oneCollection.UserId));
        if (selectedUser != null)
        {
            var pointsPerKG = 0;
            RecyclingTypeList = _recyclingTypeService.GetAllTypes();
            foreach (var recyclingType in RecyclingTypeList)
            {
                if (recyclingType.Name == "Collection")
                {
                    pointsPerKG = recyclingType.PointsPerKg;
                    break;
                }
            }

            var companyList = _companyService.GetCompany(1);
            if (companyList != null)
            {
                oneCollection.CollectionDate = DateTime.Now;
                oneCollection.PointsAllocated = Convert.ToInt32(oneCollection.TotalWeight * Convert.ToDecimal(pointsPerKG));
                selectedUser.Total_Points += oneCollection.PointsAllocated;
                oneCollection.AssignedCompany = companyList.Name;
                oneCollection.Status = "Collected";
                //oneCollection.CompanyID = companyList.Id;
                _collectionService.AddCollection(oneCollection);
                return RedirectToPage("/ScanRecyclable");
            }
            else
            {
                ModelState.AddModelError("", "Company does not exist.");
                return Page();
            }

        }
        else
        {
            ModelState.AddModelError("", "Barcode string does not exist.");
            return Page();
        }
    }

    public IActionResult OnPostNo_Add_Collection()
    {
        return RedirectToPage("/ScanRecyclable");
    }
}