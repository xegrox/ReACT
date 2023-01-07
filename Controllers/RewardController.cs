using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ReACT.Models;

namespace ReACT.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RewardController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateReward(
        [Required] [FromForm] int categoryId,
        [Required] [FromForm] string name,
        [Required] [FromForm] string redeemUrl,
        [Required] IFormFile image,
        [Required] [FromForm] List<VariantFormModel> variants,
        [FromServices] IWebHostEnvironment env)
    {
        var imageUrl = $"/images/uploads/{Guid.NewGuid().ToString()}{Path.GetExtension(image.FileName)}";
        var imagePath = Path.Join(env.WebRootPath, imageUrl);
        Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);
        var imageStream = new FileStream(imagePath, FileMode.Create);
        await image.CopyToAsync(imageStream);

        var reward = new Reward(
            id: (MockRewardsDb.Rewards.LastOrDefault()?.Id ?? 0) + 1,
            name: name,
            categoryId: categoryId,
            imageUrl: imageUrl,
            redeemUrl: redeemUrl
        );

        MockRewardsDb.Rewards.Add(reward);
        MockRewardsDb.Variants.AddRange(variants.Select(v => new RewardVariant(
            id: (MockRewardsDb.Variants.LastOrDefault()?.Id ?? 0) + 1,
            rewardId: reward.Id,
            name: v.Name,
            points: v.Points
        )));
        return new OkResult();
    }


    [HttpGet("{id:int}")]
    public ActionResult GetReward(int id)
    {
        var reward = MockRewardsDb.Rewards.Find(r => r.Id == id);
        if (reward == null) return new NotFoundResult();
        var variants = MockRewardsDb.Variants.Where(v => v.RewardId == id);
        return new JsonResult(new
        {
            id,
            categoryId = reward.CategoryId,
            name = reward.Name,
            imageUrl = reward.ImageUrl,
            redeemUrl = reward.RedeemUrl,
            variants
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
        var reward = MockRewardsDb.Rewards.Find(r => r.Id == id);
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
                var variant = MockRewardsDb.Variants.Find(v => v.Id == variantFormModel.Id);
                if (variant != null)
                {
                    variant.Name = variantFormModel.Name;
                    variant.Points = variantFormModel.Points;
                    continue;
                }
            }

            MockRewardsDb.Variants.Add(new RewardVariant(
                id: (MockRewardsDb.Variants.LastOrDefault()?.Id ?? 0) + 1,
                rewardId: reward.Id,
                name: variantFormModel.Name,
                points: variantFormModel.Points
            ));
        }

        return new OkResult();
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteReward(int id)
    {
        var reward = MockRewardsDb.Rewards.Find(r => r.Id == id);
        if (reward == null) return new NotFoundResult();
        MockRewardsDb.Rewards.Remove(reward);
        return new OkResult();
    }

    [HttpGet("Category")]
    public ActionResult GetCategories()
    {
        return new JsonResult(MockRewardsDb.Categories.Select(category => new
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
        var category = new RewardCategory(
            id: (MockRewardsDb.Categories.LastOrDefault()?.Id ?? 0) + 1,
            name, icon
        );
        MockRewardsDb.Categories.Add(category);
        return new JsonResult(category);
    }
    
    
    [HttpPut("Category/{id:int}")]
    public ActionResult UpdateCategory(
        int id,
        [Required] [FromForm] string name,
        [Required] [FromForm] string icon)
    {
        var category = MockRewardsDb.Categories.Find(c => c.Id == id);
        if (category == null) return NotFound();
        category.Name = name;
        category.Icon = icon;
        return new OkResult();
    }

    [HttpDelete("Category/{id:int}")]
    public ActionResult DeleteCategory(int id)
    {
        var category = MockRewardsDb.Categories.Find(c => c.Id == id);
        if (category == null) return new NotFoundResult();
        MockRewardsDb.Categories.Remove(category);
        return new OkResult();
    }
}

public class VariantFormModel
{
    public int? Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] [Range(1, int.MaxValue)] public int Points { get; set; }
}