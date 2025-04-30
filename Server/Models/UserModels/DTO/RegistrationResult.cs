namespace Server.Models.UserModels.DTO
{
    public class RegistrationResult
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();   
        public string? UserId { get; set; } 
    }
}
