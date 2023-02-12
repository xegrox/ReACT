using System;

namespace ReACT.Models
{
	public class Message
	{
		public int Id { get; set; }

		public string Content { get; set; } = string.Empty;

		public DateTime Date { get; set; } = DateTime.Now;
		public string status { get; set; } = "shown";
		public int replyTo { get; set; } = 0;

        public string UserId { get; set; } = string.Empty;
		public string UserName { get; set; } = string.Empty;

		public int threadId { get; set; }
    }
}

