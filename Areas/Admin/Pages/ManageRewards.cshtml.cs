using FuzzySharp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReACT.Helpers;
using ReACT.Models;

namespace ReACT.Areas.Admin.Pages;

public class ManageRewards : PageModel
{
    private AuthDbContext _context;
    
    public ManageRewards(AuthDbContext context)
    {
        _context = context;
    }
    
    public IActionResult OnGet(int? categoryId, bool all, string? search, int pageIndex = 1)
    {
        if (pageIndex < 1) return RedirectToPage(new {categoryId, all, search});
        const int rewardsPerPage = 5;
        List<int> rewardsIds;
        if (all)
        {
            rewardsIds = search == null ? _context.Rewards.Select(r => r.Id).ToList()
                : _context.Rewards.Where(r => r.Name.FuzzyMatch(search)).Select(r => r.Id).ToList();
        }
        else
        {
            categoryId ??= _context.RewardCategories.FirstOrDefault()?.Id;
            if (categoryId == null) return RedirectToPage("ManageRewards", new { all = true });
            var category = _context.RewardCategories.Include(c => c.Rewards).SingleOrDefault(c => c.Id == categoryId);
            if (category == null) return RedirectToPage();
            rewardsIds = category.Rewards.Where(r => search == null || r.Name.FuzzyMatch(search)).Select(r => r.Id).ToList();
            ViewData["activeCategoryId"] = categoryId;
        }

        var totalRewardsCount = rewardsIds.Count;
        if (totalRewardsCount > 0 && rewardsPerPage * (pageIndex - 1) + 1 > totalRewardsCount) return RedirectToPage(new {categoryId, all, search});
        rewardsIds = rewardsIds.Skip(rewardsPerPage * (pageIndex-1)).Take(rewardsPerPage).ToList();

        var rewards = _context.Rewards.Include(r => r.Variants).Where(r => rewardsIds.Contains(r.Id)).ToList();

        ViewData["rewardsPerPage"] = rewardsPerPage;
        ViewData["pageIndex"] = pageIndex;
        ViewData["pageCount"] = (totalRewardsCount - 1) / rewardsPerPage + 1;
        ViewData["search"] = search;
        ViewData["rewards"] = rewards;
        ViewData["categories"] = _context.RewardCategories.ToList();
        return Page();
    }
}