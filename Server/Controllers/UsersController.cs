using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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


        public UsersController(IUserService dataRepository, JwtHelperService jwtHelperService)
        {
            _userService = dataRepository;
            _jwtHelperService = jwtHelperService;
        }

        [Authorize]
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

        [Authorize]
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userService.GetAsync(id);
            if (user is null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var result = await _userService.RegisterAsync(registerDto);
            if (!result.Succeeded)
            {
                return BadRequest(new { errors = result.Errors });
            }

            return Ok("Registrering lyckades!");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);

            if (!result.Succeeded)
            {
                return Unauthorized(new { errors = result.Errors });
            }
            if (!string.IsNullOrWhiteSpace(result.Token))
            {
                Response.Cookies.Append("jwt", result.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(60)
                });
            }
            if (result.RefreshToken != null)
            {
                Response.Cookies.Append("refreshToken", result.RefreshToken.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
            }
            if (!string.IsNullOrWhiteSpace(result.UserId))
            {
                Response.Cookies.Append("userId", result.UserId, new CookieOptions
                {
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
            }

            return Ok(result);
        }
    }
}
