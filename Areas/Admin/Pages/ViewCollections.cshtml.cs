using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReACT.Models;
using ReACT.Services;

namespace ReACT.Areas.Admin.Pages;

public class ViewCollectionModel : PageModel
{
    private readonly CollectionService _collectionService;

    public ViewCollectionModel(CollectionService collectionService)
    {
        _collectionService = collectionService;
    }
    public List<Collection> CollectionList { get; set; } = new();
    public void OnGet()
    {
        CollectionList = _collectionService.GetCollections();
    }

}