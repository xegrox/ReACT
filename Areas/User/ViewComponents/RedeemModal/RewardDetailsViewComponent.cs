using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReACT.Models;

namespace ReACT.Areas.User.ViewComponents.RedeemModal;

public class RedeemModalRewardDetailsViewComponent : ViewComponent
{
    private readonly AuthDbContext _context;

    public RedeemModalRewardDetailsViewComponent(AuthDbContext context)
    {
        _context = context;
    }

    public Task<IViewComponentResult?> InvokeAsync(int rewardId)
    {
        var reward = _context.Rewards
            .Include(r => r.Variants)
            .Include(r => r.Category)
            .SingleOrDefault(reward => reward.Id == rewardId);
        return reward == null ? Task.FromResult<IViewComponentResult?>(null) : Task.FromResult<IViewComponentResult?>(View(reward));
    }
}