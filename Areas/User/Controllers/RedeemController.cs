using Microsoft.AspNetCore.Mvc;
using ReACT.Helpers;
using ReACT.Models;

namespace ReACT.Areas.User.Controllers;

[Area("User")]
public class RedeemController : Controller
{
    
    public IActionResult ModalRootFrame()
    {
        return ViewComponent("RedeemModalRoot");
    }

    public IActionResult RewardListFrame(int categoryId)
    {
        return ViewComponent("RedeemModalRewardList", new { categoryId });
    }

    public IActionResult RewardDetailsFrame(int rewardId)
    {
        return ViewComponent("RedeemModalRewardDetails", new { rewardId });
    }
}