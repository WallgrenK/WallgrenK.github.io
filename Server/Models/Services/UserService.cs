using Microsoft.EntityFrameworkCore;
using Server.Models.Interface;
using Server.Models.UserModels;

namespace Server.Models.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddAsync(User entity)
        {
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync<User>();
        }

        public async Task<User?> GetAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var response = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (response == null) 
            {
                _logger.LogError($"User with username {username} was not found.");
            }

            return response;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var response = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (response == null)
            {
                _logger.LogError($"No user with the email address {email} was found");
            }

            return response;
        }

        public Task UpdateAsync(User entityToUpdate, User entity)
        {
            throw new NotImplementedException();
        }
    }
}
