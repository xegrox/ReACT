using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using System.Runtime.Intrinsics.X86;

namespace ReACT.Areas.User.Pages
{
    [Authorize(Roles = "Admin, User")]
    public class scoreboardRankModel : PageModel
    {
        private readonly AuthDbContext _authDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public List<ApplicationUser> UsersList { get; set; } = new();
        public IDictionary<int, ApplicationUser> userRankings = new Dictionary<int, ApplicationUser>();

        public scoreboardRankModel(AuthDbContext authDbContext, UserManager<ApplicationUser> userManager)
        {
            _authDbContext = authDbContext;
            _userManager = userManager;
        }

        public void OnGet()
        {
            UsersList = _authDbContext.Users.OrderByDescending(x => x.Total_Points).ToList();
            //for (int i = 0; i < UsersList.Count; i++)
            //{
            //    if (UsersList[i].PublicPrivate)
            //    {
            //        var privateFNStr = UsersList[i].FirstName.Substring(2, UsersList[i].FirstName.Length);
            //        var toBeAddedFNStr = "";
            //        var toBeAddedLNStr = "";
            //        foreach (var a in privateFNStr)
            //        {
            //            toBeAddedFNStr += "*";
            //        }
            //        for (int j = 0; j < UsersList[i].LastName.Length -2; j++)
            //        {
            //            toBeAddedLNStr += "*";
            //        }
            //        UsersList[i].FirstName = UsersList[i].FirstName.Substring(0, 2) + toBeAddedFNStr;
            //        UsersList[i].LastName = toBeAddedLNStr;
            //    }
            //    userRankings.Add(i + 1, UsersList[i]);
            //}
        }
    }
}
