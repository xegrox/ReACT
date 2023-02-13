using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReACT.Models;
using ReACT.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
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
    private readonly CompanyService _companyService;
    private readonly ForestService _forestService;

    public AllocatePoints(CollectionService collectionService, AuthDbContext authDbContext, UserManager<ApplicationUser> userManager, RecyclingTypeService recyclingTypeService, CompanyService companyService, ForestService forestService)
    {
        _collectionService = collectionService;
        _authDbContext = authDbContext;
        _userManager = userManager;
        _recyclingTypeService = recyclingTypeService;
        _companyService = companyService;
        _forestService = forestService;
    }

    //[BindProperty]
    //public Collection oneCollection { get; set; } = new();

    [BindProperty]
    public string userID { get; set; }

    [BindProperty]
    [Range(1, 10000)]
    [Column(TypeName = "decimal(7,2)")]
    public decimal totalWeight { get; set; }

    public List<Collection> RecyclableCollectionList { get; set; } = new();

    public List<ApplicationUser> UsersList { get; set; } = new();

    public List<RecyclingType> RecyclingTypeList { get; set; } = new();

    [BindProperty]
    public string recyclingMethod { get; set; }

    public async void OnGet()
    {
        RecyclingTypeList = _recyclingTypeService.GetAllTypes();
        string userId = _userManager.GetUserId(User);
        ApplicationUser? currentUser = _authDbContext.Users.FirstOrDefault(x => x.Id.Equals(userId));
        RecyclableCollectionList = _collectionService.GetCollectionsByCompany(currentUser.FirstName);
    }

    public async Task<ActionResult> OnPostAdd_Collection()
    {
        string myMessage = "";
        ApplicationUser? selectedUser = _authDbContext.Users.FirstOrDefault(x => x.Id.Equals(userID));
        if (selectedUser != null)
        {
            var pointsPerKG = 0;

            // get company
            var companyList = await _userManager.GetUserAsync(User);
            if (companyList != null)
            {
                // check if user has scheduled a recyclable collection
                var check_collection = _authDbContext.Collections.FirstOrDefault(x => x.UserId.Equals(selectedUser.Id) && x.Status.Equals("Not Completed"));
                if (check_collection == null) {
                    ModelState.AddModelError("", "This user has not scheduled a collection.");
                    return Page();
                }

                // get PointsPerKG conversion rate
                RecyclingTypeList = _recyclingTypeService.GetAllTypes();
                foreach (var recyclingType in RecyclingTypeList)
                {
                    if (recyclingMethod == $"Collection ({recyclingType.PointsPerKg} points/kg)")
                    {
                        pointsPerKG = recyclingType.PointsPerKg;
                        break;
                    }
                    else if (recyclingMethod == $"Self recycled ({recyclingType.PointsPerKg} points/kg)")
                    {
                        pointsPerKG = recyclingType.PointsPerKg;
                        break;
                    }
                }

                // update collection status
                check_collection.UserId = check_collection.UserId;
                check_collection.Username = check_collection.Username;
                check_collection.Address = check_collection.Address;
                check_collection.ScheduledDate = check_collection.ScheduledDate;
                check_collection.CollectionDate = DateTime.Now;
                check_collection.AssignedCompany = companyList.FirstName;
                check_collection.Status = "Completed";
                check_collection.TotalWeight = totalWeight;
                check_collection.PointsAllocated = Convert.ToInt32(totalWeight * Convert.ToDecimal(pointsPerKG));
                _collectionService.UpdateCollection(check_collection);

                // add user points
                selectedUser.Total_Points += check_collection.PointsAllocated;

                foreach (var recyclingType in RecyclingTypeList)
                {
                    if (recyclingMethod == $"Collection ({recyclingType.PointsPerKg} points/kg)")
                    {
                        myMessage = $"{check_collection.AssignedCompany} collected {check_collection.TotalWeight}kg of recyclables.";
                        break;
                    }
                    else if (recyclingMethod == $"Self recycled ({recyclingType.PointsPerKg} points/kg)")
                    {
                        myMessage = $"You recycled {check_collection.TotalWeight}kg of recyclables.";
                        break;
                    }
                }

                // add points history
                PointsHistory addRow = new PointsHistory
                {
                    userId = check_collection.UserId,
                    points_spent = 0,
                    activity_description = myMessage,
                    points_gained = check_collection.PointsAllocated
                };
                _authDbContext.PointsHistory.Add(addRow);
                await _authDbContext.SaveChangesAsync();

                // add tree
                _forestService.InsertRandomTree(Convert.ToDouble(check_collection.TotalWeight));
                if (selectedUser.chance_TreeTask == 0)
                {
                    selectedUser.chance_TreeTask = 1;
                    selectedUser.chances_Free += 1;
                }

                _authDbContext.Users.Update(selectedUser);
                await _authDbContext.SaveChangesAsync();

                // return
                return Page();
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
            ModelState.AddModelError("", "Cannot find user with this QR Code");
            return Page();
        }
    }

    public IActionResult OnPostNo_Add_Collection()
    {
        return RedirectToPage("/ScanRecyclable");
    }
}