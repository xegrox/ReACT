using AspNetCore;

namespace ReACT.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CollectionDate { get; set; }
        public string AssignedCompany { get; set; }

        public decimal TotalWeight { get; set; }

        public int PointsAllocated { get; set; }

        public  int CompanyID { get; set; }
        //public Companies? Company { get; set; }
           
    }
}
