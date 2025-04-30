using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Models.Interface;
using Server.Models.UserModels;
using Server.Models.UserModels.DTO;
using Server.Security.Jwt;

namespace Server.Models.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IValidator<RegisterDTO> _registerValidator;
        private readonly JwtHelperService _jwtHelperService;

        public UserService(JwtHelperService jwtHelperService, IValidator<RegisterDTO> registerValidator, IServiceProvider serviceProvider, ApplicationContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _registerValidator = registerValidator;
            _jwtHelperService = jwtHelperService;
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

        public async Task<User?> GetAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var response = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

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

        public async Task<RegistrationResult> RegisterAsync(RegisterDTO dto)
        {
            var errors = new List<string>();
            var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
            ValidationResult validationResult = await _registerValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                errors.AddRange(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var usernameAvalible = await this.GetUserByUsernameAsync(dto.Username);
            var emailAvalible = await this.GetUserByEmailAsync(dto.Email);

            if (usernameAvalible != null)
            {
                errors.Add("Användarnamn upptaget");
            }
            if (emailAvalible != null)
            {
                errors.Add("Angiven email är redan i bruk");
            }

            if (errors.Count != 0)
            {
                return new RegistrationResult { Succeeded = false, Errors = errors };
            }

            User user = new User
            {
                UserName = dto.Username,
                Email = dto.Email
            };

            var result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return new RegistrationResult
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => e.Description)
                };
            }
            await userManager.AddToRoleAsync(user, "user");
            await this.AddAsync(user);

            return new RegistrationResult
            {
                Succeeded = true,
                UserId = user.Id
            };
        }

        public async Task<LoginResult> LoginAsync(LoginDTO dto)
        {
            var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
            var errors = new List<string>();
            var user = await this.GetUserByUsernameAsync(dto.Username);

            if (user == null || user.UserName == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                errors.Add("Ogiltigt användarnamn eller lösenord");
                return new LoginResult { Succeeded = false, Errors = errors };
            }

            string token = _jwtHelperService.GenerateToken(user);

            return new LoginResult { Succeeded = true, Token = token, UserId = user.Id};
        }
    }
}
