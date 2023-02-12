using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReACT.Models;
using ReACT.Services;

namespace ReACT.Areas.User.Controllers;

[Authorize]
[Area("User")]
public class RedeemController : Controller
{

    private readonly AuthDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly EmailSender _email;

    public RedeemController(AuthDbContext context, UserManager<ApplicationUser> userManager, EmailSender email)
    {
        _context = context;
        _userManager = userManager;
        _email = email;
    }

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

    public IActionResult RewardHistoryFrame()
    {
        return ViewComponent("RedeemModalRewardHistory");
    }
    
    public async Task<ActionResult> ClaimReward(int variantId, [EmailAddress] string? recipient = null)
    {
        var variant = _context.RewardVariants.Include(v => v.Reward).FirstOrDefault(v => v.Id == variantId);
        if (variant == null) return new NotFoundResult();
        var code = _context.RewardCodes.FirstOrDefault(c => c.VariantId == variantId);
        if (code == null) return new NotFoundResult();
        var user = await _userManager.GetUserAsync(User);
        if (user.Total_Points < variant.Points) return new BadRequestResult();

        recipient ??= user.Email;
        await _email.SendEmailAsync(
            recipient, 
            $"Redeem {variant.Reward.Name} {variant.Name} code", 
            $"<p>Here is your reward code: <b>{code.Code}</b></p>" +
            $"<p>Please redeem it at <a href='{variant.Reward.RedeemUrl}'>{variant.Reward.RedeemUrl}</a></p>" +
            "<p>Thank you for using ReACT, happy recycling!</p>");
        _context.RewardCodes.Remove(code);
        variant.Popularity += 1;
        variant.Reward.Popularity += 1;
        _context.RewardHistories.Add(new RewardHistory
        {
            UserId = user.Id,
            RewardName = variant.Reward.Name,
            VariantName = variant.Name,
            RecipientEmail = recipient,
            RedeemUrl = variant.Reward.RedeemUrl,
            Date = DateTime.Now,
            Code = code.Code
        });
        await _context.SaveChangesAsync();
        user.Total_Points -= variant.Points;
        await _userManager.UpdateAsync(user);
        return new OkResult();
    }

    public async Task<IActionResult> ResendCode(int historyId)
    {
        var userId = _userManager.GetUserId(User);
        var history = await _context.RewardHistories.FindAsync(historyId);
        if (history == null || history.UserId != userId) return new NotFoundResult();
        await _email.SendEmailAsync(
            history.RecipientEmail,
            $"Resend: Redeem {history.RewardName} {history.VariantName} code", 
            $"<p>Here is your reward code: <b>{history.Code}</b></p>" +
            $"<p>Please redeem it at <a href='{history.RedeemUrl}'>{history.RedeemUrl}</a></p>" +
            "<p>Thank you for using ReACT, happy recycling!</p>");
        return new OkResult();
    }
}