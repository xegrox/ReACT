using ReACT.Models;

namespace ReACT.Services
{
    public class EditCycleOfWasteService
    {
        private readonly AuthDbContext _context;

        public EditCycleOfWasteService(AuthDbContext context)
        {
            _context = context;
        }

        public List<CycleOfWaste_prizes> GetAll()
        {
            return _context.CycleOfWaste_prizes.Where(x => x.VisibleToUser.Equals(true)).ToList();
        }
    }
}
