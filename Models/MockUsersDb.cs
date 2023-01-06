namespace ReACT.Models
{
    public class MockUsersDb
    {
        public static readonly List<UserInfo> Users = new()
        {
            new UserInfo(1, "Mike Oxmaul", "123 Dork Street", "mikeoxmaul@gmail.com", "mikeoxmaul"),
            new UserInfo(2, "John Mason", "123 John Street", "johnmason@gmail.com", "johnmason"),
            new UserInfo(3, "Jason Bourne", "123 Spy Street", "jasonbourne@gmail.com", "jasonbourne")
        };

        public List<UserInfo> GetAll()
        {
            return Users.OrderBy(x => x.Id).ToList();
        }

        public UserInfo? GetUser(int id)
        {
            UserInfo? user = Users.FirstOrDefault(x => x.Id == id);
            return user;
        }

        public void AddUser(UserInfo user)
        {
            Users.Add(user);
        }
    }
}
