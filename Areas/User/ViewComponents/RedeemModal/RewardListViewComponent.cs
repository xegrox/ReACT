using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReACT.Models;

namespace ReACT.Areas.User.ViewComponents.RedeemModal;

public class RedeemModalRewardListViewComponent : ViewComponent
{
    private readonly AuthDbContext _context;

    public RedeemModalRewardListViewComponent(AuthDbContext context)
    {
        _context = context;
    }

    public Task<IViewComponentResult?> InvokeAsync(int categoryId)
    {
        var category = _context.RewardCategories
            .Include(c => c.Rewards)
            .ThenInclude(r => r.Variants)
            .SingleOrDefault(c => c.Id == categoryId);
        return category == null ? Task.FromResult<IViewComponentResult?>(null) : Task.FromResult<IViewComponentResult?>(View(category));
    }
}