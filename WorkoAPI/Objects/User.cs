namespace WorkoAPI.Objects
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public int balance { get;set; }

        public User(string id, string name, string email, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            //Here Configure user register balance
            balance = 10;
        }
    }
}
