using System.ComponentModel.DataAnnotations;

namespace ReACT.Models
{
    public class Thread
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;
        public string Status { get; set; } = "shown";
    }
}
