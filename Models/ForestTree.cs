namespace ReACT.Models;

public class ForestTree
{
    public int Id { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public bool Active { get; set; }
    
    public DateTime Date { get; set; }
    
    public int RecycledKg { get; set; }
    public ForestTreeVariant Variant { get; set; }
}

public enum ForestTreeVariant
{
    Oak
}