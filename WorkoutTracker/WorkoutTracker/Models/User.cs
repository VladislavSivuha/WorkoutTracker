using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Login), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Должно быть заполнено")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Должно быть заполнено")]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "Должно быть заполнено")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Должно быть заполнено")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Должно быть заполнено")]
        public string Role { get; set; }


        public List<TemplateHeader> TemplateHeaders { get; set; } = new();


        public User()
        {
            
        }
        public User(string login, string password, string email, string name)
        {
            Login = login;
            PasswordHash = password;
            Email = email;
            Name = name;
            Role = "User";
        }
    }
}
