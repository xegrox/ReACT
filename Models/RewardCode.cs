namespace ReACT.Models;

public class RewardCode
{
    public int Id { get; set; }
    public int VariantId { get; set; }
    public string Code { get; set; }
    
    public RewardVariant Variant { get; set; }
}