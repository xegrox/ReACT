using System.ComponentModel.DataAnnotations;

namespace ReACT.ViewModels
{
    public class EditCompany
    {
        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }


        [DataType(DataType.Text)]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        public int ContactNo { get; set; }
    }
}
