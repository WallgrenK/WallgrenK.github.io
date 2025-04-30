using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Models.Interface;
using Server.Models.SecurityModels;
using Server.Models.SecurityModels.DTO;
using Server.Models.UserModels;
using Server.Security.Jwt;
using System.Security.Cryptography;

namespace Server.Models.Services
{
    public class TokenService : ITokenService
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        private readonly JwtHelperService _jwtHelperService;
        public TokenService(JwtHelperService jwtHelperService, ApplicationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
            _jwtHelperService = jwtHelperService;
        }

        public async Task<RefreshTokenResult> RefreshTokenAsync(RefreshTokenRequest request)
        {
            List<string> errors = new List<string>();
            var currentToken = await GetValidRefreshTokenAsync(request.RefreshToken);

            if (currentToken == null)
            {
                return new RefreshTokenResult
                {
                    Succeeded = false,
                    Errors = new List<string> { "Invalid or expired refresh token." }
                };
            }

            var user = await _userManager.FindByIdAsync(currentToken.UserId);

            if (user == null)
            {
                errors.Add("No user found");
                return new RefreshTokenResult { Succeeded = false, Errors = errors };
            }

            var newAccessToken = await _jwtHelperService.GenerateToken(user);
            var newRefreshToken = await GenerateRefreshTokenAsync(user.Id);
            await InvalidateRefreshTokenAsync(request.RefreshToken);

            return new RefreshTokenResult
            {
                Succeeded = true,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            };
        }

        public async Task<RefreshToken> GenerateRefreshTokenAsync(string userId)
        {
            var token = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<RefreshToken?> GetValidRefreshTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token && rt.IsActive);
        }

        public async Task InvalidateRefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
            if (refreshToken != null)
            {
                refreshToken.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
