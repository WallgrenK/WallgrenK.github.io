using Microsoft.AspNetCore.Identity;
using Server.Models.SeatingModels;
using System.ComponentModel.DataAnnotations;

namespace Server.Models.UserModels
{
    public class User : IdentityUser
    {
        public Seat? BookedSeat { get; set; }
    }
}
