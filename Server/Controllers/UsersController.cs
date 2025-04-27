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
            var usernameAvalible = await _userService.GetByUsernameAsync(registerDto.Username);
            if (usernameAvalible != null)
            {
                return BadRequest("Username already taken.");
            }

            ValidationResult validationResult = await _registerValidator.ValidateAsync(registerDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            User user = new User
            {
                Username = registerDto.Username,
                Password = passwordHash,
                Email = registerDto.Email
            };

            await _userService.AddAsync(user);

            return Ok("User registered successfully");

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _userService.GetByUsernameAsync(loginDto.Username);

            if (user?.Username == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return Unauthorized("Invalid password.");
            }

            string token = _jwtHelperService.GenerateToken(user.Username, user.Id);

            return Ok(new { Token = token });
        }
    }
}
