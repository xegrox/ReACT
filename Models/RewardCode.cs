namespace ReACT.Models;

public class RewardCode
{
    public RewardCode(int id, int variantId, string code)
    {
        Id = id;
        VariantId = variantId;
        Code = code;
    }

    public int Id { get; set; }
    public int VariantId { get; set; }
    public string Code { get; set; }
}