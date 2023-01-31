namespace ReACT.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CollectionDate { get; set; }
        public string AssignedCompany { get; set; }
        public string Status { get; set; }

        public decimal TotalWeight { get; set; }

        public int PointsAllocated { get; set; }

        public Company? Company { get; set; }
           
    }
}
