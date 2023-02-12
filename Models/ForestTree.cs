namespace ReACT.Models;

public class ForestTree
{
    public int Id { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public bool Active { get; set; }
    
    public DateTime Date { get; set; }
    
    public double RecycledKg { get; set; }
    public ForestTreeVariant Variant { get; set; }

    public ForestTreeStage getStage()
    {
        var days = (DateTime.Now - Date).TotalDays;
        return days switch
        {
            <= 7 => ForestTreeStage.Seedling,
            <= 21 => ForestTreeStage.Sapling,
            _ => ForestTreeStage.Tree
        };
    }
}

public enum ForestTreeStage
{
    Seedling,
    Sapling,
    Tree
}

public enum ForestTreeVariant
{
    Oak,
    Pine
}