using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Newtonsoft.Json.Serialization;
using ReACT.Models;
using ReACT.Services;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace ReACT.Areas.User.Pages;

[Authorize]
public class Recycle : PageModel
{
    private readonly CollectionService _collectionService;
    private readonly CompanyService _companyService;
    private readonly AuthDbContext _context;
    private UserManager<ApplicationUser> UserManager { get; }

    public Recycle(AuthDbContext context, UserManager<ApplicationUser> userManager, CollectionService collectionService, CompanyService companyService)
    {
        _context = context;
        UserManager = userManager;
        _collectionService = collectionService;
        _companyService = companyService;
    }

    [BindProperty, Required]
    public string Date { get; set; }
    public DateTime ParsedDate { get; set; }

    [BindProperty, Required]
    public string Address { get; set; }
    public ApplicationUser user { get; set; }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            string userId = UserManager.GetUserId(User);
            ApplicationUser? currentUser = _context.Users.FirstOrDefault(x => x.Id.Equals(userId));
            user = currentUser;

            currentUser.Address = Address;
            await UserManager.UpdateAsync(currentUser);
            _context.SaveChanges();

            ParsedDate = DateTime.ParseExact(Date, "yyyy-MM-dd", CultureInfo.CurrentCulture);

            var collection = new Collection()
            {
                UserId = userId,
                Username = $"{user.FirstName} {user.LastName}",
                Address = Address,
                CollectionDate = ParsedDate.Date,
                AssignedCompany = "N/A",
                Status = "Not Completed"
            };
            _collectionService.AddCollection(collection);

            var company = new Models.Company()
            {
                Name = "Company Test",
                ContactNo = "12345678",
                Address = "Recycling Inc. 123456"
            };
            _companyService.AddCompany(company);

            return Redirect("/User/Dashboard");
        }
        return Page();
    }

    public IActionResult OnGetPaperSlips()
    {
        return new ViewResult
        {
            ViewName = "RecyclePaperSlips"
        };
    } 
}