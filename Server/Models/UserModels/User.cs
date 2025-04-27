using System.ComponentModel.DataAnnotations;

namespace Server.Models.UserModels
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
