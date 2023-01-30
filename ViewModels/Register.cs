using System.ComponentModel.DataAnnotations;

namespace ReACT.ViewModels
{
    public class Register
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        
        [DataType(DataType.Text)]
        public string? Address { get; set; }

       
        public IFormFile? Profile { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }

    }
}
