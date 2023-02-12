namespace ReACT.Models;

public class RewardHistory
{
    public int Id { get; set; }

    public string UserId { get; set; }
    
    public string RewardName { get; set; }
    
    public string VariantName { get; set; }
    
    public string RedeemUrl { get; set; }
    
    public string RecipientEmail { get; set; }
    
    public string Code { get; set; }
    
    public DateTime Date { get; set; }
}