using Server.Models.UserModels;

namespace Server.Models.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task AddAsync(User entity);
        Task UpdateAsync(User entityToUpdate, User entity);
        Task DeleteAsync(User entity);
    }
}
