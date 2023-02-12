using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReACT.Models;
using ReACT.Services;

namespace ReACT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CycleOfWasteController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _authDbContext;
        private readonly CycleOfWasteService _cycleOfWasteService;

        public CycleOfWasteController(UserManager<ApplicationUser> userManager, AuthDbContext authDbContext, CycleOfWasteService cycleOfWasteService)
        {
            _userManager = userManager;
            _authDbContext = authDbContext;
            _cycleOfWasteService = cycleOfWasteService;
        }

        [HttpPost, Authorize]
        public async Task<ActionResult> RandomReward()
        {
            var points_spent = 0;

            // if no rewards in db
            if (_authDbContext.CycleOfWaste_prizes.Count() == 0)
            {
                var vnew = new RewardVariant { Name = "$10", Points = 100 };
                var rnew = new Reward { Name = "test", ImageUrl = "asd", RedeemUrl = "", Variants = new[] { vnew } };
                var cnew = new RewardCategory { Name="test", Icon = "", Rewards = new[] { rnew } };
                _authDbContext.RewardCategories.Add(cnew);
                _authDbContext.CycleOfWaste_prizes.Add(new CycleOfWaste_prizes { 
                    Name = "",
                    RewardVariant = vnew,
                    VisibleToUser = true
                });
                await _authDbContext.SaveChangesAsync();
            }
            var ListOfPrizes = _authDbContext.CycleOfWaste_prizes.ToList();
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.chances_Free != 0)
            {
                points_spent = 0;
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

            PointsHistory addRow = new PointsHistory
            {
                userId = currentUser.Id,
                points_spent = points_spent,
                activity_description = "Spin the Cycle of Waste to get " + variant.Name + " " + variant.Reward.Name,
                points_gained = randomPrize.Points
            };

            _authDbContext.PointsHistory.Add(addRow);
            await _authDbContext.SaveChangesAsync();

            await _userManager.UpdateAsync(currentUser);

            return new JsonResult(new
            {
                id = variant.Id,
                name = variant.Reward.Name,
                variantName = variant.Name,
                imageUrl = variant.Reward.ImageUrl
            });
        }
    }
}
