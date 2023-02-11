using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReACT.Models;
using ReACT.Services;

namespace ReACT.Areas.User.Controllers
{
    public class CollectionController : Controller
    {
        private UserManager<ApplicationUser> _userManager { get; }
        private AuthDbContext _authDbContext;
        private readonly CollectionService _collectionService;
        private readonly CompanyService _companyService;
        public CollectionController(CollectionService collectionService, CompanyService companyService, UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
        {
            _collectionService = collectionService;
            _companyService = companyService;
            _userManager = userManager;
            _authDbContext = authDbContext;
        }
        public ActionResult Index()
        {
            var collectionList = new List<Collection>();//load data from database
            return PartialView(collectionList);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var collection = _collectionService.GetCollection(id);//load data from database by RestaurantId
            return PartialView(collection);
        }
    }
}
