using Microsoft.AspNetCore.Mvc;
using ReACT.Models;

namespace ReACT.Areas.User.ViewComponents.RedeemModal;

public class RedeemModalRootViewComponent : ViewComponent
{
    private readonly AuthDbContext _context;
    
    public RedeemModalRootViewComponent(AuthDbContext context)
    {
        _context = context;
    }
    
    public Task<IViewComponentResult> InvokeAsync()
    {
        return Task.FromResult<IViewComponentResult>(View(_context.RewardCategories.ToList()));
    }
}