namespace ReACT.Models
{
    public class UserInfo
    {
        public UserInfo(int id, string name, string address, string email, string password)
        {
            Id = id;
            Name = name;
            Address = address;
            Email = email;
            Password = password;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
