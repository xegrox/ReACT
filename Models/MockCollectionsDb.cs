namespace ReACT.Models
{
    public class MockCollectionsDb
    {
        public static readonly List<Collection> Collections = new()
        {
            new Collection(1, 1, "1-5-2023", "Company A"),
            new Collection(2, 2, "12-12-2022", "Company B"),
            new Collection(3, 3, "1-1-2023", "Company C")
        };

        public List<Collection> GetAll()
        {
            return Collections.OrderBy(x => x.Id).ToList();
        }

        public Collection? GetCollection(int id)
        {
            Collection? collection = Collections.FirstOrDefault(x => x.Id == id);
            return collection;
        }

        public void AddCollection(Collection collection)
        {
            Collections.Add(collection);
        }
    }
}
