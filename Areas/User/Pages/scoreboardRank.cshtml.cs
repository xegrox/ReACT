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
        public IDictionary<int, ApplicationUser> currentUser = new Dictionary<int, ApplicationUser>();

        public scoreboardRankModel(AuthDbContext authDbContext, UserManager<ApplicationUser> userManager)
        {
            _authDbContext = authDbContext;
            _userManager = userManager;
        }

        public async Task OnGet()
        {
            var manyUsers = await _userManager.GetUsersInRoleAsync("User");
            UsersList = manyUsers.OrderByDescending(x => x.Total_Points).ToList();
            for (int i = 0; i < UsersList.Count; i++)
            {
                if (UsersList[i].PublicPrivate == true)
                {
                    var toBeAddedFNStr = "";
                    var toBeAddedLNStr = "";

                    if (UsersList[i].FirstName.Length > 2)
                    {
                        var privateFNStr = UsersList[i].FirstName.Substring(2);
                        foreach (var a in privateFNStr)
                        {
                            toBeAddedFNStr += "*";
                        }
                        UsersList[i].FirstName = UsersList[i].FirstName.Substring(0, 2) + toBeAddedFNStr;
                    }
                    else
                    {
                        foreach (var b in UsersList[i].FirstName)
                        {
                            toBeAddedFNStr += "*";
                        }
                        UsersList[i].FirstName = toBeAddedFNStr;
                    }
                    if (UsersList[i].LastName.Length > 2)
                    {
                        for (int j = 0; j < UsersList[i].LastName.Length - 2; j++)
                        {
                            toBeAddedLNStr += "*";
                        }
                        UsersList[i].LastName = toBeAddedLNStr + UsersList[i].LastName.Substring(UsersList[i].LastName.Length - 2);
                    }
                    else
                    {
                        foreach (var c in UsersList[i].LastName)
                        {
                            toBeAddedLNStr += "*";
                        }
                        UsersList[i].LastName = toBeAddedLNStr;
                    }
                }
                userRankings.Add(i + 1, UsersList[i]);
                if (UsersList[i].Id == _userManager.GetUserId(User))
                {
                    currentUser.Add(i + 1, UsersList[i]);
                }
            }
        }
    }
}
