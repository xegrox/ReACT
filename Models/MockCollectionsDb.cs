namespace ReACT.Models
{
    public class MockCollectionsDb
    {
        public static readonly List<Collection> Collections = new()
        {
            new Collection(1, 1, "2023-5-1", "Company A"),
            new Collection(2, 2, "2022-12-12", "Company B"),
            new Collection(3, 3, "2023-1-1", "Company C")
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
