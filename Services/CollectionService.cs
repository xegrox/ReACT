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

        public List<Collection> GetPendingCollections()
        {
            return _context.Collections.Where(x => x.Status == "Not Completed").ToList();
        }

        public List<Collection> GetCompletedCollections()
        {
            return _context.Collections.Where(x => x.Status == "Completed").ToList();
        }

        public List<Collection> GetIncompleteCollectionsByCompany(string companyName)
        {
            return _context.Collections.Where(x => x.AssignedCompany == companyName && x.Status == "Not Completed").ToList();
        }

        public List<Collection> GetCollectionsByCompany(string companyName)
        {
            return _context.Collections.Where(x => x.AssignedCompany == companyName && x.Status == "Completed").ToList();
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
