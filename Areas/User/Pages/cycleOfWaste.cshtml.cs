using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReACT.Models;
using ReACT.Services;

namespace ReACT.Areas.User.Pages
{
    [Authorize]
    public class cycleOfWasteModel : PageModel
    {
        private readonly CycleOfWasteService _cycleOfWasteService;
        private readonly EditCycleOfWasteService _editCycleOfWasteService;
        private readonly UserManager<ApplicationUser> _userManager;
        private AuthDbContext _authDbContext;

        [BindProperty]
        public string prizeName { get; set; }

        public ApplicationUser? currentUser { get; set; }

        public List<CycleOfWaste_prizes> ListOfPrizes { get; set; }

        public string? chosenPrize { get; set; }

        public cycleOfWasteModel(CycleOfWasteService cycleOfWasteService, UserManager<ApplicationUser> userManager, AuthDbContext authDbContext, EditCycleOfWasteService editCycleOfWasteService)
        {
            _cycleOfWasteService = cycleOfWasteService;
            _userManager = userManager;
            _authDbContext = authDbContext;
            _editCycleOfWasteService = editCycleOfWasteService;
        }

        public void OnGet()
        {
            ListOfPrizes = _editCycleOfWasteService.GetAll();
            var currentUserId = _userManager.GetUserId(User);
            ApplicationUser? currentUser1 = _authDbContext.Users.FirstOrDefault(x => x.Id.Equals(currentUserId));
            if (currentUser1 != null)
            {
                if (DateTime.Now.ToShortTimeString() == "00:00")
                {
                    currentUser1.chances_Default = 3;
                    currentUser1.chances_Free = 0;
                    currentUser1.postComments_counter = 0;
                    currentUser1.chance_CommentTask = 0;
                    currentUser1.chance_TreeTask = 0;
                }
                currentUser = currentUser1;
            }
        }

        public IActionResult OnPostAddPrize()
        {
            var points_spent = 0;
            List<CycleOfWaste_prizes> ListOfPrizes = _editCycleOfWasteService.GetAll();
            var currentUserId = _userManager.GetUserId(User);
            ApplicationUser? currentUser = _authDbContext.Users.FirstOrDefault(x => x.Id.Equals(currentUserId));
            if (currentUser != null)
            {
                if (currentUser.chances_Free != 0)
                {
                    if (currentUser.chance_TreeTask == 1 && currentUser.used_TreeTask == false)
                    {
                        currentUser.used_TreeTask = true;
                        currentUser.chances_Free -= 1;
                    }
                    else if (currentUser.chance_CommentTask == 1 && currentUser.used_CommentTask == false)
                    {
                        currentUser.used_CommentTask = true;
                        currentUser.chances_Free -= 1;
                    }
                }
                else
                {
                    points_spent = 50;
                    currentUser.chances_Default -= 1;
                }

                var random = new Random();
                var randomIndex = random.Next(ListOfPrizes.Count);
                var randomLabel = ListOfPrizes[randomIndex];
                chosenPrize = randomLabel.Name;

                //switch (prizeName)
                //{
                //    case "10 points":
                //        points_earned = 10;
                //        break;
                //    case "30 points":
                //        points_earned = 30;
                //        break;
                //    case "100 points":
                //        points_earned = 100;
                //        break;
                //    case "300 points":
                //        points_earned = 300;
                //        break;
                //    default:
                //        break;
                //}
                currentUser.Total_Points -= points_spent;
                currentUser.Total_Points += randomLabel.Points_Worth;
                CycleOfWaste oneTry = new CycleOfWaste
                {
                    Points_spent = points_spent,
                    Points_earned = randomLabel.Points_Worth,
                    UserId = currentUser.Id,
                    User = currentUser
                };
                _cycleOfWasteService.AddPrize(oneTry);
                return RedirectToPage("cycleOfWaste");
            }
            return RedirectToPage("/Login");
        }
    }
}
