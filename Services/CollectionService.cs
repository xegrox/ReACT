using ReACT.Models;

namespace ReACT.Services
{
    public class CollectionService
    {
        private readonly AuthDbContext _context;
        public CollectionService(AuthDbContext context)
        {
            _context = context;
        }

        public List<Collection> GetCollections()
        {
            return _context.Collections.OrderBy(x => x.Id).ToList();
        }

        public Collection? GetCollection(int id)
        {
            Collection? collection = _context.Collections.FirstOrDefault(x => x.Id == id);
            return collection;
        }

        public void AddCollection(Collection collection)
        {
            _context.Collections.Add(collection);
            _context.SaveChanges();
        }

        public void UpdateCollection(Collection collection)
        {
            _context.Collections.Update(collection);
            _context.SaveChanges();
        }

        public void DeleteCollection(int id)
        {
            var collection = _context.Collections.FirstOrDefault(x => x.Id == id);
            _context.Collections.Remove(collection);
            _context.SaveChanges();
        }
    }
}
