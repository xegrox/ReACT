using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReACT.Models;

namespace ReACT.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RewardController : ControllerBase
{
    
    private readonly AuthDbContext _context;
    
    public RewardController(AuthDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult> CreateReward(
        [Required] [FromForm] int categoryId,
        [Required] [FromForm] string name,
        [Required] [FromForm] string redeemUrl,
        [Required] IFormFile image,
        [Required] [FromForm] List<VariantFormModel> variants,
        [FromServices] IWebHostEnvironment env)
    {
        var category = await _context.RewardCategories.FindAsync(categoryId);
        if (category == null) return new BadRequestResult();
        
        var imageUrl = $"/images/uploads/{Guid.NewGuid().ToString()}{Path.GetExtension(image.FileName)}";
        var imagePath = Path.Join(env.WebRootPath, imageUrl);
        Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);
        var imageStream = new FileStream(imagePath, FileMode.Create);
        await image.CopyToAsync(imageStream);

        var reward = new Reward
        {
            CategoryId = categoryId,
            Name = name,
            ImageUrl = imageUrl,
            RedeemUrl = redeemUrl,
            Variants = variants.Select(v => new RewardVariant
            {
                Name = v.Name,
                Points = v.Points
            }).ToHashSet()
        };
        
        category.Rewards.Add(reward);
        await _context.SaveChangesAsync();
        return new OkResult();
    }


    [HttpGet("{id:int}")]
    public ActionResult GetReward(int id)
    {
        var reward = _context.Rewards.Include(r => r.Variants).SingleOrDefault(r => r.Id == id);
        if (reward == null) return new NotFoundResult();
        return new JsonResult(new
        {
            id,
            categoryId = reward.CategoryId,
            name = reward.Name,
            imageUrl = reward.ImageUrl,
            redeemUrl = reward.RedeemUrl,
            reward.Variants
        });
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateReward(
        int id,
        [Required] [FromForm] int categoryId,
        [Required] [FromForm] string name,
        [Required] [FromForm] string redeemUrl,
        IFormFile? image,
        [Required] [FromForm] List<VariantFormModel> variants,
        [FromServices] IWebHostEnvironment env)
    {
        var reward = _context.Rewards.Include(r => r.Variants).SingleOrDefault(r => r.Id == id);
        if (reward == null) return new NotFoundResult();

        if (image != null)
        {
            var oldImagePath = Path.Join(env.WebRootPath, reward.ImageUrl);
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            var imageUrl = $"/images/uploads/{Guid.NewGuid().ToString()}{Path.GetExtension(image.FileName)}";
            var imagePath = Path.Join(env.WebRootPath, imageUrl);
            Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);
            var imageStream = new FileStream(imagePath, FileMode.Create);
            await image.CopyToAsync(imageStream);
            reward.ImageUrl = imageUrl;
        }

        reward.CategoryId = categoryId;
        reward.Name = name;
        reward.RedeemUrl = redeemUrl;

        foreach (var variantFormModel in variants)
        {
            if (variantFormModel.Id != null)
            {
                var variant = reward.Variants.SingleOrDefault(v => v.Id == variantFormModel.Id);
                if (variant != null)
                {
                    variant.Name = variantFormModel.Name;
                    variant.Points = variantFormModel.Points;
                    continue;
                }
            }

            reward.Variants.Add(new RewardVariant {
                Name = variantFormModel.Name,
                Points = variantFormModel.Points
            });
        }

        await _context.SaveChangesAsync();
        return new OkResult();
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteReward(int id)
    {
        var reward = _context.Rewards.Find(id);
        if (reward == null) return new NotFoundResult();
        _context.Rewards.Remove(reward);
        _context.SaveChanges();
        return new OkResult();
    }

    [HttpGet("Category")]
    public ActionResult GetCategories()
    {
        return new JsonResult(_context.RewardCategories.Select(category => new
        {
            id = category.Id,
            name = category.Name,
            icon = category.Icon
        }));
    }

    [HttpPost("Category")]
    public ActionResult CreateCategory(
        [Required] [FromForm] string name,
        [Required] [FromForm] string icon)
    {
        var category = new RewardCategory {
            Name = name,
            Icon = icon
        };
        _context.RewardCategories.Add(category);
        _context.SaveChanges();
        return new JsonResult(category);
    }
    
    
    [HttpPut("Category/{id:int}")]
    public ActionResult UpdateCategory(
        int id,
        [Required] [FromForm] string name,
        [Required] [FromForm] string icon)
    {
        var category = _context.RewardCategories.Find(id);
        if (category == null) return NotFound();
        category.Name = name;
        category.Icon = icon;
        _context.SaveChanges();
        return new OkResult();
    }

    [HttpDelete("Category/{id:int}")]
    public ActionResult DeleteCategory(int id)
    {
        var category = _context.RewardCategories.Find(id);
        if (category == null) return new NotFoundResult();
        _context.RewardCategories.Remove(category);
        _context.SaveChanges();
        return new OkResult();
    }
}

public class VariantFormModel
{
    public int? Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] [Range(1, int.MaxValue)] public int Points { get; set; }
}