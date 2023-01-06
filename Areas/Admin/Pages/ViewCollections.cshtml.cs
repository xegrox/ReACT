using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;

namespace ReACT.Areas.Admin.Pages;

public class ViewCollectionModel : PageModel
{
    private readonly MockCollectionsDb _mockCollectionsDb;

    public ViewCollectionModel(MockCollectionsDb mockCollectionsDb)
    {
        _mockCollectionsDb = mockCollectionsDb;
    }
    public List<Collection> CollectionList { get; set; } = new();
    public void OnGet()
    {
        CollectionList = _mockCollectionsDb.GetAll();
    }

}