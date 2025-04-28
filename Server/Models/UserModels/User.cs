using Server.Models.SeatingModels;
using System.ComponentModel.DataAnnotations;

namespace Server.Models.UserModels
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public Seat? BookedSeat { get; set; }
    }
}
