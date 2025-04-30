using Microsoft.AspNetCore.Identity;
using Server.Models.SeatingModels;
using Server.Models.SecurityModels;
using System.ComponentModel.DataAnnotations;

namespace Server.Models.UserModels
{
    public class User : IdentityUser
    {
        public Seat? BookedSeat { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
