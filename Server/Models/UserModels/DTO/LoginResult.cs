namespace Server.Models.UserModels.DTO
{
    public class LoginResult : RegistrationResult
    {
        public string? Token { get; set; }
    }
}
