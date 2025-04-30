using Server.Models.UserModels;
using Server.Models.UserModels.DTO;

namespace Server.Models.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetAsync(string id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task AddAsync(User entity);
        Task UpdateAsync(User entityToUpdate, User entity);
        Task DeleteAsync(User entity);
        Task<RegistrationResult> RegisterAsync(RegisterDTO dto);
        Task<LoginResult> LoginAsync(LoginDTO dto);
    }
}
