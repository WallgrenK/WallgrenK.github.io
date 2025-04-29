using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models.Interface;
using Server.Models.UserModels;
using Server.Models.UserModels.DTO;
using Server.Security.Jwt;
using System.Security.Claims;

namespace Server.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtHelperService _jwtHelperService;
        private readonly IValidator<RegisterDTO> _registerValidator;

        public UsersController(IValidator<RegisterDTO> registerValidator, IUserService dataRepository, JwtHelperService jwtHelperService)
        {
            _userService = dataRepository;
            _jwtHelperService = jwtHelperService;
            _registerValidator = registerValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();

            return Ok(users);
        }

        // Send JwtToken in Authorization header to access Authorize endpoints.
        [Authorize]
        [HttpGet("currentuser")]
        public IActionResult GetLoggedInUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            return Ok(new { UserId = userId, Username = username });
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetAsync(id);
            if (user is null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            ValidationResult validationResult = await _registerValidator.ValidateAsync(registerDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var usernameAvalible = await _userService.GetUserByUsernameAsync(registerDto.Username);
            var emailAvalible = await _userService.GetUserByEmailAsync(registerDto.Email);
            
            if (usernameAvalible != null)
            {
                return BadRequest("Användarnamn upptaget");
            }
            if (emailAvalible != null)
            {
                return BadRequest("Angiven email är redan i bruk");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            User user = new User
            {
                Username = registerDto.Username,
                Password = passwordHash,
                Email = registerDto.Email
            };

            await _userService.AddAsync(user);

            return Ok("Registrering lyckades!");

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _userService.GetUserByUsernameAsync(loginDto.Username);

            if (user?.Username == null)
            {
                return Unauthorized("Ogiltigt användarnamn eller lösenord");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return Unauthorized("Ogiltigt lösenord.");
            }

            string token = _jwtHelperService.GenerateToken(user.Username, user.Id);

            return Ok(new { Token = token });
        }
    }
}
