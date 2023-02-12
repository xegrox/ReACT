using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;
using System.Data;

namespace ReACT.Areas.Admin.Pages;

[Authorize(Roles = "Admin")]
public class AllocationSettings : PageModel
{
    private readonly RecyclingTypeService _recyclingTypeService;

    public AllocationSettings(RecyclingTypeService recyclingTypeService)
    {
        _recyclingTypeService = recyclingTypeService;
    }

    [BindProperty]
    public RecyclingType oneAllocation { get; set; } = new();

    public List<RecyclingType> RecyclingTypeList { get; set; } = new();

    public void OnGet()
    {
        RecyclingTypeList = _recyclingTypeService.GetAllTypes();
    }

    public IActionResult OnPostAdd_Type()
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (oneAllocation.PointsPerKg <= 0)
                {
                    ModelState.AddModelError("PointsPerKg", "Points per KG must be a whole number and at least 1");
                    return Page();
                }

                RecyclingType? checking = _recyclingTypeService.GetRecyclableTypeById(oneAllocation.Id);
                if (checking != null)
                {
                    ModelState.AddModelError("Name", "Recycling Type already exists");
                    return Page();
                }

                Convert.ToInt64(oneAllocation.PointsPerKg);
                var allTypes = _recyclingTypeService.GetAllTypes();
                _recyclingTypeService.AddRecyclingType(oneAllocation);
                return RedirectToPage("/PointsSettings");
            }
            catch
            {
                ModelState.AddModelError("PointsPerKg", "Invalid Points per KG");
                return Page();
            }
        }
        var errors = ModelState.Values.SelectMany(v => v.Errors);
        return Page();
    }

    public IActionResult OnPostNo_Add_Type()
    {
        return RedirectToPage("/PointsSettings");
    }

    public IActionResult OnPostEdit_Type()
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (oneAllocation.PointsPerKg <= 0)
                {
                    ModelState.AddModelError("PointsPerKg", "Points per KG must be at least 1");
                    return Page();
                }

                Convert.ToInt64(oneAllocation.PointsPerKg);
                _recyclingTypeService.UpdateRecyclingType(oneAllocation);
                return RedirectToPage("/PointsSettings");
            }
            catch
            {
                ModelState.AddModelError("PointsPerKg", "Invalid Points per KG");
                return Page();
            }
        }
        return Page();
    }

    public IActionResult OnPostNo_Edit_Type()
    {
        return RedirectToPage("/PointsSettings");
    }

    public IActionResult OnPostRemove_Type()
    {
        _recyclingTypeService.DeleteRecyclingType(oneAllocation);
        return RedirectToPage("/PointsSettings");
    }

    public IActionResult OnPostNo_Remove_Type()
    {
        return RedirectToPage("/PointsSettings");
    }
}