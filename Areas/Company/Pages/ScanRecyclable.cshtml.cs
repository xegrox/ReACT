using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;
using System.Data;
using System.Runtime.Intrinsics.X86;

namespace ReACT.Areas.Company.Pages;

[Authorize(Roles = "Company")]
public class AllocatePoints : PageModel
{
    private readonly CollectionService _collectionService;
    private readonly AuthDbContext _authDbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RecyclingTypeService _recyclingTypeService;

    public AllocatePoints(CollectionService collectionService, AuthDbContext authDbContext, UserManager<ApplicationUser> userManager, RecyclingTypeService recyclingTypeService)
    {
        _collectionService = collectionService;
        _authDbContext = authDbContext;
        _userManager = userManager;
        _recyclingTypeService = recyclingTypeService;
    }

    [BindProperty]
    public Collection oneCollection { get; set; } = new();

    public List<Collection> RecyclableCollectionList { get; set; } = new();

    public List<ApplicationUser> UsersList { get; set; } = new();

    public List<RecyclingType> RecyclingTypeList { get; set; } = new();

    public void OnGet()
    {
        RecyclableCollectionList = _collectionService.GetCollections();
        //UsersList = _authDbContext.Users.OrderBy(x => x.FirstName).ToList();
    }

    public IActionResult OnPostAdd_Collection()
    {
        ApplicationUser? foundUser = null;
        //UsersList = _authDbContext.Users.OrderBy(x => x.FirstName).ToList();
        //foreach (var oneUser in UsersList)
        //{
        //    if (Convert.ToInt32(oneCollection.UserId.Barcode_Number) == oneUser.Barcode_Number)
        //    {
        //        oneCollection.UserId = Convert.ToInt32(oneUser.Id);
        //        foundUser = oneUser;
        //        break;
        //    }
        //}

        //if (foundUser == null)
        //{
        //    ModelState.AddModelError("", "Barcode number does not exist");
        //    return Page();
        //}

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

        oneCollection.CollectionDate = DateTime.Now.ToLongDateString();
        oneCollection.PointsAllocated = Convert.ToInt32(oneCollection.TotalWeight * Convert.ToDecimal(pointsPerKG));
        foundUser.Total_Points += oneCollection.PointsAllocated;
        oneCollection.AssignedCompany = "Company1";
        _collectionService.AddCollection(oneCollection);
        return RedirectToPage("/ScanRecyclable");
    }

    public IActionResult OnPostNo_Add_Collection()
    {
        return RedirectToPage("/ScanRecyclable");
    }
}