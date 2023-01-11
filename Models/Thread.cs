using System.ComponentModel.DataAnnotations;

namespace ReACT.Models
{
    public class Thread
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required, StringLength(50, MinimumLength = 5, ErrorMessage = "The thread title must be between 5 and 50 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(300, ErrorMessage = "The thread description must be less than 300 characters.")]
        public string Content { get; set; } = string.Empty;

        [Required, MaxLength(75)]
        public string ImageURL { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public string Status { get; set; } = "shown";
    }
}

