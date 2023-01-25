using ReACT.Models;

namespace ReACT.Services
{
    public class RecyclingTypeService
    {
        private readonly AuthDbContext _context;

        public RecyclingTypeService(AuthDbContext context)
        {
            _context = context;
        }

        public List<RecyclingType> GetAllTypes()
        {
            RecyclingType? selfRecycled = _context.RecyclingType.FirstOrDefault(x => x.Name == "Self recycled");
            if (selfRecycled == null)
            {
                var ok1 = new RecyclingType()
                {
                    Name = "Self recycled",
                    PointsPerKg = 50
                };
                _context.RecyclingType.Add(ok1);
                _context.SaveChanges();
            }

            RecyclingType? collection = _context.RecyclingType.FirstOrDefault(x => x.Name == "Collection");
            if (collection == null)
            {
                var ok2 = new RecyclingType()
                {
                    Name = "Collection",
                    PointsPerKg = 20
                };
                _context.RecyclingType.Add(ok2);
                _context.SaveChanges();
            }

            return _context.RecyclingType.OrderBy(x => x.Id).ToList();
        }

        public RecyclingType? GetRecyclableTypeById(int id)
        {
            RecyclingType? recyclingType = _context.RecyclingType.FirstOrDefault(x => x.Id == id);
            return recyclingType;
        }

        public void AddRecyclingType(RecyclingType oneType)
        {
            _context.RecyclingType.Add(oneType);
            _context.SaveChanges();
        }

        public void UpdateRecyclingType(RecyclingType oneType)
        {
            _context.RecyclingType.Update(oneType);
            _context.SaveChanges();
        }

        public void DeleteRecyclingType(RecyclingType oneType)
        {
            _context.RecyclingType.Remove(oneType);
            _context.SaveChanges();
        }
    }
}
