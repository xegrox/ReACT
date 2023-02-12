using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReACT.Models;
using ReACT.Services;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ReACT.Areas.Company.Pages
{
    [Authorize(Roles = "Company")]
    public class ViewCollectionsModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager { get; }
        private AuthDbContext _authDbContext;
        private readonly CollectionService _collectionService;
        private readonly CompanyService _companyService;

        public ViewCollectionsModel(CollectionService collectionService, CompanyService companyService, UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
        {
            _collectionService = collectionService;
            _companyService = companyService;
            _userManager = userManager;
            _authDbContext = authDbContext;
        }
        public Models.Company Company { get; set; }
        public List<Collection> CollectionList { get; set; } = new();
        public ApplicationUser user { get; set; }
        public IActionResult OnGet(bool completed)
        {
            string userId = _userManager.GetUserId(User);
            ApplicationUser? currentUser = _authDbContext.Users.FirstOrDefault(x => x.Id.Equals(userId));
            user = currentUser;

            if (completed)
            {
                CollectionList = _collectionService.GetCollectionsByCompany(user.FirstName);
                ViewData["completed"] = 2;
            }
            else
            {
                CollectionList = _collectionService.GetIncompleteCollectionsByCompany(user.FirstName);
                ViewData["completed"] = 1;
            }

            return Page();
        }
    }
}
