namespace Server.Models.SecurityModels.DTO
{
    public class RefreshTokenResult
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
    }
}
