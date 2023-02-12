using System.ComponentModel.DataAnnotations;

namespace ReACT.ViewModels
{
    public class AddUser
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string? LastName { get; set; }


        [DataType(DataType.Text)]
        public string Address { get; set; }


        public IFormFile? Profile { get; set; }

        public bool PublicPrivate { get; set; }

        [Required]
        [MinLength(12, ErrorMessage = "Enter at least a 12 characters password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{12,18}$",
            ErrorMessage = "Passwords must be at least 12 characters long and contain at least an upper case letter, lower case letter, digit and a symbol")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }
    }
}
