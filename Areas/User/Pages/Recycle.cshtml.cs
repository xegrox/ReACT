using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Newtonsoft.Json.Serialization;
using ReACT.Models;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace ReACT.Areas.User.Pages;

[Authorize]
public class Recycle : PageModel
{
    private readonly MockCollectionsDb _mockCollectionsDb;

    public Recycle(MockCollectionsDb mockCollectionsDb)
    {
        _mockCollectionsDb = mockCollectionsDb;
    }
    [Required]
    public string CollectionDate { get; set; }
    [Required, RegularExpression(@"^[A-Za-z0-9]+(?:\s[A-Za-z0-9'_-]+)+$", ErrorMessage = "Invalid Address.")]
    public string Address { get; set; }
    public void OnGet()
    {
        
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            var nextId = MockCollectionsDb.Collections.Last().Id + 1;
            _mockCollectionsDb.AddCollection(new Collection(nextId, 1, CollectionDate, ""));
            return Redirect("/User/Dashboard");
        }
        return Page();
    }
}