namespace ReACT.Models
{
    public class Collection
    {
        public Collection(int id, int userid, string collectionDate, string assignedCompany) {
            Id = id;
            UserId= userid;
            CollectionDate = collectionDate;
            AssignedCompany = assignedCompany;
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CollectionDate { get; set; }
        public string AssignedCompany { get; set; }
    }
}
