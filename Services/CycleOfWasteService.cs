using ReACT.Models;

namespace ReACT.Services
{
    public class CycleOfWasteService
    {
        private readonly AuthDbContext _context;

        public CycleOfWasteService(AuthDbContext context)
        {
            _context = context;
        }

        public List<CycleOfWaste> GetAll()
        {
            return _context.CycleOfWaste.OrderBy(d => d.ActivityDate).ToList();
        }

        public void AddPrize(CycleOfWaste oneTry)
        {
            _context.CycleOfWaste.Add(oneTry);
            _context.SaveChanges();
        }
    }
}
