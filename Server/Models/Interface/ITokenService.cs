using Server.Models.SecurityModels;
using Server.Models.SecurityModels.DTO;

namespace Server.Models.Interface
{
    public interface ITokenService
    {
        Task<RefreshTokenResult> RefreshTokenAsync(RefreshTokenRequest request);
        Task<RefreshToken> GenerateRefreshTokenAsync(string userId);
        Task<RefreshToken?> GetValidRefreshTokenAsync(string token);
        Task InvalidateRefreshTokenAsync(string token);
    }
}
