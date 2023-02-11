using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;

namespace ReACT.Areas.User.Pages
{
    [Authorize]
    public class pointsHistoryModel : PageModel
    {
        private readonly AuthDbContext _authDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public pointsHistoryModel(UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
        {
            _userManager = userManager;
            _authDbContext = authDbContext;
        }
        public List<PointsHistory> PointsHistoryList { get; set; } = new();

        public void OnGet()
        {
            PointsHistoryList = _authDbContext.PointsHistory.Where(x => x.userId.Equals(_userManager.GetUserId(User))).OrderBy(x => x.id).ToList();
        }
    }
}
