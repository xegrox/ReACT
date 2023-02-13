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

        public string CalcTime(DateTime dateTime)
        {
            var timeDiff = (int)DateTime.Now.Subtract(dateTime).TotalSeconds;

            //Less than a minute old
            if (timeDiff < 60) { return "Just now"; }
            //Less than an hour old; Count in minutes
            else if (timeDiff < 3600) { return $"{timeDiff / 60}min ago"; }
            //Less than a day old; Count in hours
            else if (timeDiff < 86400) { return $"{timeDiff / 3600}h ago"; }
            //Less than a month old; Count in days
            else if (timeDiff < 2635200) { return $"{timeDiff / 86400} days ago"; }
            //Less than a year old; Count in months
            else if (timeDiff < 31622400) { return $"{timeDiff / 2635400}mo. ago"; }
            //Over a year old; Count in years
            else { return $"{timeDiff / 31622400}yr ago"; }
        }
    }
}

