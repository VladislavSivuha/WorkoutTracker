namespace WorkoutTracker.Models
{
    public class UserModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }


        public UserModel()
        {

        }
        public UserModel(string login, string password, string email, string name)
        {
            Login = login;
            Password = password;
            Email = email;
            Name = name;
        }
    }
}
