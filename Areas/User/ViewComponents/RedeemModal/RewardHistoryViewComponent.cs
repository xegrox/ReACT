using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReACT.Models;

namespace ReACT.Areas.User.ViewComponents.RedeemModal;

public class RedeemModalRewardHistoryViewComponent : ViewComponent
{
    private readonly AuthDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public RedeemModalRewardHistoryViewComponent(AuthDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public Task<IViewComponentResult?> InvokeAsync()
    {
        var userId = _userManager.GetUserId(Request.HttpContext.User);
        var history = _context.RewardHistories.Where(u => u.UserId == userId).ToList();
        return Task.FromResult<IViewComponentResult?>(View(history));
    }
}