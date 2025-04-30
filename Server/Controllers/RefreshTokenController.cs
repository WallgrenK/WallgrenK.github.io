using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models.Interface;
using Server.Models.SecurityModels.DTO;

namespace Server.Controllers
{
    [Route("api/refresh")]
    [ApiController]
    public class RefreshTokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public RefreshTokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _tokenService.RefreshTokenAsync(request);

            if (!result.Succeeded)
            {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
    }
}
