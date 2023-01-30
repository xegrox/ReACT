namespace ReACT.Models
{
    public class MockThreadsDb
    {
        public static List<Thread> Threads = new()
        {
            new Thread{Id = 1, Title = "General", Content = "Talk about stuff", ImageURL = "", Date = new DateTime(2022, 01, 05, 10, 0, 0), Status = "shown" },
            new Thread{Id = 2, Title = "Recycling", Content = "Talk about recycling", ImageURL = "", Date = DateTime.Now, Status = "shown" },
            new Thread{Id = 3, Title = "New Year's Event 2023", Content = "Thread for New Year's 2023 recycling event", ImageURL = "", Date = new DateTime(2022, 1, 1, 0, 0, 0), Status = "hidden" }
        };

        public List<Thread> GetAll()
        {
            return Threads.OrderBy(x => x.Id).ToList();
        }

        public Thread? GetThread(int id)
        {
            Thread? thread = Threads.FirstOrDefault(x => x.Id == id);
            return thread;
        }

        public void AddThread(Thread thread)
        {
            Threads.Add(thread);
        }

        //public void CalcPostTime(DateTime dateTime)
        //{
        //    var dateDiff = (DateTime.Now - dateTime).Ticks;
        //}
    }
}

