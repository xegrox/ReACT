namespace ReACT.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string ContactNo { get; set; }

        public string Address { get; set; }

        public List<Collection> Collections { get; set; }
     }
}
