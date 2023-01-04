using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ReACT.Models;

namespace ReACT.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RewardController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(
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
            id: MockRewardsDb.Rewards.Last().Id + 1,
            name: name,
            categoryId: categoryId,
            imageUrl: imageUrl,
            redeemUrl: redeemUrl
        );

        MockRewardsDb.Rewards.Add(reward);
        MockRewardsDb.Variants.AddRange(variants.Select(v => new RewardVariant(
            id: MockRewardsDb.Variants.Last().Id + 1,
            rewardId: reward.Id,
            name: v.Name,
            points: v.Points
        )));
        return new OkResult();
    }


    [HttpGet("{id:int}")]
    public ActionResult Get(int id)
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
    public async Task<ActionResult> Update(
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
                id: MockRewardsDb.Variants.Last().Id + 1,
                rewardId: reward.Id,
                name: variantFormModel.Name,
                points: variantFormModel.Points
            ));
        }

        return new OkResult();
    }
}

public class VariantFormModel
{
    public int? Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] [Range(1, int.MaxValue)] public int Points { get; set; }
}