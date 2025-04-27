using Server.Models.UserModels;

namespace Server.Models.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User entity);
        Task UpdateAsync(User entityToUpdate, User entity);
        Task DeleteAsync(User entity);
    }
}
