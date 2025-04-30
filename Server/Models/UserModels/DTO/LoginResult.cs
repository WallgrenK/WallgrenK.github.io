using Server.Models.SecurityModels;

namespace Server.Models.UserModels.DTO
{
    public class LoginResult : RegistrationResult
    {
        public string? Token { get; set; }
        public RefreshToken? RefreshToken { get; set; }
    }
}
