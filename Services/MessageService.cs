using ReACT.Models;

namespace ReACT.Services
{
    public class MessageService
    {
        private readonly AuthDbContext _context;

        public MessageService(AuthDbContext context)
        {
            _context = context;
        }

        public List<Message> GetAll()
        {
            return _context.Messages.OrderBy(m => m.Id).ToList();
        }

        public Message? GetMessage(int id)
        {
            Message? message = _context.Messages.FirstOrDefault(m => m.Id == id);
            return message;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
        }

        public void UpdateMessage(Message message)
        {
            _context.Messages.Update(message);
            _context.SaveChanges();
        }

    }
}