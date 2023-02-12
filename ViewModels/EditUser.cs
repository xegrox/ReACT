using System.ComponentModel.DataAnnotations;

namespace ReACT.ViewModels
{
    public class EditUser
    {
        [Required]
        [DataType(DataType.Text)]
        public string? FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string? LastName { get; set; }


        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }


        [DataType(DataType.Text)]
        public string? Address { get; set; }

        public bool PublicPrivate { get; set; }

    }
}
