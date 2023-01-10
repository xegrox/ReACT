using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Newtonsoft.Json.Serialization;
using ReACT.Models;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace ReACT.Areas.User.Pages;

public class Recycle : PageModel
{
    private readonly MockCollectionsDb _mockCollectionsDb;

    public Recycle(MockCollectionsDb mockCollectionsDb)
    {
        _mockCollectionsDb = mockCollectionsDb;
    }
    [BindProperty, Required]
    public string CollectionDate { get; set; }
    [BindProperty, Required]
    public string Address { get; set; }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            var nextId = MockCollectionsDb.Collections.Last().Id + 1;
            _mockCollectionsDb.AddCollection(new Collection(nextId, 1, CollectionDate, "Company T"));
            return Redirect("/User/Dashboard");
        }
        return Page();
    }
}