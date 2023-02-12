using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using System.Data;

namespace ReACT.Areas.Admin.Pages;

[Authorize(Roles = "Admin")]
public class ViewCollectionModel : PageModel
{
    private readonly AuthDbContext _context;
    
    public ViewCollectionModel(AuthDbContext context)
    {
        _context = context;
    }
    
    public List<Collection> CollectionList { get; set; } = new();
    public void OnGet()
    {
        CollectionList = _context.Collections.ToList();
    }

}