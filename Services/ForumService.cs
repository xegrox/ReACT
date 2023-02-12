using ReACT.Models;

namespace ReACT.Services
{
    public class ForumService
    {
        private readonly AuthDbContext _context;

        public ForumService(AuthDbContext context)
        {
            _context = context;
        }

        public List<Models.Thread> GetAll()
        {
            return _context.Threads.OrderBy(f => f.Id).ToList();
        }

        public Models.Thread? GetThread(int id)
        {
            Models.Thread? thread = _context.Threads.FirstOrDefault(f => f.Id == id);
            return thread;
        }

        public void AddThread(Models.Thread thread)
        {
            _context.Threads.Add(thread);
            _context.SaveChanges();
        }

        public void UpdateThread(Models.Thread thread)
        {
            _context.Threads.Update(thread);
            _context.SaveChanges();
        }

        //public void CalcTime(DateTime dateTime)
        //{
        //    var dateDiff = (DateTime.Now - dateTime).Ticks;
        //    
        //}
    }
}

