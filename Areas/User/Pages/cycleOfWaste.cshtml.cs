using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReACT.Models;
using ReACT.Services;

namespace ReACT.Areas.User.Pages
{
    [Authorize(Roles = "Admin, User")]
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
        public List<string> ListOfPrizes_name { get; set; } = new List<string>();

        public int? chosenPrize { get; set; }

        public cycleOfWasteModel(CycleOfWasteService cycleOfWasteService, UserManager<ApplicationUser> userManager, AuthDbContext authDbContext, EditCycleOfWasteService editCycleOfWasteService)
        {
            _cycleOfWasteService = cycleOfWasteService;
            _userManager = userManager;
            _authDbContext = authDbContext;
            _editCycleOfWasteService = editCycleOfWasteService;
        }

        public void OnGet()
        {
            var prize_list = _editCycleOfWasteService.GetAll();
            ListOfPrizes = prize_list;
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
            foreach (var prize in prize_list)
            {
                var variantName = _authDbContext.RewardVariants.FirstOrDefault(x => x.Id.Equals(prize.RewardVariantId));
                ListOfPrizes_name.Add(variantName.Name + " " + prize.Name);
            }
        }

        public async Task<IActionResult> OnPost()
        {
            var points_spent = 0;
            List<CycleOfWaste_prizes> ListOfPrizes = _editCycleOfWasteService.GetAll();
            var currentUser = await _userManager.GetUserAsync(User);
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
            var randomPrize = ListOfPrizes[randomIndex];
            var variant = _authDbContext.RewardVariants.Include(r => r.Reward).First(v => v.Id == randomPrize.RewardVariantId);

            currentUser.Total_Points -= points_spent;
            currentUser.Total_Points += randomPrize.Points;
            CycleOfWaste oneTry = new CycleOfWaste
            {
                Points_spent = points_spent,
                Points_earned = randomPrize.Points,
                UserId = currentUser.Id,
                User = currentUser
            };
            _cycleOfWasteService.AddPrize(oneTry);
                
            return new JsonResult(new
            {
                id = oneTry.Id,
                name = variant.Reward.Name,
                variantName = variant.Name,
                imageUrl = variant.Reward.ImageUrl
            });
        }
    }
}
