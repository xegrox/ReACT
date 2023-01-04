namespace ReACT.Models
{
    public class RecyclingType
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string PointsPerKg { get; set; }
        public RecyclingType(int id, string name, string pointsPerKg)
        {
            Id = id;
            Name = name;
            PointsPerKg = pointsPerKg;
        }
    }
}
