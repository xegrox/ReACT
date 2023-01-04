using Microsoft.Build.Framework;

namespace ReACT.Areas.Admin.Views.Shared.ManageRewards;

public class AddCategoryModalPartialModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Icon { get; set; }
}