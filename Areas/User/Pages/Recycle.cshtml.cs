using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Newtonsoft.Json.Serialization;
using ReACT.Models;
using ReACT.Services;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace ReACT.Areas.User.Pages;

[Authorize]
public class Recycle : PageModel
{
    private readonly CollectionService _collectionService;

    public Recycle(CollectionService collectionService)
    {
        _collectionService = collectionService;
    }

    [BindProperty, Required]
    public string CollectionDate { get; set; }
    [BindProperty, Required]
    public string Address { get; set; }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            var collection = new Collection()
            {
                CollectionDate = CollectionDate
            };
            _collectionService.AddCollection(collection);
            return Redirect("/User/Dashboard");
        }
        return Page();
    }
}