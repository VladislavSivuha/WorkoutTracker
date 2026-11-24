using WorkoutTracker.Models;

namespace WorkoutTracker.Interfaces
{
    public interface IAuthorizationService
    {
        Task<User?> AuthenticateAsync(string login, string password);
        Task<User> RegisterAsync(string login, string password, string email, string name);
    }
}
