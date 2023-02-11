using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReACT.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime CollectionDate { get; set; }
        public string AssignedCompany { get; set; }
        public string Status { get; set; }

        [Range(0.5, 10000)]
        [Column(TypeName = "decimal(7,2)")]
        public decimal TotalWeight { get; set; }

        public int PointsAllocated { get; set; }

        public Company? Company { get; set; }
           
    }
}
