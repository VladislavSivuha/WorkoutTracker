using WorkoutTracker.Interfaces;
using WorkoutTracker.Models;
using BC = BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace WorkoutTracker.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly PgsqlDbContext _context;
        public AuthorizationService(PgsqlDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateAsync(string login, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Login == login);

            if (user == null)
                return null;

            if (!BC.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return user;
        }
        public async Task<User> RegisterAsync(string login, string password, string email, string name)
        {
            string passwordHash = BC.BCrypt.HashPassword(password);

            var newUser = new User(login, passwordHash, email, name);
            
            _context.User.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }
    }
}
